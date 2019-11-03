using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler 
{
    public  LifeComponent life;
    public Item item;
    public int type=0;

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("Drop "+ eventData.pointerDrag.GetComponent);
         Drag drag = eventData.pointerDrag.GetComponent<Drag>();
       // Debug.Log("Drop " + drag.item.type);
        if (drag != null)
        {
            if (type==1 &&  drag.item.type == 1)
            {
                if (life && life.life < life.full_life)
                {
                    life.life += 10;
                  
                    //   if (life.life > life.full_life)
                    //    life.life = life.full_life;
                     drag.transform.SetParent(this.transform);
                    life.plauerUpdatePanel();
                    //  drag.transform.SetParent(drag.old);
                }
                else
                    drag.transform.SetParent(drag.old);
            }
            else
               drag.transform.SetParent(transform);
            //  else

        }
    }


}
