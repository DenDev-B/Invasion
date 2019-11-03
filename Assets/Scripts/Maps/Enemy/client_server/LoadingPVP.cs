using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LoadingScens
{
    Menu,
    Game,
    GameBot,
}

public class LoadingPVP : MonoBehaviour
{
    private static LoadingScens _nextScene { get; set; }

    private bool _isControllable;

    // Start is called before the first frame update
    void Start()
    {
      //  if (_nextScene == LoadingScens.Menu)
      //  {
         //   PhotonNetwork.networkingPeer.OpJoinLobby(TypedLobby.Default);
       // }
    }

   

    void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        PhotonNetwork.LoadLevel(/*Config.SceneLobby*/"Menu");
    }

    void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        if(_nextScene== LoadingScens.GameBot)
            PhotonNetwork.LoadLevel(/*Config.SceneGame*/"PVPDefense");
        else
            PhotonNetwork.LoadLevel(/*Config.SceneGame*/"PVPBattle");
    }

    public static void Load(LoadingScens nextScene)
    {
        _nextScene = nextScene;
        PhotonNetwork.LoadLevel("LoadingPVP");
    }
}
