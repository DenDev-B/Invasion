using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class InputSystem : ComponentSystem
{
    GameObject ground;
    Game game;
    RaycastHit hit;
    Ray ray;
    Transform objectHit;
    bool isFire;

    struct Player
    {
        public ComponentArray<Setting> sett;
        public ComponentArray<Transform> tr;
        public ComponentDataArray<HumanoidData> tag;
        public ComponentDataArray<ActiveData> active;
      //  public ComponentArray<NavMeshAgent> agents;
        public readonly int Length;
    }
    protected override void OnStartRunning()
    {
        if(Boot.PVP)
          game = GameObject.Find("Boot").GetComponent<Game>();
        GameObject[] grounds = GameObject.FindGameObjectsWithTag("Map");
        if(grounds.Length>0)
         ground = grounds[0];
    }

    [Inject] Player player;
    protected override void OnUpdate()
    {
        Vector3 mp = Input.mousePosition;
        ray = Camera.main.ScreenPointToRay(mp);

         if (Input.GetMouseButtonDown(0) /*&& !isFire*/)
        {
            isFire = false;
            ray = Camera.main.ScreenPointToRay(mp);

           if (EventSystem.current.IsPointerOverGameObject())
                   return;
     

            for (int i = 0; i < player.Length; i++)
                {
                var set = player.sett[i];
                   var tr = player.tr[i];
                if (Boot.batle && !Boot.STEP && !Boot.PVP)
                {
                    Debug.Log("Input continue");
                    continue;
                }
                if (Boot.PVP)
                {
                    if (PVPBattle.instance)
                    {
                        if (!PVPBattle.instance.isStep())
                        {
                            Debug.Log("Input isStep continue");
                            return;
                        }
                    }
                }
                if (Physics.Raycast(ray, out hit))
                    {
                        objectHit = hit.transform;
                       Debug.Log("Input objectHit "+ objectHit.tag+"  "+isFire);
                      if (objectHit.tag == "Food" || objectHit.tag == "Cartridge")
                        {
                           Item item = hit.collider.GetComponent<Item>();
                            if (item != null)
                                {
                                //  set.inventar.Add(item);
                                    int count = 1;
                                    if (hit.collider.GetComponent<CountComponent>())
                                         count = hit.collider.GetComponent<CountComponent>().count;
                                    set.addInventar(item, count);
                                    GameObject.Destroy(hit.collider.gameObject);
                                }
                       }
                        if (objectHit.tag == "Enemy" || objectHit.tag == "Player_Enemy")
                        {
                            isFire = true;
                            SettingsWeapon _ws;
                            if (set.weaponActive == 0 && set.viewWeaponLeft)
                            {
                                _ws = set.viewWeaponLeft.GetComponent<SettingsWeapon>();
                            }
                            else
                            {
                                _ws = set.viewWeaponRight.GetComponent<SettingsWeapon>();
                            }
                            int shop = _ws.shop;

                            if (set.weaponActive == 1)
                                shop = _ws.shop;

                            if (set.od >= _ws.od && shop <= _ws.shopFull && shop >= _ws.cart_step)
                            {
                                set.od -= _ws.od;
                                if (set.weaponActive == 1)
                                _ws.shop -= _ws.od;
                                else
                                  _ws.shop -= _ws.od;

                            if (_ws.shop < 0)
                                _ws.shop = 0;
                            //Debug.Log("Fire");
                               tr.LookAt(new Vector3(objectHit.position.x, 0, objectHit.position.z));
                            if (!game) 
                                objectHit.GetComponent<LifeComponent>().onKick(10);

                                if (!set.view.GetComponent<Animator>().GetBool("fire")) 
                                    set.view.GetComponent<Animator>().SetBool("fire", true);
                                    


                                set.view.GetComponent<Animator>().fireEvents = true;
                                GameObject.Find("PlauerInfo").GetComponent<panelOD>().updateOD(set.maxOD, set.od);
                            if (game && objectHit.tag == "Enemy")
                            {
                                int pr = set.procentKillEnemy(objectHit.gameObject);
                                var dmg = (pr * _ws.losMax) / 100;
                                
                                if (dmg < _ws.losMin)
                                    dmg = _ws.losMin;
                                if (dmg > _ws.losMax)
                                    dmg = _ws.losMax;
                                if (pr < 20)
                                    dmg = 0;
                                Mob mob = objectHit.GetComponent<Mob>();

                                if(mob)
                                    mob.onDmg(dmg);
                                //game.playerAttack(objectHit.GetComponent<Setting>().photonID, _ws.losMin, _ws.losMax, pr);
                            }
                            if (game && objectHit.tag == "Player_Enemy")
                                {
                                     int pr = set.procentKillPlayer(objectHit.gameObject);
                                     game.playerAttack(objectHit.GetComponent<Setting>().photonID, _ws.losMin, _ws.losMax, pr);
                                }
                          
                                Boot.PlauerInfo.GetComponent<PanelInfo>().drawSett(set);
                                if (!Boot.batle)
                                {
                                    Boot.batle = true;
                                    GameObject pd = GameObject.Find("panelDown");
                                    if (pd)
                                          pd.GetComponent<ControllerPanelDown>().onBatle();
                                }
                            }
                        }
                           //  set.view.GetComponent<Animator>().fireEvents=true;
                    //   set.view.GetComponent<Animator>().SetBool("fire", false);

                    if (!isFire)
                            if (objectHit.tag == "Map")
                            {
                                var positi = player.tag[i];
                                Vector3 toGo = new Vector3(hit.point.x,0, hit.point.z);
                                List<Node> arr = GameObject.Find("Grid").GetComponent<Pathfinding>().FindPath(tr.position, toGo);
                                if (arr == null)
                                     return;
                                if (arr.Count > 0)
                                {
                                    toGo.x = arr[arr.Count - 1].worldPosition.x;
                                    toGo.y = 0;
                                    toGo.z = arr[arr.Count - 1].worldPosition.z;
                                }

                                if (Boot.batle || Boot.PVP)
                                    {
                                        int dist = (int)Vector3.Distance(hit.point, tr.position) + 1;
                                        if (dist <= set.od)
                                        {
                                            //positi.dist = hit.point;
                                            positi.dist = toGo;
                                          //  positi.welk = 1;
                                            set.od -= dist;                           
                                       //     GameObject.Find("PlauerInfo").GetComponent<panelOD>().updateOD(set.maxOD, set.od);
                                             panelOD.instance.createOD(set.maxOD, set.od);
                                         }
                                    }
                                    else
                                    {
                                        //positi.dist = hit.point;
                                        positi.dist = toGo;
                                     //  positi.welk = 1;
                                    }

                                    player.tag[i] = positi;
                            }
                    }
                        
                } 
        }
    }
}
