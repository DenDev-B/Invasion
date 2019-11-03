using ExitGames.Client.Photon;
using System;
using UnityEngine;

public class Nmenu : MonoBehaviour
{
    public enum MenuState
    {
        Home,
        Options,
        Mission,
        Create,
        BattleZ,
        Lobby,
        Exit,
    }

    public Transform createHero;

    private float x, y, xrat, yrat;
    private MenuState _state;

    private int _maxPlayerCount = 20;
    public string MapName = "Map";
    public string PlayerName = "Player_";

    private Vector3 createHeroPos;
    public float speed=5f;
    // Start is called before the first frame update
    void Start()
    {
        createHeroPos = createHero.localPosition;

        Cursor.visible = true;
        xrat = Screen.width / 100;
        yrat = Screen.height / 100;
        _state = MenuState.Home;
        var k = Math.Round(UnityEngine.Random.Range(1f, 60f));
        PlayerName += k;

       
    }

    // Update is called once per frame
    void Update()
    {
        if (createHeroPos != createHero.localPosition)
        {
           createHero.localPosition = Vector3.Lerp(createHero.localPosition, createHeroPos, speed*Time.deltaTime);
        }
    }


    void OnGUI()
    {
        switch (_state)
        {
            case MenuState.Home:
                DrawHome();
                break;
            case MenuState.Create:
                DrawCreate();
                break;
              case MenuState.Mission:
                  DrawMission();
                  break;
              case MenuState.BattleZ:
                  DrawCreate2();
                 break;

            /*      case MenuState.Battle:
                     DrawBattle();
                     break;
                 case MenuState.Lobby:
                     DrawLobby();
                     break;*/
            case MenuState.Exit:
                Application.Quit();

                break;
        }

    }
    private void DrawCreate2()
    {
        GUI.Box(new Rect(xrat * 8, yrat * 22, xrat * 16, yrat * 36), "Menu");
        if (GUI.Button(new Rect(xrat * 9, yrat * 27, xrat * 14, yrat * 6), "Play"))
            //_state = MenuState.Create;
            onJoinPVP(true);
        if (GUI.Button(new Rect(xrat * 9, yrat * 50, xrat * 14, yrat * 6), "Back"))
            _state = MenuState.Home;

        showCreate(true);

    }

    private void DrawCreate()
    {
        GUI.Box(new Rect(xrat * 8, yrat * 22, xrat * 16, yrat * 36), "Menu");
        if (GUI.Button(new Rect(xrat * 9, yrat * 27, xrat * 14, yrat * 6), "Play"))
            //_state = MenuState.Create;
            onJoinPVP();
        if (GUI.Button(new Rect(xrat * 9, yrat * 50, xrat * 14, yrat * 6), "Back"))
            _state = MenuState.Home;

        showCreate(true);
        
    }
    public int score = 0;
    public Hashtable hash;
    private void onJoinPVP(bool bots =false)
    {
        Debug.Log("JoinRoom");
        PhotonNetwork.playerName = PlayerName;
        
        RoomOptions MayRoom = new RoomOptions();
        //  MayRoom.maxPlayers = (byte)8;
        //    MayRoom.MaxPlayers =(byte)8;
        RoomOptions roomOptions = new RoomOptions() { isVisible = true, maxPlayers = 6 };
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        roomOptions.CustomRoomProperties.Add("wave", -1);
        
        // hash = new Hashtable();
        //  hash.Add("score", score);
        //  PhotonNetwork.player.SetCustomProperties(hash);
        NewHero nh = createHero.GetComponent<NewHero>();

        Hashtable setPlayerHW = new Hashtable() { { "HeroView", nh.myList[nh.num].ava } };
        PhotonNetwork.player.SetCustomProperties(setPlayerHW);

        Hashtable setPlayerLW = new Hashtable() { { "lWeapon", nh.myWeap[nh.numL].name } };
        PhotonNetwork.player.SetCustomProperties(setPlayerLW);

        Hashtable setPlayerRW = new Hashtable() { { "rWeapon", nh.myWeap[nh.numR].name } };
        PhotonNetwork.player.SetCustomProperties(setPlayerRW);
        if (bots)
        {
            PhotonNetwork.JoinOrCreateRoom("testRoom2", roomOptions, TypedLobby.Default);
            LoadingPVP.Load(LoadingScens.GameBot);
        }
        else
        {
            PhotonNetwork.JoinOrCreateRoom("testRoom", roomOptions, TypedLobby.Default);
            LoadingPVP.Load(LoadingScens.Game);
        }
    }

    private void showCreate(bool state=false)
    {
        if(state)
            //  createHero.localPosition = new Vector3(0f, 0f, 0f);
            createHeroPos = new Vector3(0f, 0f, 0f);
        else
            //  createHero.localPosition = new Vector3(0f, -750f, 0f);
            createHeroPos = new Vector3(0f, -750f, 0f);

    }

    private void DrawMission()
    {
        Debug.Log(" mission ");
        Application.LoadLevel("LoadScence");
    }

    private void DrawHome()
    {
        showCreate();
        //createHero.transform.position = new Vector3(0f, -750f, 0f);
        GUI.Box(new Rect(xrat * 8, yrat * 22, xrat * 16, yrat * 36), "Menu");
        if (GUI.Button(new Rect(xrat * 9, yrat * 27, xrat * 14, yrat * 6), "Battle"))
            _state = MenuState.Create;
        if (GUI.Button(new Rect(xrat * 9, yrat * 34, xrat * 14, yrat * 6), "Defense"))
            _state = MenuState.BattleZ;
        /*  if (GUI.Button(new Rect(xrat * 9, yrat * 36, xrat * 14, yrat * 6), "Mission"))
              _state = MenuState.Mission;*/

        if (GUI.Button(new Rect(xrat * 9, yrat * 50, xrat * 14, yrat * 6), "Exit"))
            Application.Quit();
    }
}
