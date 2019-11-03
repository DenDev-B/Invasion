using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PVPBattle : MonoBehaviour
{
    public static PVPBattle instance;

    [SerializeField]
    private byte step;
    [SerializeField]
    private byte myTeam=0;         //Comand A || Command B

    public GameObject player;
    [SerializeField]
    private Text TxtTimer;
  /*  [SerializeField]
    private Text TxtStep;*/

   
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
    }

   

    public void onTeam(byte st = 1)
    {
       
        myTeam = st;

    }
    public byte getTeam()
    {
      return  myTeam ;
    }

    private string timeTxt;
    public void onTime(int st = 1)
    {
        TxtTimer.text = timeTxt + st; 
    }

    public void onPlayer(GameObject go)
    {
        player = go;
      //  Setting sett = go.GetComponent<Setting>();
     //   maxOd = sett.maxOD;
    //    od = sett.od;
    }
    public void onStep(byte st = 1)
    {
        if (step == st)
            return;

        step = st;
     //   string str;
        if(step ==1 )
        {
            TxtTimer.color = Color.blue;
        }else if(step == 2)
        {
            TxtTimer.color = Color.red;
        }
       if(step == myTeam)
       {
            if (player)
            {
                Setting sett = player.GetComponent<Setting>();
                player.GetComponent<Setting>().od = player.GetComponent<Setting>().maxOD;
                panelOD.instance.createOD(sett.maxOD, sett.od);
            }

            //   str = "ВАША комнада ходит ";
            timeTxt = "ВАШ ход: ";
        }
       else
       {
            //    str = "ходит комнада врага ";
            timeTxt = "Ход врага: ";
            checkBox();
       }
      //  TxtStep.text = str;
    }

    private void checkBox()
    {
        GameObject[] boxs = GameObject.FindGameObjectsWithTag("Box");
        if(boxs.Length>0)
        {
            for(int b = 0; b < boxs.Length; b++)
            {
                if(boxs[b].GetComponent<PhotonView>().isMine)
                {
                    //GameObject.DestroyObject(boxs[b]);
                    PhotonNetwork.Destroy(boxs[b]);
                }
            }
        }
    }

    public bool isStep()
    {
        if (step == myTeam)
            return true;

        return false;
    }
  
}
