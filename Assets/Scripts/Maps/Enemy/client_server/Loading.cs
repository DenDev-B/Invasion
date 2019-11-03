using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LoadingScene
{
    Menu,Mission,Battle
}

public class Loading : MonoBehaviour
{
    private static LoadingScene _nextScene { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        if(_nextScene == LoadingScene.Menu)
        {
            StartCoroutine(JointLobby());
        }
        if (_nextScene == LoadingScene.Battle)
        {
            PhotonNetwork.networkingPeer.OpJoinLobby(TypedLobby.Default);
        }
    }

    private IEnumerator JointLobby()
    {
        //while (PhotonNetwork.networkingPeer.State != PeerState.ConnectedToMasster)
        while (PhotonNetwork.networkingPeer.State != ClientState.ConnectedToMaster)
            yield return new WaitForFixedUpdate();

        PhotonNetwork.networkingPeer.OpJoinLobby(TypedLobby.Default);      
    }

    void OnJoinedLobby()
    {
        PhotonNetwork.LoadLevel(Config.SceneMenu);
    }

    void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("PVPBattle");
    }

    public static void Load(LoadingScene nextScene)
    {
        _nextScene = nextScene;
        PhotonNetwork.LoadLevel(Config.SceneLoad);
    }        
}
