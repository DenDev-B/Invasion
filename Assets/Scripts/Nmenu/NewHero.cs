using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewHero : MonoBehaviour
{
    [System.Serializable]
    public class weaponArr
    {
        public string name;
        public int rad;
        public int od;
        public int dmgMin;
        public int dmgMax;
        public int mag;
    }
    public List<weaponArr> myWeap;
    [System.Serializable]
    public class heroArr
    {
        public string ava;
        public int life;
        public int od;
    }
    public List<heroArr> myList;
    //public List<string> myList = new List<string>();
    public int num = 0;
    public int numL = 0;
    public int numR = 0;
    private GameObject viewAva;
    private GameObject viewLW;
    private GameObject viewRW;
    public InputField PlayerInputName;
    public Transform PlayerAva;
    public Transform PlayerLF;
    public Transform PlayerRF;
    private string PlayerName = "Player_";
    public Text lifeID;
    public Text odID;

    public Text nameLw;
    public Text radLW;
    public Text odLW;
    public Text dmLW;
    public Text magLW;

    public Text nameRw;
    public Text radRW;
    public Text odRW;
    public Text dmRW;
    public Text magRW;
    // Start is called before the first frame update
    void Start()
    {
        var k = System.Math.Round(UnityEngine.Random.Range(1f, 60f));
        PlayerName += k;
      //   PlayerInputName.GetComponent<Text>().text = "PlayerName";
        PlayerInputName.text = PlayerName;
        /*   myList.Add("MotusManPVP");
           myList.Add("BodyguardPVP");
           myList.Add("SaraBeckerPVP");*/
        loadAva();
        loadLeftW();
        loadRightW();
    }

    private void loadLeftW()
    {
        if (viewLW)
            GameObject.Destroy(viewLW);
        viewLW = Instantiate(Resources.Load<GameObject>("Hero/UiWeapons/" + myWeap[numL].name));
        viewLW.transform.SetParent(PlayerLF);
          nameLw.text = "" + myWeap[numL].name;
          radLW.text = "" + myWeap[numL].rad;
          odLW.text = "" + myWeap[numL].od;
        dmLW.text = "" + myWeap[numL].dmgMin+"-"+ myWeap[numL].dmgMax;
        magLW.text = "" + myWeap[numL].mag;
       
        viewLW.transform.position = PlayerLF.position;// + posAdd;
    }

    public void NextLW()
    {
        numL++;
        if (numL == myWeap.Count)
            numL = 0;
        loadLeftW();
    }

    public void previousLW()
    {
        numL--;
        if (numL == -1)
            numL = myWeap.Count - 1;
        loadLeftW();
    }

    private void loadRightW()
    {
        if (viewRW)
            GameObject.Destroy(viewRW);
        viewRW = Instantiate(Resources.Load<GameObject>("Hero/UiWeapons/" + myWeap[numR].name));
        viewRW.transform.SetParent(PlayerRF);
        nameRw.text = "" + myWeap[numR].name;
        radRW.text = "" + myWeap[numR].rad;
        odRW.text = "" + myWeap[numR].od;
        dmRW.text = "" + myWeap[numR].dmgMin + "-" + myWeap[numR].dmgMax;
        magRW.text = "" + myWeap[numR].mag;
        viewRW.transform.position = Vector3.zero;
        viewRW.transform.position = PlayerRF.position;// + posAdd;
    }

    public void NextRW()
    {
        numR++;
        if (numR == myWeap.Count)
            numR = 0;
        loadRightW();
    }

    public void previousRW()
    {
        numR--;
        if (numR == -1)
            numR = myWeap.Count - 1;
        loadRightW();
    }

    private void loadAva()
    {
        if (viewAva)
            GameObject.Destroy(viewAva);
        viewAva = Instantiate(Resources.Load<GameObject>("Hero/Ava/" + myList[num].ava));
        viewAva.transform.SetParent(PlayerAva);
        lifeID.text = "" + myList[num].life;
        odID.text = "" + myList[num].od;
        //  Vector3 posAdd = new Vector3(-175f + k * 100f, 0f, 0f);
        viewAva.transform.position = PlayerAva.position;// + posAdd;
      //  avas.Add(k, viewAva);
    }

    public void Next()
    {
        num++;
        if (num == myList.Count)
            num = 0;
        loadAva();
    }

    public void previous()
    {
        num--;
        if (num == -1)
            num = myList.Count-1;
        loadAva();
    }

    // Update is called once per frame
    void Update()
    {
       // Instantiate(Resources.Load<GameObject>("Weapon/" + nameWeapon));
    }
}
