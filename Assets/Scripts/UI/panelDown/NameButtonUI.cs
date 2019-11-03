using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameButtonUI : MonoBehaviour
{
    public Button buttonComponent;
    public GameObject go;
    public Transform tr;
    public Transform parent;
    public string name;
//    bool batle;
    GameObject plauerInfo;

    // Start is called before the first frame update
    void Start()
    {
        
       
        GameObject[] tt = GameObject.FindGameObjectsWithTag("Player");

        plauerInfo = GameObject.Find("PlauerInfo");

        // var pos = plauerInfo.GetComponent<RectTransform>();


        //   if(plauerInfo)
        // plauerInfo.gameObject.SetActive = false;

        for (int i = 0; i < tt.Length; i++)        //подвязываем нужного персонажа.
            if (tt[i].transform == tr)
            {
                go = tt[i];
                break;
            }

        var hbc = go.GetComponent<HeroBttnComponent>();
        if(hbc)
         Object.Destroy(hbc as Object);
        if (tt.Length == 1)
        {
            buttonComponent.onClick.AddListener(HandleClick);
            activeBttn();
            Boot.cam.GetComponent<CameraControl>().onPosition(go.transform.position);
        }
          
        //Debug.Log("");
        /* if (go) {
             HeroBttnData hbb = go.GetComponent<HeroBttnData>();
             go.Destroy(hbb);
         }*/
    }


    public void HandleClick()
    {
                   //  batle = Boot.batle; //GameObject.Find("Boot").GetComponent<Boot>().batle;
            plauerInfo.GetComponent<PanelInfo>().showInventarPanel("down");
        if (Boot.batle)
            clear();
        var act = go.GetComponent<ActiveComponent>();
        if (!act)
        {
            activeBttn();
            Boot.cam.GetComponent<CameraControl>().onPosition(go.transform.position);
        }
        else
        {
            plauerInfo.transform.localPosition = new Vector3(4f, -135.3f, 0f);
            buttonComponent.GetComponentInChildren<Text>().color = Color.red;
            Object.Destroy(act as Object);
          //  plauerInfo.SetActive = false;
        }
    }

    private void clear()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i<players.Length;i++)
        {
            var pl = players[i].GetComponent<ActiveComponent>();
            if(pl)
                Object.Destroy(pl as Object);
        }
        foreach (Transform child in parent)
        {
            var btn = child.GetComponent<NameButtonUI>();
            if (btn)
                btn.GetComponentInChildren<Text>().color = Color.red;
            //Debug.Log(child.name);
        }
    }


    public void activeBttn()
    {
        buttonComponent.GetComponentInChildren<Text>().color = Color.green;
        go.AddComponent<ActiveComponent>();

       // p buttonComponent.onClick.AddListener(HandleClick);lauerInfo.transform.localPosition = new Vector3(4f, -5.3f, 0f);


    }
}
