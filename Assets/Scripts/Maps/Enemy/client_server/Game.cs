using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Chat;
using static PunTeams;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PhotonView))]
public class Game : Photon.MonoBehaviour
{
    public bool Bots;
    public Transform teamApos;
    public Transform teamBpos;
    private bool _isLeaving;
    public Text TxtInfos;

    public Text yourComand;
    public Text enimyComand;
    public Text comandKill;
    public Text enimyKill;
    public Text youKill;
    private int aKill = 0;
    private int bKill = 0;
    private int yKill = 0;

    //  public Text TxtTimer;
    private static Game _instance;
    public static Game Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<Game>();
            return _instance;
        }
    }
    public Tower Tower;


    bool startTimer = false;
    int timerIncrementValue;
    double startTime;
    private double timer = 15;

    public bool IsRoundStart { get; private set; }
    //   public int Team;

    private Room Room { get { return PhotonNetwork.room; } }
    private int CurrentWave
    {
        get
        {
            //   Debug.Log("wave " + Room.customProperties["wave"]);
            return (int)Room.customProperties["wave"];
        }
        set
        {
            Room.customProperties["wave"] = value;
            photonView.RPC("onWave", PhotonTargets.Others, value);
        }
    }

    //  private Hashtable RoomProperties { get { return Room.customProperties; } }
    private PhotonPlayer[] Players { get { return PhotonNetwork.playerList; } }

    void Start()
    {
        if (!teamApos || !teamBpos)
            return;
        /*  PhotonNetwork.player.SetCustomProperties(new Hashtable
          {
              {"team",0 },
          });*/
        /*  string value = (string)PhotonNetwork.player.customProperties["HeroView"];
          Debug.Log(" hero "+ value);

          string value2 = (string)PhotonNetwork.player.customProperties["lWeapon"];
          Debug.Log(" lWeapon " + value2);

          string value3 = (string)PhotonNetwork.player.customProperties["rWeapon"];
          Debug.Log(" rWeapon " + value3);*/

        if (PhotonNetwork.player.IsMasterClient)
        {
            double _timer = timer;
            timer = 5;
            Hashtable CustomeValue = new ExitGames.Client.Photon.Hashtable();
            startTime = PhotonNetwork.time + timer;
            timer = _timer;
            startTimer = true;

            CustomeValue.Add("StartTime", startTime);
            PhotonNetwork.room.SetCustomProperties(CustomeValue);
            // Boot.onBatle(false);
        }
        else
        {
            startTime = double.Parse(PhotonNetwork.room.CustomProperties["StartTime"].ToString());
            startTimer = true;
        }

        if (PhotonNetwork.room != null)
        {
            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
                //  messageInfo.sender.SetTeam(team);
                AddToTeam(1);
            }
            else
                photonView.RPC("AddToRandomTeam", PhotonTargets.MasterClient);
        }

    }

    private void SpawnPlayer()
    {

        if (PVPBattle.instance.getTeam() == 1) //разворот камеры для команды А
        {
            GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
            if (cam)
            {
                //cam.transform.rotation=new Vector3(cam.transform.rotation.x, cam.transform.rotation.y, cam.transform.rotation.z);
                float yRotation = -70f;
                float xRotation = 50f;
                float zRotation = 0f;
                cam.transform.eulerAngles = new Vector3(xRotation, yRotation, zRotation);
            }
            //   cam.GetComponent<CameraControl>().dXYZ= new Vector3(7f, 0f, 7f);
            cam.GetComponent<CameraControl>().dXYZ = new Vector3(9f, 0f, -2f);
        }
        if (PhotonNetwork.room != null && !PhotonNetwork.offlineMode)
        {
            string value = (string)PhotonNetwork.player.customProperties["HeroView"];
            Debug.Log(" hero " + value);
            value += "PVP";
            PhotonNetwork.Instantiate(value, GetRandomPlayerPosition(), Quaternion.identity, 0);
            Debug.Log("players " + PhotonNetwork.playerList.Length);

            //  PhotonNetwork.Instantiate("SaraBeckerPVP", GetRandomPlayerPosition(), Quaternion.identity, 0);
        }
        else
        {
            //    Debug.Log("Off Line");
            //  GameObject obj = Resources.Load<GameObject>("BodyguardPVP") as GameObject;
            //   Instantiate(Resources.Load<GameObject>("BodyguardPVP"), GetRandomPlayerPosition(), Quaternion.identity);
        }

    }


    private byte newTeam = 0;
    [PunRPC]
    public void AddToRandomTeam(PhotonMessageInfo messageInfo)
    {
        if (PhotonNetwork.isMasterClient)
        {
            //  Debug.Log("AddToRandomTeam "+ messageInfo);
            var team = PunTeams.Team.red;
            var count = PhotonNetwork.room.PlayerCount;


            if (PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count <= PunTeams.PlayersPerTeam[PunTeams.Team.red].Count)
                team = PunTeams.Team.blue;
            if (Bots)
                team = PunTeams.Team.blue;
            messageInfo.sender.SetTeam(team);

            if (team == PunTeams.Team.blue)
                newTeam = 1;
            else
                newTeam = 2;



            photonView.RPC("AddToTeam", messageInfo.sender, newTeam);
            photonView.RPC("resetKillCommand", messageInfo.sender, aKill, bKill);
            //    int cc = CurrentWave;
            photonView.RPC("onWave", messageInfo.sender, CurrentWave);
            photonView.RPC("onTeak", messageInfo.sender, _time, _step);
        }
    }

    [PunRPC]
    public void onWave(int _wave)
    {
        CurrentWave = _wave;
    }
    [PunRPC]
    public void AddToTeam(byte team)
    {
        // Debug.Log("Add Team "+ team);
        PVPBattle.instance.onTeam(team);
        SpawnPlayer();
    }

    public void botPlayerAttack(int pID, int dmg)
    {
        Debug.Log("botPlayerAttack " + pID + " " + dmg);
        photonView.RPC("MobAttackPlaerID", PhotonTargets.MasterClient, pID, dmg);
    }
    [PunRPC]
    public void MobAttackPlaerID(int pID, int dmg)
    {
        Debug.Log("Bot attack " + pID + " " + dmg);
        Debug.Log("PhotonNetwork.playerList " + PhotonNetwork.playerList.Length);
        PhotonPlayer oPlayer;
        for (int pp = 0; pp < PhotonNetwork.playerList.Length; pp++)
        {
            if (PhotonNetwork.playerList[pp].ID == pID)
            {
                oPlayer = PhotonNetwork.playerList[pp];
                Debug.Log("Bot attack onDmg" + pID + " " + dmg);
                photonView.RPC("onDmg", oPlayer, dmg, 0, true);
                break;
            }
        }
    }


    public void playerAttack(int pID, int min, int max, int pr)
    {
        //  Debug.Log("Attack "+pID+"  m:"+min+ "  M:"+max+"  p:"+pr);
        photonView.RPC("AttackPlaerID", PhotonTargets.MasterClient, pID, min, max, pr, PhotonNetwork.player.ID);
    }

    [PunRPC]
    public void AttackPlaerID(int pID, int min, int max, int pr, int who)
    {
        //   Debug.Log("Master Attack " + pID + "  m:" + min + "  M:" + max + "  p:" + pr+" w:"+who);
        var dmg = (pr * max) / 100;
        if (dmg < min)
            dmg = min;
        if (dmg > max)
            dmg = max;
        if (pr < 20)
            dmg = 0;
        PhotonPlayer oPlayer;
        for (int pp = 0; pp < PhotonNetwork.playerList.Length; pp++)
        {
            if (PhotonNetwork.playerList[pp].ID == pID)
            {
                oPlayer = PhotonNetwork.playerList[pp];
                photonView.RPC("onDmg", oPlayer, dmg, who,false);
                break;
            }
        }
        //     photonView.RPC("onDmg", oPlayer, dmg);
    }

    [PunRPC]
    public void onKill(int who)
    {
        //  Debug.Log("kill " + who);
        if (!PhotonNetwork.isMasterClient)
            return;
        PhotonPlayer oPlayer = null;
        int id;//, a=0, b=0;
        for (int pl = 0; pl < PhotonNetwork.playerList.Length; pl++)
        {
            if (PhotonNetwork.playerList[pl].ID == who)
            {
                id = pl;
                oPlayer = PhotonNetwork.playerList[pl];
                if (oPlayer.GetTeam() == PunTeams.Team.blue)
                    aKill++;
                else
                    bKill++;
                break;
            }
        }
        photonView.RPC("resetKillCommand", PhotonTargets.All, aKill, bKill);

        if (oPlayer is PhotonPlayer)
            photonView.RPC("addKill", oPlayer);

    }

    [PunRPC]
    public void resetKillCommand(int a, int b)
    {
        /*  Debug.Log("resetKillCommand " + a + "  " + b);
          aKill += a;
          bKill += b;*/
        aKill = a;
        bKill = b;
        comandKill.text = "Убили: " + a;
        enimyKill.text = "Убили: " + b;
    }
    [PunRPC]
    public void addKill()
    {
        yKill++;
        youKill.text = "Вы убили " + yKill;
    }

    [PunRPC]
    public void onDmg(int pr, int who, bool bot )
    {
       // Debug.Log("You dd " + pr);
        LifeComponent life = PVPBattle.instance.player.GetComponent<LifeComponent>();
        //   Debug.Log("You game life " + life.life);
        if (life.life <= 0)
            return;
        if (bot)
        {
            //очко ботам
            
        } else
        if (pr > life.life)
        {
            // Debug.Log("You onKill " + life.life);
            photonView.RPC("onKill", PhotonTargets.MasterClient, who);
        }
        if (PVPBattle.instance.player.GetComponent<LifeComponent>())
            PVPBattle.instance.player.GetComponent<LifeComponent>().onPVPPlayerDmg(pr, dellPlayer);
    }


    
    private void dellPlayer()
    {
        // Debug.Log("You dell ");
        Setting sett = PVPBattle.instance.player.GetComponent<Setting>();
        sett.view.GetComponent<Animator>().SetBool("dead", true);

        if (Bots)
        {
            bKill++;
            photonView.RPC("resetKillCommand", PhotonTargets.All, aKill, bKill);
        }
        //PhotonView pv  = PVPBattle.instance.player.GetComponent<PhotonView>();
        //  pv.enabled = false;

        ActiveComponent ac = PVPBattle.instance.player.GetComponent<ActiveComponent>();
        ac.enabled = false;

        HumanoidComponent hc = PVPBattle.instance.player.GetComponent<HumanoidComponent>();
        hc.enabled = false;

        NetworkPlayer np = PVPBattle.instance.player.GetComponent<NetworkPlayer>();
        np.enabled = false;

        sett.GetComponent<HumanoidComponent>().enabled = false;
        GameObject.FindGameObjectWithTag("App").GetComponent<Boot>().DownPanel.GetComponent<HeroButtons>().clearBttn();
        PVPBattle.instance.player.tag = "Box";
        PVPBattle.instance.player.name = "Trup";

        //   sett.GetComponent<LifeComponent>().enabled = false;
        sett.enabled = false;
        SpawnPlayer();
    }

    public Vector3 GetRandomPlayerPosition(bool bots = false)
    {
        var pos = new Vector3(100f, 100f, 100f);

        var _pos = Vector3.zero;
        if (PVPBattle.instance.getTeam() == 1)
            _pos = teamApos.position;
        if (PVPBattle.instance.getTeam() == 2)
            _pos = teamBpos.position;
        float dx = 2f;
        if (bots)
        {
            _pos = teamBpos.position;
            dx = 4f;
        }
        if (Vector3.zero != _pos)
        {
            pos = new Vector3(UnityEngine.Random.Range(_pos.x - dx, +_pos.x + dx), 0f, UnityEngine.Random.Range(_pos.z - dx, _pos.z + dx));
        }

        return pos;
    }

    void Update()
    {
    /*   if (Input.GetKeyUp(KeyCode.Escape) && !_isLeaving)
        {
            _isLeaving = true;
            PhotonNetwork.LeaveRoom();
        }*/
        if (PhotonNetwork.isMasterClient && PhotonNetwork.room != null)
        {
            UpdateGameLogic();
            checkTime();
        }
        if (PhotonNetwork.connectionStateDetailed.ToString() != "Joined")
        {
            TxtInfos.text = "Not Connect";
        }
        else
        {
            TxtInfos.text = "Room: " + PhotonNetwork.room.name + "    Online: " + PhotonNetwork.room.playerCount + "   Master: " + PhotonNetwork.isMasterClient;
        }

    }

    private int _time;
    public byte _step = 0;
    private void checkTime()
    {
        if (!startTimer) return;

        timerIncrementValue = (int)Math.Round(startTime - (double)PhotonNetwork.time, 0);
        if (_time != timerIncrementValue)
        {
            _time = timerIncrementValue;
            photonView.RPC("onTeak", PhotonTargets.All, _time, _step);
        }
        if (timerIncrementValue <= 0)
        {
            startTime = PhotonNetwork.time + timer;
            if (_step == 1)
                _step = 2;
            else
                _step = 1;

            if (Bots && _step == 2)
            {
                var mobs = GameObject.FindGameObjectsWithTag("Enemy");
                if (mobs.Length > 0)
                    for (int m = 0; m < mobs.Length; m++)
                    {
                        mobs[m].GetComponent<SettingsEnimy>().od = mobs[m].GetComponent<SettingsEnimy>().maxOD;
                    }
            }

        }
    }
    [PunRPC]
    public void onWave(byte _wave)
    {
        CurrentWave = _wave;
    }
    [PunRPC]
    public void onTeak(int _time, byte _step)
    {
        ShowKommand();
        PVPBattle.instance.onStep(_step);
        PVPBattle.instance.onTime(_time);
    }

    //показываем состав команд
    private void ShowKommand()
    {
        string strA = "КОМАНДА А";
        for (int bl = 0; bl < PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count; bl++)
        {
            strA += "\n " + PunTeams.PlayersPerTeam[PunTeams.Team.blue][bl].NickName;
        }
        yourComand.text = strA;

        if (Bots)
            return;
        string strB = "КОМАНДА В";
        for (var rd = 0; rd < PunTeams.PlayersPerTeam[PunTeams.Team.red].Count; rd++)
        {
            strB += "\n " + PunTeams.PlayersPerTeam[PunTeams.Team.red][rd].NickName;
        }
        enimyComand.text = strB;
    }

    private void UpdateGameLogic()
    {
        if (!Bots)
            return;
        if (CurrentWave == -1)
        {
            SetNewWave(0);
        }
        else
        {
            var mobs = GameObject.FindGameObjectsWithTag("Enemy");
            if (mobs.Length == 0)
            {
                SetNewWave(CurrentWave + 1);
            }
        }
        //if(RoomProperties.)
        // RoundStarted();
        //  if (PhotonNetwork.playerList.Length > 1)
        //     Debug.Log("asd");
        //   for(int pr=1;pr< 2;pr++)
        //      PhotonNetwork.playerList[1]
        // this.photonView.RPC("RoundStarted", PhotonTargets.OthersBuffered);
        //
    }
    private void SetNewWave(int waveNum)
    {

        waveNum = Mathf.Min(waveNum, WawesManager.Waves.Count - 1);

        var wave = WawesManager.Waves[waveNum];
        CurrentWave = waveNum;

        for (int i = 0; i < wave.MobCount; i++)
        {
            CreateMob();
        }
    }

    private void CreateMob()
    {
        if (PhotonNetwork.room != null && !PhotonNetwork.offlineMode)
          PhotonNetwork.InstantiateSceneObject("ZombiePVP", GetRandomPlayerPosition(true), Quaternion.identity, 0, null);
         //  PhotonNetwork.Instantiate("ZombiePVP", GetRandomPlayerPosition(true), Quaternion.identity, 0);
        //     Debug.Log("players " + PhotonNetwork.playerList.Length);
    }

    public void ExitToMenu()
    {
        _isLeaving = true;
        PhotonNetwork.LeaveRoom();
    }

    //покинули комнату
    void OnLeftRoom()
    {
        //  Debug.Log("OnLeftRoom");
        PhotonNetwork.Disconnect();
        Application.LoadLevel(Config.SceneLogin);
    }

    //подсоединились к мастеру серверу
    void OnConnectedToMaster()
    {
        //  Debug.Log("Game OnConnectedToMaster");
        LoadingPVP.Load(LoadingScens.Menu);
    }

    public void killBot()
    {
        photonView.RPC("onKillBot", PhotonTargets.MasterClient);
    }

    [PunRPC]
    public void onKillBot()
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        aKill++;
        photonView.RPC("resetKillCommand", PhotonTargets.All, aKill, bKill);
    }


    public void onDmgBot(int id, int dmg)
    {
        if (PhotonNetwork.room != null && !PhotonNetwork.offlineMode)
        {
            photonView.RPC("onDmgBotMaster", PhotonTargets.MasterClient,id,dmg);
        }



    }
        [PunRPC]
        public void onDmgBotMaster(int id, int dmg)
        {
            if (!PhotonNetwork.isMasterClient)
                return;

            var mobs = GameObject.FindGameObjectsWithTag("Enemy");
            if (mobs.Length > 0)
                for (int m = 0; m < mobs.Length; m++)
                {
                    if (mobs[m].GetComponent<PhotonView>().viewID == id)
                    {
                        if (mobs[m].GetComponent<LifeComponent>().life > 0)
                        {
                            mobs[m].GetComponent<LifeComponent>().life -= dmg;
                          float lf = mobs[m].GetComponent<LifeComponent>().life;
                            photonView.RPC("onLifeBot", PhotonTargets.Others, id, lf);
                            if (mobs[m].GetComponent<LifeComponent>().life <= 0)
                            {
                                mobs[m].GetComponent<Mob>().dellBot();
                                
                                break;
                             }

                        }

                    }
                }
            //     photonView.RPC("resetKillCommand", PhotonTargets.All, aKill, bKill);
           }
        [PunRPC]
        public void onLifeBot(int id, float _life)
        {
            var mobs = GameObject.FindGameObjectsWithTag("Enemy");
            if (mobs.Length > 0)
            {
            for (int m = 0; m < mobs.Length; m++)
            {
                if (mobs[m].GetComponent<PhotonView>().viewID == id)
                {
                   mobs[m].GetComponent<LifeComponent>().life = _life;
                }
            }
        }
        }




}
