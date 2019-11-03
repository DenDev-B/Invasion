using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler/*, IPointerClickHandler*/
{
    public Transform canvas;
    public Transform canvasHero;
    public Transform old;
    public Item item;
    public int count;
    public string sprite;

    void Start()
    {
        canvas = GameObject.Find("InventarItemPanel").transform;
        canvasHero = GameObject.Find("HeroPanel").transform;  
    }


    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        Debug.Log("transform.parent "+ transform.parent);
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (transform.parent == canvasHero)
        {
            //      Debug.Log("canvasHero");
            canvasHero.GetComponentInParent<Inventary>().dellCell(this);
            canvasHero.GetComponentInParent<Inventary>().resetInvent();
        }
        else
        if (transform.parent == canvas)
        {
            canvas.GetComponentInParent<Inventary>().removeCell(this);
            canvas.GetComponentInParent<Inventary>().resetInvent();
        }
            //  if (transform.parent == canvas)
            //     canvas.GetComponentInParent<Inventary>().removeCell(this);
            //   Debug.Log("Canvas");

            //   transform.SetParent(old);

            GetComponent<CanvasGroup>().blocksRaycasts = true;
        

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        old = transform.parent;
        transform.SetParent(canvas);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

   /* void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        
    }*/
}
