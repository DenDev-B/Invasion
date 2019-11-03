using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeComponent : MonoBehaviour
{
    public float life;
    public float full_life=40;
    public Vector3 offset;
    public GameObject HP_Slider;
    public GameObject go;
    public GameObject lifeView;
    public GameObject canvas;

    private void Start()
    {
        canvas = GameObject.Find("Canvas");
        life = full_life;
        if (HP_Slider && go.tag=="Enemy")
        {
            lifeView = Instantiate(HP_Slider);
            lifeView.transform.SetParent(canvas.transform);
        }
    }
    public void plauerUpdatePanel()
    {
        if (HP_Slider)
        {
            HP_Slider.GetComponent<Slider>().value = (life / full_life);
            lifeView.GetComponent<Text>().text = "" + life;
        }
    }
    public void onKick(float _kick)
    {
        life -= _kick;
   
        if (lifeView)
        {
           // lifeView.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(go.transform.position + offset);
            // GetComponent<RectTransform>.position = Camera.main.WorldToScreenPoint(go.transform.position)
            lifeView.GetComponent<Slider>().value = (life / full_life);
        }
        if (life <= 0)
        {
            SettingsEnimy ss = this.gameObject.GetComponent<SettingsEnimy>();
            if (ss)
            {
                ss.view.GetComponent<Animator>().SetBool("diy", true);
                ss.tag = "Box";
             //   ss.GetComponent<ZombiComponent>().enabled = false;
              //  ss.GetComponent<LifeComponent>().enabled = false;

            }

        }
    }
    public void onPVPPlayerDmg(int dmg, Action callback)
    {
        life -= dmg;
        Debug.Log("You life "+life);
        if(life<=0)
        {
            life = 0;
          
            callback();
        }else
          Boot.PlauerInfo.GetComponent<PanelInfo>().resetLife();

    }


    public void onKillPlayer()
    {
        if (life <= 0)
        {
            Setting ss = this.gameObject.GetComponent<Setting>();
            if (ss)
            {
                ss.GetComponent<HumanoidComponent>().enabled = false;
                ss.GetComponent<LifeComponent>().enabled = false;
                
            }

        }
    } 

    public void onKickPlayer(float _kick)
    {
        life -= _kick;
        plauerUpdatePanel();
        if (HP_Slider)
        {
            HP_Slider.GetComponent<Slider>().value = (life / full_life);
            lifeView.GetComponent<Text>().text = ""+life;

        }

        if (life <= 0)
        {
            Setting ss = this.gameObject.GetComponent<Setting>();
            if (ss)
            {
                ss.view.GetComponent<Animator>().SetBool("dead", true);
                ss.tag = "Box";
            //    ss.GetComponent<HumanoidComponent>().enabled = false;
          //      ss.GetComponent<LifeComponent>().enabled = false;

            }

        }
    }

    private void Update()
    {
        if (lifeView && go.tag == "Enemy")
        {
            lifeView.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(go.transform.position + offset);
            // GetComponent<RectTransform>.position = Camera.main.WorldToScreenPoint(go.transform.position)
            //lifeView.GetComponent<Slider>().value = (life / full_life);
        }
    }

}
