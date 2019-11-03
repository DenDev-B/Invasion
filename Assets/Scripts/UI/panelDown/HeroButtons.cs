using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

using UnityEngine.UI;
//отключил
public class HeroButtons : MonoBehaviour
{
    // public GameObject[] Heroes;
    private GameObject PVPbttn;
    public GameObject buttonPrefab;
    private UnityEngine.Color hoverColor = UnityEngine.Color.red;
    int k;
    // Start is called before the first frame update
    void Start()
    {

        k = 0;
      // addHeroBttn();
    }
    public void clearBttn()
    {
        if (Boot.PVP && PVPbttn)
        {
            GameObject.DestroyObject(PVPbttn);
           // DestroyObject(PVPbttn);
        }

    }
    public void addHeroBttn(string name, Transform player)
     {
        var myButton = Instantiate(buttonPrefab, new Vector3(0f,0f,0f), Quaternion.identity);
        myButton.transform.SetParent(transform);
        myButton.GetComponentInChildren<Text>().text = name;
        myButton.name = name + "_Bttn";
        myButton.GetComponentInChildren<Text>().color = hoverColor;
        myButton.GetComponent<NameButtonUI>().tr = player;
        var x = -190f +k * 110f;
        myButton.GetComponent<NameButtonUI>().parent = transform;
        myButton.transform.position = transform.position + new Vector3(x, 55f, 0);
        if(Boot.PVP)
         PVPbttn = myButton;
          k++;
    }

}