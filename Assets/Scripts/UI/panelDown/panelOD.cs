using System;
using UnityEngine;

public class panelOD : MonoBehaviour
{
    public Transform parentPanel;
    public GameObject item;
    public GameObject itemFul;
    public static panelOD instance;
  //  private int lives;
    public GameObject[] items;
    //  public List list<>;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
       // createOD(10);
    }

    public void createOD(int max,int f)
    {
       clearOD();
       for (int i=0;i< max; i++)
       {
            GameObject view;
            if (i<f)
               view = Instantiate(item);
            else
               view = Instantiate(itemFul);
            view.name = string.Format(view.name+i);
            view.transform.SetParent(parentPanel);
            var x = -90f + i * 18; //-280f + i * 20f;
            view.transform.localPosition = new Vector3(x, 0f, 0f);
           // view.transform.position = new Vector3(x,+30f,0);
           //  view.transform.position = new Vector3(x,+30f,0);

            //  view.transform.position = new Vector3(x,0f,0f);
            //  view.transform.position = new Vector3(0f,0f,0f);
            //  items[i] = view;

        }
    }
    public void updateOD(int v,int f) //f-full v - realy
    {
        createOD(v,f);
    }
    public void clearOD()
    {
        if(parentPanel.childCount>0)
        {
            foreach (Transform child in parentPanel) Destroy(child.gameObject);
        }
    }

}
