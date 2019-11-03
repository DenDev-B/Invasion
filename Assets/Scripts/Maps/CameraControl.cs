using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Vector3 dXYZ;
    public Vector3 rotXYZ;
    public float speed;
    private float dt;
    bool gotoCheck;
   Vector3 start;
    Vector3 pos;
    Vector3 rot;
    


  //  public Sprite cursorNormal, cursorMove;
 //   public RectTransform _cursor;
    public float sensitivity = 30;
   // public float speed = 3;

  //  private Image img;
    private Vector3 /*offset,*/ direction;
    private Vector3  mpClick;
   // private Sprite current;



    private void Start()
    {
        gotoCheck = false;
        dt = Time.deltaTime;
         start = transform.position;
        pos = transform.position;
     //   rot = transform.rotation;
    }

    public void onPosition(Vector3 _pos)
    {

        pos = _pos;
        gotoCheck = true;
    }

    void Update()
    {
          if (pos != start && gotoCheck)
          {
             transform.position = new Vector3(Mathf.Lerp(transform.position.x, pos.x+ dXYZ.x, speed * dt), transform.position.y+dXYZ.y, Mathf.Lerp(transform.position.z, pos.z+dXYZ.z, speed * dt));
             start = transform.position;
          }

        if (Input.GetMouseButtonDown(2))
        {
            mpClick = Input.mousePosition;
        }

        if (Input.GetMouseButton(2)) {
                   
            if (CheckMouse())
            {
                gotoCheck = false;
                direction = Camera.main.transform.TransformDirection(direction);

                transform.Translate(direction * speed * 2 * dt);
                start = transform.position;
            }
        }
        /*direction = Camera.main.transform.TransformDirection(direction);
      
        transform.Translate(direction * speed*2 * dt);*/


    }
    /*  void LateUpdate()
       {
           direction = Vector3.zero;

           if (CheckMouse())
           {
              // current = cursorMove;
           }
           else
           {
              // offset = new Vector3(_cursor.sizeDelta.x / 2, -_cursor.sizeDelta.y / 2, 0);
              // current = cursorNormal;
           }

         //  _cursor.anchoredPosition = Input.mousePosition + offset;
         //  float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //   _cursor.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
           direction = Camera.main.transform.TransformDirection(direction);
         //  direction.y = 0;
            transform.Translate(direction * speed * dt);
        //   img.sprite = current;
       }*/
    Vector3 mpPosT;
    bool CheckMouse()
    {
        bool left = false, right = false, down = false, up = false;
        if (mpPosT != Input.mousePosition)
            mpPosT = Input.mousePosition;
        else
            return false;
     //   if (Input.mousePosition.x < sensitivity)
        if (Input.mousePosition.x > mpClick.x)
        {
        /*    offset = new Vector3(_cursor.sizeDelta.x / 2, 0, 0);*/
            direction = Vector3.right;
            right = true;
           // Debug.Log("left");
        }

      //  if (Input.mousePosition.x > Screen.width - sensitivity)
        if (Input.mousePosition.x < mpClick.x)
        {
       /*    offset = new Vector3(-_cursor.sizeDelta.x / 2, 0, 0);*/
            direction = Vector3.left;
            left = true;
          //  Debug.Log("right");
        }

      //  if (Input.mousePosition.y < sensitivity)
        if (Input.mousePosition.y > mpClick.y)
        {
       
            direction = Vector3.forward;
            down = true;
           // Debug.Log("down");
        }

      //  if (Input.mousePosition.y > Screen.height - sensitivity)
        if (Input.mousePosition.y < mpClick.y)
        {
         
            direction = Vector3.back;
            up = true;
        //    Debug.Log("up");
        }
        
        if (left && up)
        {
         
            direction = new Vector3(-1, 0, -1);
            return true;
        }
        else if (left && down)
        {  
            direction = new Vector3(-1, 0, 1);
            return true;
        }
        else if (right && down)
        {
        
            direction = new Vector3(1, 0, 1);
            return true;
        }
        else if (right && up)
        {
        
            direction = new Vector3(1, 0, -1);
            return true;
        }

        if (left || up || right || down) return true;
        
        return false;
    }
}
