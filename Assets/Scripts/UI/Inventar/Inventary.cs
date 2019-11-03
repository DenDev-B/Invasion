using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventary : MonoBehaviour
{

  //  List<ItemDrop> list;
    public GameObject inventar;
    public GameObject container;
    public GameObject cellView;
    public Transform heroPanel;
    public int cellCount;
    private Setting _set;


    private void Start()
    {
        for (int i = 0; i < cellCount; i++)
        {
            GameObject cell = Instantiate(cellView);
            cell.transform.SetParent(inventar.transform);
        }
    }
    public void  resetInvent()
    {
        addInvent(_set);
    }
    public void addInvent(Setting set)
    {
        Drop drop = heroPanel.GetComponent<Drop>();
        clear();
        _set = set;
        drop.life = set.GetComponentInParent<LifeComponent>();
    //    this.
       int countInventar = set.inventar.Count;
        for (int i = 0; i < countInventar; i++)
        {
            Item it = set.inventar[i].item;
            int count = set.inventar[i].count;
            if (inventar.transform.childCount >= i)
            {
                GameObject img = Instantiate(container);
                img.transform.SetParent(inventar.transform.GetChild(i).transform);
              //  img.transform.SetParent(container.transform.GetChild(i).transform);
                img.transform.localPosition = new Vector3(0f,0f,0f);
                img.GetComponent<Image>().sprite = Resources.Load<Sprite>(it.icon);
                img.GetComponent<Drag>().item = it;
                img.GetComponent<Drag>().sprite = it.sprite;
                img.GetComponent<Drag>().count = count;
                img.GetComponent<CellImg>().name.GetComponent<Text>().text= it.name;
                img.GetComponent<CellImg>().count.GetComponent<Text>().text="x"+ count;
            }
            else
                break;
        }
    }
    public void dellCell(Drag drag)
    {
        Destroy(drag.gameObject);
        _set.dellItemInventar(drag.item.name, 1);
    }
    public void removeCell(Drag drag)
    {
        
        GameObject nn = Instantiate<GameObject>(Resources.Load<GameObject>(drag.sprite));
        //   nn.transform.position = new Vector3(0f, 0f, 0f);
        Vector3 pos = new Vector3(_set.parent.GetComponent<Transform>().position.x, .3f, _set.parent.GetComponent<Transform>().position.z);
        nn.transform.position = pos;
        nn.name = drag.item.name;
        Destroy(drag.gameObject);
       
        _set.dellItemInventar(drag.item.name, 1);
    //    _set.inventar.Remove((drag.item,1));
    //   list.Remove(drag);
    }
    public void clear()
    {
        for(int i = 0; i <inventar.transform.childCount; i++)
        {
            if (inventar.transform.GetChild(i).transform.childCount > 0)
            {
                Destroy(inventar.transform.GetChild(i).transform.GetChild(0).gameObject);
            }
        }
    }
}
