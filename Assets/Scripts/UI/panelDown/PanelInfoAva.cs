using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelInfoAva : MonoBehaviour
{
    Dictionary<int, GameObject> avas = new Dictionary<int, GameObject>();
    public Transform avaPanel;
    int k;
    public void drawAddAva(Setting sett)
    {
     //   Debug.Log("ADD Ava " + sett);
        drawAva(sett.viewHero);
        
    }

    public void drawAva(string ava)
    {
     //   Debug.Log(ava);
       /* if (viewAva)
            Destroy(viewAva);*/
       GameObject viewAva = Instantiate(Resources.Load<GameObject>("Hero/Ava/" + ava));
        viewAva.transform.SetParent(avaPanel);
        Vector3 posAdd = new Vector3(-175f + k * 100f, 0f, 0f);
        viewAva.transform.position = avaPanel.position +posAdd;
        avas.Add(k, viewAva);
        k++;
       // Debug.Log("");
    }

    public void clearAva()
    {
        k = 0;
    }
}
