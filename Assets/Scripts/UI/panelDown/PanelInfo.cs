using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelInfo : MonoBehaviour
{
    public Transform avaPanel;
    public Transform wLwftPanel;
    public Transform wRightPanel;
    public Transform odPanel;
    public GameObject life;
    public GameObject lifeCount;
    public GameObject viewAva;
    public GameObject viewWeaponL;
    public GameObject viewWeaponR;
    public GameObject bttnResetL;
    public GameObject bttnResetR;
    Setting _sett;
   

    public void drawSett(Setting sett)
    {
        _sett = sett;
        drawAva(sett.viewHero);
        drawLife(sett);
        drawWeaponL(sett.nameWeaponLeft);
        drawWeaponR(sett.nameWeaponRight);
    }

    public void resetLife()
    {
        LifeComponent Life = _sett.GetComponentInParent<LifeComponent>();
        lifeCount.GetComponent<Text>().text = "" + Life.life;
        life.GetComponent<Slider>().value = (Life.life / Life.full_life);
        _sett.life = life;
    }
    private void drawLife(Setting sett)
    {
        LifeComponent Life =  sett.GetComponentInParent<LifeComponent>();

        // var count = life.transform.GetChild(0);
        lifeCount.GetComponent<Text>().text = "" +Life.life;
        life.GetComponent<Slider>().value = (Life.life / Life.full_life);
        sett.life = life;
    }

    public void  resetWL() {

     //   Debug.Log("Reset left");
        // var weapo = viewWeaponL.GetComponent<SettingsWeapon>();
        SettingsWeapon wSetL = _sett.viewWeaponLeft.GetComponent<SettingsWeapon>();
       if (wSetL.shop < wSetL.shopFull)
        {
            int full = _sett.coountOnInventar(wSetL.cartridge);
            if (full >0)
            {
                int need = wSetL.shopFull - wSetL.shop;
               // if(full >= need)
                //{
                    int add = _sett.dellItemInventar(wSetL.cartridge, need);
                    wSetL.shop += add;
                    drawWeaponL(_sett.nameWeaponLeft);
               // }
            }
        }
    }

    public void resetWR()
    {
      //  Debug.Log("Reset left");
        // var weapo = viewWeaponL.GetComponent<SettingsWeapon>();
        SettingsWeapon wSetR = _sett.viewWeaponRight.GetComponent<SettingsWeapon>();
        if (wSetR.shop < wSetR.shopFull)
        {
            int full = _sett.coountOnInventar(wSetR.cartridge);
            if (full > 0)
            {
                int need = wSetR.shopFull - wSetR.shop;
                // if(full >= need)
                //{
                int add = _sett.dellItemInventar(wSetR.cartridge, need);
                wSetR.shop += add;
                drawWeaponR(_sett.nameWeaponRight);
                // }
            }
        }
    }

    private void drawWeaponL(string viewWeaponLeft)
    {
    //     Debug.Log("viewWeaponLeft");
        if (viewWeaponL)
            Destroy(viewWeaponL);
        String weaponStr = viewWeaponLeft;
        if (weaponStr == "")
            weaponStr = "Empty";
        var vPanel = wLwftPanel.GetChild(0);
        SettingsWeapon wSetL = null;
        if (weaponStr != "Empty")
        {
            wSetL = _sett.viewWeaponLeft.GetComponent<SettingsWeapon>();

            viewWeaponL = Instantiate(wSetL.Icon);
        }
        else
              viewWeaponL = Instantiate(Resources.Load<GameObject>("Hero/UiWeapons/" + weaponStr));
  
       viewWeaponL.transform.SetParent(vPanel);
       viewWeaponL.transform.position = wLwftPanel.position;

        if (weaponStr != "Empty" && wSetL)
        {
            wLwftPanel.transform.Find("name").GetComponent<Text>().text = viewWeaponLeft;
            wLwftPanel.transform.Find("od").GetComponent<Text>().text = "од " + wSetL.od;
            wLwftPanel.transform.Find("shop").GetComponent<Text>().text = "" + wSetL.shop + "/" + wSetL.shopFull;
            if (wSetL.shop < wSetL.shopFull)
                bttnResetL.SetActive(true);
            else
                bttnResetL.SetActive(false);
        }
        else
        {
            wLwftPanel.transform.Find("name").GetComponent<Text>().text = "нет анимации";
            wLwftPanel.transform.Find("od").GetComponent<Text>().text = "";
            wLwftPanel.transform.Find("shop").GetComponent<Text>().text = "";
            bttnResetL.SetActive (false);
        }
        if (_sett.weaponActive == 0)
        {
            wLwftPanel.GetComponent<Image>().color = Color.green;
        }
        else
            wLwftPanel.GetComponent<Image>().color = Color.white;
                
    }



    private void drawWeaponR(string viewWeaponRight)
    {
   
        //Debug.Log(viewWeaponRight);
        if (viewWeaponR)
            Destroy(viewWeaponR);
        
        String weaponStr = viewWeaponRight;
        if (weaponStr == "")
            weaponStr = "Empty";
       var vPanel = wRightPanel.GetChild(0);
        SettingsWeapon wSet = null ;
        if (weaponStr != "Empty")
        {
            /* GameObject weaponGo = Resources.Load<GameObject>("Weapon/" + weaponStr);
             wSet = weaponGo.GetComponent<SettingsWeapon>();
             viewWeaponR = Instantiate(wSet.Icon);*/
            wSet = _sett.viewWeaponRight.GetComponent<SettingsWeapon>();

            viewWeaponR = Instantiate(wSet.Icon);
        }
        else
          viewWeaponR = Instantiate(Resources.Load<GameObject>("Hero/UiWeapons/" + weaponStr));
     

        viewWeaponR.transform.SetParent(vPanel);
        viewWeaponR.transform.position = wRightPanel.position;
        if (weaponStr != "Empti" && wSet)
        {
            wRightPanel.transform.Find("name").GetComponent<Text>().text = viewWeaponRight;
            wRightPanel.transform.Find("od").GetComponent<Text>().text = "од " + wSet.od;
            wRightPanel.transform.Find("shop").GetComponent<Text>().text = "" + wSet.shop + "/" + wSet.shopFull;
            if (wSet.shop < wSet.shopFull)
            {
                bttnResetR.SetActive(true);
            }else
            bttnResetR.SetActive(false);
        }
        else
        {
            wRightPanel.transform.Find("name").GetComponent<Text>().text = "нет анимации";
            wRightPanel.transform.Find("od").GetComponent<Text>().text = "";
            wRightPanel.transform.Find("shop").GetComponent<Text>().text = "";
            bttnResetR.SetActive(false);
        }
       
         if (_sett.weaponActive == 1)
        {
            wRightPanel.GetComponent<Image>().color = Color.green;
        }
        else
            wRightPanel.GetComponent<Image>().color = Color.white;
    }

    public void activLeft()
    {
        if (_sett.nameWeaponLeft == "")
            return;

        _sett.weaponActive = 0;
        drawWeaponL(_sett.nameWeaponLeft);
        drawWeaponR(_sett.nameWeaponRight);
        _sett.LoadWeapon();
    }

    public void activRight()
    {
        if (_sett.nameWeaponRight == "")
            return;
        _sett.weaponActive = 1;
        drawWeaponL(_sett.nameWeaponLeft);
        drawWeaponR(_sett.nameWeaponRight);
        _sett.LoadWeapon();
    }

    public void drawAva(string ava)
    {
       // Debug.Log(ava);
        if (viewAva)
            Destroy(viewAva);
        viewAva = Instantiate(Resources.Load<GameObject>("Hero/Ava/" + ava));
        viewAva.transform.SetParent(avaPanel);
        viewAva.transform.position = avaPanel.position;
    }
   
    public void onHold()
    {
        if (Boot.PVP && PVPBattle.instance)
            if (!PVPBattle.instance.isStep())
                return;
        if (_sett.view.GetComponent<Animator>().GetBool("walk"))
            return;
        if (_sett.view.GetComponent<Animator>().GetBool("fire"))
            return;
        if (_sett.view.GetComponent<Animator>().GetBool("dead"))
            return;
        int pos = _sett.view.GetComponent<Animator>().GetInteger ("pose");
        if (pos != 0)
        {
            if (getOD(2))
             //   if (_sett.od >= 2)
            {
          //      _sett.od -= 2;
                _sett.view.GetComponent<Animator>().SetInteger("pose", 0);
            }
            else
                return;
        }
      //  _sett.view.GetComponent<Animator>().SetInteger("pose",0);
    }
    public void onSite()
    {
        if (Boot.PVP && PVPBattle.instance)
            if (!PVPBattle.instance.isStep())
                return;
        if (_sett.view.GetComponent<Animator>().GetBool("walk"))
            return;
        if (_sett.view.GetComponent<Animator>().GetBool("fire"))
            return;
        if (_sett.view.GetComponent<Animator>().GetBool("dead"))
            return;
        int pos = _sett.view.GetComponent<Animator>().GetInteger("pose");
        if (pos != 1)
        {
            if(getOD(2))
          //  if (_sett.od >= 2)
            {
            //    _sett.od -= 2;
                _sett.view.GetComponent<Animator>().SetInteger("pose", 1);
            }
            else
                return;
        }
        // _sett.view.GetComponent<Animator>().SetInteger("pose", 1);
    }

    private bool getOD(int v)
    {
        if (_sett.od >= v)
        {
            _sett.od -= v;
            panelOD.instance.createOD(_sett.maxOD, _sett.od);
            return true;
        }
        return false;
    }

    public void onLay()
    {
        if (Boot.PVP && PVPBattle.instance)
            if (!PVPBattle.instance.isStep())
                return;
        if (_sett.view.GetComponent<Animator>().GetBool("walk"))
            return;
        if (_sett.view.GetComponent<Animator>().GetBool("fire"))
            return;
        if (_sett.view.GetComponent<Animator>().GetBool("dead"))
            return;
        int pos = _sett.view.GetComponent<Animator>().GetInteger("pose");
        if (pos != 2)
        {
            if (getOD(2))
              //  if (_sett.od >= 2)
            {
            //    _sett.od -= 2;
                _sett.view.GetComponent<Animator>().SetInteger("pose", 2);
            }
            else
                return;
        }
        // _sett.view.GetComponent<Animator>().SetInteger("pose", 2);
    }

    public void showInventarPanel(string str="")
    {

        GameObject inventar = GameObject.Find("InventarPanel");
        if(str == "down")
        {
            if (inventar.transform.localPosition.y > 0)
            {
                inventar.transform.localPosition = new Vector3(80f, -600f, 0f);
            }
            return;
        }

        if (inventar.transform.localPosition.y < 0)
        {
            inventar.transform.localPosition = new Vector3(80f, 90f, 0f);
            var iii = inventar.GetComponent<Inventary>();
                iii.addInvent(_sett);
        }
        else
        {
            inventar.transform.localPosition = new Vector3(80f, -600f, 0f);
            inventar.GetComponent<Inventary>().clear();
        }
    }
}
