using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using Unity.Entities;
using UnityEngine.UI;

public class Boot : MonoBehaviour
{
    public static bool batle;
    public static bool STEP; //true - hero /// false - comp;   
    public static GameObject PlauerInfo;
    public GameObject map;
    public int playerOnMap;
    public GameObject DownPanel;
    public static GameObject cam;
    private static GameObject batlePanel;
    
    public bool PVPbattle = false;
    public static bool PVP;

    private static EntityManager entityManager;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        PVP = PVPbattle;
        //   cam = cc[0];
        PlauerInfo = GameObject.Find("PlauerInfo");
        batlePanel = GameObject.Find("BatleButtonPanel"); ;
        //  entityManager = World.Active.GetOrCreateManager<EntityManager>();

        //     LOADSAVE();
        // GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        // playerOnMap = players.Length; 
        //   CreateHero();
        // Debug.Log("Start Boot");
    }

    public static void onBatle(bool st=false)
    {

        batle = true;
        if (Boot.PVP)
            return;
        batlePanel.transform.localPosition = new Vector3(225f, 90f, 0f);
        batlePanel.GetComponent<BattlePanel>().nextStep(st);
    }

    public static void offBatle()
    {
        if (Boot.PVP)
            return;

        batle = false;
        batlePanel.transform.localPosition = new Vector3(225f, -200f, 0f);
    }
    public static void onStep(bool st = false)
    {
       /* if (Boot.PVP)
            return;*/

        if (st)
        {
            STEP = true;
            batlePanel.GetComponentInChildren<Text>().text = "завершить ход";
        }
        else
        {
            STEP = false;
            batlePanel.GetComponentInChildren<Text>().text = "ходит компьютер";
        }
    }
    /*
    private void CreateHero()
    {
        GameObject[] spawnZone = GameObject.FindGameObjectsWithTag("SpawnHero");
        Debug.Log("SpawnHero " + spawnZone.Length);
    }
    */

    /*[Serializable]        
   public class Data
   {
       public string name;
       public string bamalu;
   }
   Data data;
   */
    /* private void LOADSAVE()
     {
         // Load.SaveJson(data);
         // data=   Load.LoadingJSSON<Data>("/Map/Test1.json");

         //data = LoadBinary.Load<Data>();

         //  SaveBinary.Save(data);
         //data=LoadBinary.Load(ref data);
     }*/



}
