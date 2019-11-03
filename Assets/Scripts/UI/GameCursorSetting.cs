using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameCursorSetting : MonoBehaviour
{
    public Texture2D cursorNormal;
    public Texture2D cursorWalk;
    public Texture2D cursorFair;
    public int size = 30; // размер курсора по ширине и высоте

    private Vector2 offset;
    public Texture2D cursor;
    public GameObject cursorTxt;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
        Cursor.visible = false; 
    }

    void OnGUI()
    {
        Vector2 mousePos = Event.current.mousePosition;
        GUI.depth = 999; // поверх остальных элементов
        GUI.Label(new Rect(mousePos.x + offset.x, mousePos.y + offset.y, size, size), cursor);


      //  Vector3 point = new Vector3();
      /*  Event currentEvent = Event.current;
        Vector2 mousePoss = new Vector2();
        mousePoss.x = currentEvent.mousePosition.x;
        mousePoss.y = cam.pixelHeight - currentEvent.mousePosition.y;
        cursorTxt.transform.position = new Vector2(mousePoss.x, mousePoss.y);
        //   point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
        */
        /* GUILayout.BeginArea(new Rect(20, 20, 250, 120));
         GUILayout.Label("Screen pixels: " + cam.pixelWidth + ":" + cam.pixelHeight);
         GUILayout.Label("Mouse position: " + mousePoss);
         GUILayout.Label("World position: " + point.ToString("F3"));
         GUILayout.EndArea();*/
    }
}
