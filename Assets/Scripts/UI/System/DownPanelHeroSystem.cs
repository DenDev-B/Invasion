using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class DownPanelHeroSystem : ComponentSystem
{
    //GameObject boot;

    struct BttnPlayer
    {
        public ComponentArray<Setting> sett;
        public ComponentDataArray<HeroBttnData> data;
        public readonly int Length;
    }

    struct InfoPlayer
    {
        public ComponentArray<Setting> sett;
        public ComponentDataArray<ActiveData> data;
        public readonly int Length;
    }

  

    [Inject] BttnPlayer bttnPlayer;
    [Inject] InfoPlayer infoPlayer;
    protected override void OnUpdate()
    {
        for (int i = 0; i < bttnPlayer.Length; i++)     //Добавление кнопок
        {
          var sett = bttnPlayer.sett[i];
          var data = bttnPlayer.data[i];
          if (data.button == 0)
           {
                createBttn(sett);
                data.button = 1;
                bttnPlayer.data[i] = data;
          }
        }
        for (int j = 0; j < infoPlayer.Length; j++)
        {
            var settinfo = infoPlayer.sett[j];
            var show = infoPlayer.data[j];
            if (infoPlayer.Length == 1 && show.infoShow==0)
            {
                //  Debug.Log("Info"); 
              
                show.infoShow = 1;
                infoPlayer.data[j] = show;
              //  settinfo.od = settinfo.maxOD;
                drawInfoUnit(settinfo);
            }
            if(infoPlayer.Length != 1 && show.infoShow ==1)
            {
                GameObject   pi = GameObject.Find("PlauerInfo");
                  pi.transform.localPosition = new Vector3(4f, -135f, 0f);
            }   
            if(infoPlayer.Length>1)
            {
                if(show.infoShow==1)
                {
                    show.infoShow = 0;
                    infoPlayer.data[j] = show;
                }
              //  Debug.Log("Full");
            }

            //all 
            if (infoPlayer.Length > 1)
            {
                 GameObject pia = GameObject.Find("PlauerInfoAva");
                 pia.transform.localPosition = new Vector3(4f, -5.3f, 0f);
            }else
            {
                GameObject pia = GameObject.Find("PlauerInfoAva");
                pia.transform.localPosition = new Vector3(4f, -135f, 0f);
            }
        }
    }

    private void drawInfoAllUnit(Setting settinfo)
    {

        
      //  pia.GetComponent<PanelInfoAva>().drawAddAva(settinfo);
    }

    private void drawInfoUnit(Setting settinfo)
    {
      //  Debug.Log("PlauerInfoAva " + settinfo.view);

        GameObject pi = GameObject.Find("PlauerInfo");
        pi.transform.localPosition = new Vector3(4f, -5.3f, 0f);

        pi.GetComponent<PanelInfo>().drawSett(settinfo);
       // pi.GetComponent<PanelInfo>().drawAva(settinfo.viewHero);
       // pi.GetComponent<PanelInfo>().drawWeapon(settinfo.viewHero);
       
    }

    private void createBttn(Setting sett)
    {
        GameObject.FindGameObjectWithTag("App").GetComponent<Boot>().DownPanel.GetComponent<HeroButtons>().addHeroBttn(sett.viewHero, sett.parent);
        GameObject pia = GameObject.Find("PlauerInfoAva");
        pia.GetComponent<PanelInfoAva>().drawAddAva(sett);
    }

}
