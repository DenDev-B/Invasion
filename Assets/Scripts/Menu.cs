using System.Collections;
using UnityEngine;
using System.IO;
using System;

public class Menu : MonoBehaviour
{
    public enum MenuState
    {
        Home,
        Options,
        Mission,
        Create,
        Battle,
        Lobby,
        Exit,
    }

    private MenuState _state;

    private int _maxPlayerCount = 10;
    public string MapName = "Map";
    public string PlayerName = "Player_";


    public Texture2D menustart;//, menusingle, menubattle, menuoption, menusand;
 

    //public string file_save = "SandSave/SandSave.kaa";

    private int visible = 0;
    private int menu = 0;
    private float x, y, xrat, yrat;

    
    private void Start()
    {
        Cursor.visible = true;
        xrat = Screen.width / 100;
        yrat = Screen.height / 100;
        _state = MenuState.Home;
        var  k =Math.Round( UnityEngine.Random.Range(1f, 60f));
        PlayerName += k;
    }

  

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), menustart);

        switch (_state)
        {
            case MenuState.Home:
                DrawHome();
                break;
            case MenuState.Options:
                DrawOptions();
                break;
            case MenuState.Mission:
                DrawMission();
                break;
            case MenuState.Create:
                DrawCreate();
                break;
            case MenuState.Battle:
                DrawBattle();
                break;
            case MenuState.Lobby:
                DrawLobby();
                break;
            case MenuState.Exit:
                Application.Quit();
            
                break;
        }
       

    }

    private void DrawOptions()
    {
        Debug.Log(" mission ");
        Application.LoadLevel("CreateHero");
    }
    #region Server   
    private void DrawBattle()
    {
          Debug.Log("JoinRoom");
           PhotonNetwork.playerName = PlayerName;
        RoomOptions MayRoom = new RoomOptions();
        MayRoom.MaxPlayers = 20;
          PhotonNetwork.JoinOrCreateRoom("testRoom", MayRoom, TypedLobby.Default);
          LoadingPVP.Load(LoadingScens.Game);
    }
    #endregion

    private void DrawMission()
    {
        Debug.Log(" mission " );
        Application.LoadLevel("LoadScence");
    }

    private void DrawLobby()
    {
       
    }

    private void DrawHome()
    {
     //   if (GUI.Button(new Rect(xrat * 8, yrat * 22, xrat * 20, yrat * 8), "Редактировать игрока"))
        //    _state = MenuState.Mission;
      //  if (GUI.Button(new Rect(xrat * 8, yrat * 22, xrat * 20, yrat * 8), "Одиночная игра"))
        //    _state = MenuState.Mission;

        if (GUI.Button(new Rect(xrat * 8, yrat * 32, xrat * 20, yrat * 8), "Сетевой батл"))
            _state = MenuState.Create;
     //   if (GUI.Button(new Rect(xrat * 4, yrat * 42, xrat * 20, yrat * 8), "подключится"))
      //      _state = MenuState.Lobby;
    }


    void DrawCreate()
    {
        if (GUI.Button(new Rect(xrat * 8, yrat * 22, xrat * 20, yrat * 8), "Войти"))
            _state = MenuState.Battle;
        if (GUI.Button(new Rect(xrat * 8, yrat * 62, xrat * 20, yrat * 8), "Назад"))
            _state = MenuState.Home;

      //  GUI.Box(new Rect(xrat * 45, yrat * 22, xrat * 20, yrat * 8), "Имя");
          GUI.Label(new Rect(xrat * 40, yrat * 22, xrat * 20, yrat * 8),"Имя");
      
    
         PlayerName = GUI.TextField(new Rect(xrat * 50, yrat * 22, xrat * 20, yrat * 8), PlayerName, 25);

        /*  Debug.Log("JoinRoom");
          PhotonNetwork.JoinOrCreateRoom("testRoom", new RoomOptions { }, TypedLobby.Default);
          LoadingPVP.Load(LoadingScens.Game);*/
    }


}
