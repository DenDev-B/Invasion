using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorSystem : ComponentSystem
{
    GameObject ground;
    GameCursorSetting gs;
    GameObject cursorTxt;
    RaycastHit hit;
    String str;


    struct Cursor
    {
        public ComponentDataArray<CursorData> data;
        public readonly int Length;
    }

    struct Player
    {
        public ComponentArray<Setting> sett;
        public ComponentArray<Transform> tr;
        public ComponentDataArray<BatleData> data;
        public ComponentDataArray<ActiveData> activ;
        public readonly int Length;
    }



    protected override void OnStartRunning()
    {
        gs = GameObject.Find("Cursor").GetComponent<GameCursorSetting>();
        cursorTxt = GameObject.Find("CursorTxt");
        GameObject[] grounds = GameObject.FindGameObjectsWithTag("Map");
        if (grounds.Length > 0)
            ground = grounds[0];
    }


    [Inject] Player player;
    [Inject] Cursor cursor;
    protected override void OnUpdate()
    {
        Vector3 mp = Input.mousePosition;
        //  RaycastHit hit;
        if (Boot.PVP)
        {
            if (PVPBattle.instance)
            {
                if (!PVPBattle.instance.isStep())
                {
                    cursorTxt.GetComponent<Text>().text = "";
                    MainCursor("UI");
                    return;
                }
             }
        }
        Ray ray = Camera.main.ScreenPointToRay(mp);
        cursorTxt.transform.position = new Vector2(mp.x + 55f, mp.y - 40f);
        cursorTxt.GetComponent<Text>().text = "";
        

        for (int j = 0; j < cursor.Length; j++)
        {
            if (Physics.Raycast(ray, out hit))
            {
              
                Transform objectHit = hit.transform;
            
                if (EventSystem.current.IsPointerOverGameObject())
                {
                  gs.cursor = gs.cursorNormal;
                    return;

                }
                if(player.Length == 1)
                {
                    var tr = player.tr[j];
                    var set = player.sett[j];
                    var data = player.data[j];
                   // Debug.Log(objectHit.tag);
                    MainCursor(objectHit.tag);
                    if (objectHit.tag == "Tower" || objectHit.tag == "Player_Friend")
                    {
                        LifeComponent life = objectHit.gameObject.GetComponent<LifeComponent>();
                        cursorTxt.GetComponent<Text>().text = "life:" + life.life;
                    }
                        if (objectHit.tag == "Map")
                    {
                        if (Boot.batle || Boot.PVP)
                        {
                            if (ground.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
                            {
                                cursorTxt.GetComponent<Text>().color = Color.black;
                                int dist = (int)Vector3.Distance(hit.point, tr.position) + 1;
                                str = dist.ToString();
                                 if (dist > set.od)                       // сначала систему батле и начисление ОД
                                    {
                                        cursorTxt.GetComponent<Text>().color = Color.red;
                                        str = "X";
                                    }
                                cursorTxt.GetComponent<Text>().text = str;
                            }
                        }
                    }
                   if ( objectHit.tag=="Player_Enemy" && tr.transform != objectHit.transform)
                    {
                        SettingsWeapon _ws;
                        if (set.weaponActive == 0 && set.viewWeaponLeft)
                        {
                            _ws = set.viewWeaponLeft.GetComponent<SettingsWeapon>();
                        }
                        else if (set.weaponActive == 1 && set.viewWeaponRight)
                        {
                            _ws = set.viewWeaponRight.GetComponent<SettingsWeapon>();
                        }
                        else
                        {
                            _ws = new SettingsWeapon();
                            _ws.shop = 0;
                            _ws.od = 0;
                            _ws.shopFull = 0;
                            _ws.cart_step = 0;
                        }
                        if (ground.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
                        {
                            cursorTxt.GetComponent<Text>().color = Color.black;
                            int dist = (int)Vector3.Distance(objectHit.position, tr.position);
                            int shop = _ws.shop;
                            if (set.weaponActive == 1)
                                shop = _ws.shop;
                          //  LifeComponent life = objectHit.gameObject.GetComponent<LifeComponent>();
                          //  LifeComponent life = objectHit.GetComponent<LifeComponent>();
                            if (set.od >= _ws.od && shop <= _ws.shopFull && shop >= _ws.cart_step)
                            {
                                if (_ws.radius < dist)
                                {
                                    cursorTxt.GetComponent<Text>().color = Color.red;
                                    str = "X";
                                }
                                int pr = set.procentKillPlayer(objectHit.gameObject);
                                cursorTxt.GetComponent<Text>().text = pr + "%";
                            //    cursorTxt.GetComponent<Text>().text = pr + "% \n life:" + life.life;
                            }
                            else
                            {
                                cursorTxt.GetComponent<Text>().color = Color.red;
                                str = "R";
                                cursorTxt.GetComponent<Text>().text = str = "R";
                               // cursorTxt.GetComponent<Text>().text = str + "\n life:" + life.life;
                            }
                          
                                
                               
                            

                        }
                    }else
                
                    if (objectHit.tag == "Enemy"  )
                    {
                       
                        SettingsWeapon _ws;
                        if (set.weaponActive == 0 && set.viewWeaponLeft)
                        {
                            _ws = set.viewWeaponLeft.GetComponent<SettingsWeapon>();
                        }
                        else if (set.weaponActive == 1 && set.viewWeaponRight)
                        {
                            _ws = set.viewWeaponRight.GetComponent<SettingsWeapon>();
                        }
                        else
                        {
                            _ws = new SettingsWeapon();
                            _ws.shop = 0;
                            _ws.od = 0;
                            _ws.shopFull = 0;
                            _ws.cart_step = 0;
                        }
                        if (ground.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
                        {
                            cursorTxt.GetComponent<Text>().color = Color.black;
                            int dist = (int)Vector3.Distance(objectHit.position, tr.position);
                            int shop = _ws.shop;
                            if (set.weaponActive == 1)
                                shop = _ws.shop;
                            if (set.od >= _ws.od && shop <= _ws.shopFull && shop >= _ws.cart_step)
                            { 
                                if (_ws.radius < dist)
                                {
                                    cursorTxt.GetComponent<Text>().color = Color.red;
                                    str = "X";
                                }
                                int pr=  set.procentKillEnemy(objectHit.gameObject);
                            
                                cursorTxt.GetComponent<Text>().text ="pr: "+ pr+"%";
                                if (Boot.PVP)
                                {
                                    LifeComponent life = objectHit.gameObject.GetComponent<LifeComponent>();
                                    cursorTxt.GetComponent<Text>().text = "pr: "+ pr + "% \n life:"+ life.life;
                                }
                            }
                            else
                            {
                                cursorTxt.GetComponent<Text>().color = Color.red;
                                str = "R";
                                cursorTxt.GetComponent<Text>().text = str = "R";
                            }

                        }
                    }
                }
                else
                    MainCursor("UI");
            }
         
        }

      

    }

    private string ToString(int z)
    {
        throw new NotImplementedException();
    }

    void MainCursor(string tags)
    {
      
        if (tags == "UI")
        {
            gs.cursor = gs.cursorNormal;
           
        }
        else
        if (tags == "Map")
        {
            gs.cursor = gs.cursorWalk;
 
        }
        else
        if (tags == "Player")
        {
           gs.cursor = gs.cursorNormal;

        }
        else
        if (tags == "Enemy" || tags == "Player_Enemy")
        {
            gs.cursor = gs.cursorFair;
        }
        else // курсор по умолчанию
        {
            gs.cursor = gs.cursorNormal;
        }


    }
}
