using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.autoJoinLobby = false;
        PhotonNetwork.ConnectUsingSettings("1.0");
      
    }

    void OnConnectedToMaster()
    {
        Loading.Load(LoadingScene.Menu);
    }
}
