using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PunTeams;

public class NetworkPlayer : Photon.MonoBehaviour
{
    public GameObject Visual;
    public GameObject camera;
 
    public struct NetworkState
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public double Timestamp;

        public NetworkState(Vector3 pos, Quaternion rotatinon, double timestamp)
        {
            Position = pos;
            Rotation = rotatinon;
            Timestamp = timestamp;
        }
    }
    private bool player_visible;

    private NetworkState[] _stateBuffer = new NetworkState[20];
    private int _stateCount;
    public float InterpolationBackTime = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        player_visible = false;

        if (!photonView.isMine && PhotonNetwork.room != null && !PhotonNetwork.offlineMode)
        {

            gameObject.tag = "Enemy_team";
            gameObject.GetComponent<Setting>().photonID = photonView.ownerId; 
        }
        else
        {
               gameObject.name = PhotonNetwork.player.name;
                   gameObject.GetComponent<Setting>().photonID = photonView.ownerId;
                 Visual.GetComponent<Setting>().comanda = PVPBattle.instance.getTeam();
                 Visual.GetComponent<Setting>().name = PhotonNetwork.player.name;

                 Visual.AddComponent<HeroBttnComponent>();
               PVPBattle.instance.onPlayer(gameObject);

       }
        chacTag();

   }

    private void chacTag()
    {
        if (PVPBattle.instance && PVPBattle.instance.getTeam() == 0)
            return;

        GameObject[] enim = GameObject.FindGameObjectsWithTag("Enemy_team");
        if(enim.Length >0)
        {
         /*   for (int pp=0; pp < PhotonNetwork.otherPlayers.Length; pp++)
            {
                PhotonPlayer oPlayer = PhotonNetwork.otherPlayers[pp];
            }*/
            for (int en=0;en < enim.Length; en++)
            {
                GameObject eGo = enim[en];
                Setting eSett=  eGo.GetComponent<Setting>();
             //   int eID = eSett.player.GetComponent<PhotonView>().;
                int eID = eSett.photonID;

                for (int pp = 0; pp < PhotonNetwork.otherPlayers.Length; pp++)
                {
                    PhotonPlayer oPlayer = PhotonNetwork.otherPlayers[pp];
                   
                    if (oPlayer.ID == eSett.photonID)
                    {
                        eGo.name = oPlayer.NickName;
                        if (oPlayer.GetTeam() == PhotonNetwork.player.GetTeam())
                        {
                            eGo.tag = "Player_Friend";
                        }
                      else
                        {
                            eGo.tag = "Player_Enemy";
                        }
                    }
                }
            }
        }
   
    }

    private void ReceiveState(NetworkState state)
   {
       for (int i = _stateBuffer.Length - 1; i > 0; i--)
       {
           _stateBuffer[i] = _stateBuffer[i - 1];
       }
       _stateBuffer[0] = state;
       _stateCount = Mathf.Min(_stateCount + 1, _stateBuffer.Length);
   }

   public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
   {
       if (stream.isWriting)
       {
           stream.SendNext(transform.position);
           stream.SendNext(transform.rotation);
       }
       else
       {
           var pos = (Vector3)stream.ReceiveNext();
           var rot = (Quaternion)stream.ReceiveNext();
           ReceiveState(new NetworkState(pos, rot, info.timestamp));
       }
   }

   // Update is called once per frame
   void Update()
   {
       /*  if (photonView.isMine && PhotonNetwork.room != null)
         {
             if (!player_visible && GetComponent<Setting>().comanda > 0)
             {
                 player_visible = true;
                 int com = GetComponent<Setting>().comanda;
                 GameObject.Find("TestRPC").GetComponent<Text>().text = "comand: " + com;
                 //    Visual.SetActive(true);
                 Vector3 _pos = Vector3.zero;
                 if (com == 1)
                     _pos = Game.teamApos.position;
                 if (com == 2)
                     _pos = Game.teamBpos.position;
                 GetComponent<Setting>().player.transform.position = new Vector3(Random.Range(_pos.x - 2f, _pos.x + 2f), 0f, Random.Range(_pos.z - 2f, _pos.z + 2f));
                 Visual.AddComponent<HeroBttnComponent>();
                 gameObject.name = PhotonNetwork.playerName;
             }
         }  */
         if (photonView == null || photonView.isMine)
            return;

        if (_stateCount == 0)
            return;
    

        var currentTime = PhotonNetwork.time;
        var interpolationTime = currentTime - InterpolationBackTime;

        if (_stateBuffer[0].Timestamp > interpolationTime)
        {
            for (int i = 0; i < _stateCount; i++)
            {
                if (_stateBuffer[i].Timestamp <= interpolationTime || i == _stateCount - 1)
                {
                    //the state closest to network time
                    var lhs = _stateBuffer[i];

                    //the state one slot newer
                    var rhs = _stateBuffer[Mathf.Max(i - 1, 0)];

                    //use time between lhs and rhs to interpolate
                    var length = rhs.Timestamp - lhs.Timestamp;

                    var t = 0f;
                    if (length > 0.0001)
                    {
                        t = (float)((interpolationTime - lhs.Timestamp) / length);
                    }
                    if (Vector3.Distance(lhs.Position, rhs.Position) > 5f)
                        transform.position = rhs.Position;
                    else
                        transform.position = Vector3.Lerp(lhs.Position, rhs.Position, t);

                    transform.rotation = Quaternion.Lerp(lhs.Rotation, rhs.Rotation, t);
                    break;
                }
            }
        }
        else
        {
            //Logger.(no timestamp)
            var lhs = _stateBuffer[0];

            if (Vector3.Distance(lhs.Position, lhs.Position) > 2f)
                transform.position = lhs.Position;
            else
                transform.position = Vector3.Lerp(lhs.Position, lhs.Position, 0.1f);

            transform.rotation = Quaternion.Lerp(lhs.Rotation, lhs.Rotation, 0.1f);
        }
    }
}
