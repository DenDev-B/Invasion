using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine.AI;

public class WalkSystem : ComponentSystem
{
    // ComponentGroup group;
    struct Player
    {
        public ComponentArray<Setting> sett;
        public ComponentDataArray<HumanoidData> tag;
     //   public ComponentArray<NavMeshAgent> agents;
        public ComponentArray<Transform> poistion;
        public readonly int Length;
    }

    struct Zombi
    {
        public ComponentArray<SettingsEnimy> sett;
        public ComponentDataArray<ZombiData> tag;
     //   public ComponentArray<NavMeshAgent> agents;
        public ComponentArray<Transform> poistion;
        public readonly int Length;
    }

    [Inject] Player player;
    [Inject] Zombi zombi;
    protected override void OnUpdate()
    {
        for (int i = 0; i < player.Length; i++)
        {
            var positi = player.tag[i];
         //   var agent = player.agents[i];
            var posHero = player.poistion[i];
            var sett = player.sett[i];

            if (positi.dist != Vector3.zero && positi.dist != posHero.position)
            {
                Boot.cam.GetComponent<CameraControl>().onPosition(sett.view.transform.position);
               
                List<Node> arr = GameObject.Find("Grid").GetComponent<Pathfinding>().FindPath(posHero.position, positi.dist);
                /* if(arr.Count>0 && (positi.dist.x != arr[arr.Count - 1].worldPosition.x || positi.dist.z != arr[arr.Count - 1].worldPosition.z))
                 {
                     positi.dist.x = arr[arr.Count-1].worldPosition.x;
                     positi.dist.y =0;
                     positi.dist.z = arr[arr.Count-1].worldPosition.z;
                 }       */
                //   if (!Boot.batle)
                //   {
                    if (positi.stop == 1)
                    {
                        if(arr.Count>0)
                        {
                            positi.dist.x = arr[0].worldPosition.x;
                            positi.dist.y = 0;
                            positi.dist.z = arr[0].worldPosition.z;
                            positi.stop = 0;
                         }    
                    }
                    if (arr.Count > 0 && (positi.welk==0 || positi.welking==1))
                    {
                        posHero.LookAt(new Vector3(arr[0].worldPosition.x, 0, arr[0].worldPosition.z));
                        posHero.position = Vector3.MoveTowards(posHero.position, new Vector3(arr[0].worldPosition.x, 0, arr[0].worldPosition.z), Time.deltaTime * 2);
                        positi.welk = 1;
                        positi.welking = 1;
                        if (!sett.view.GetComponent<Animator>().GetBool("walk"))
                        {
                            sett.view.GetComponent<Animator>().SetBool("walk", true);

                        }
                        //posHero.LookAt(new Vector3(arr[0].worldPosition.x, 0, arr[0].worldPosition.z));
                    }
                    
                    Vector3 p1 = new Vector3(posHero.position.x, 0, posHero.position.z);
                    Vector3 p2 = new Vector3(positi.dist.x, 0, positi.dist.z);
                    float dist = (float)Vector3.Distance(p1, p2);
                    if ((p1 == p2 || dist < 0.3f) && positi.welk == 1)
                    {
                        //if()
                        //Debug.Log("hero Stop");
                       // if (sett.view.GetComponent<Animator>().GetBool("walk"))
                       // {
                       //     sett.view.GetComponent<Animator>().SetBool("walk", false);
                        positi.welk = 0;
                        positi.welking = 0;
                        if (sett.view.GetComponent<Animator>().GetBool("walk"))
                        {
                           sett.view.GetComponent<Animator>().SetBool("walk", false);
                        }
                    }
                    if ((p1 != p2 || dist > 0.3f) && positi.welk == 0 && arr.Count == 0)
                    {
                        positi.welk = 0;
                        positi.welking = 0;
                    //     if (sett.view.GetComponent<Animator>().GetBool("walk"))
                    //    {
                           sett.view.GetComponent<Animator>().SetBool("walk", true);
                           sett.view.GetComponent<Animator>().SetBool("walk", false);
                     //   }
                    }
                    if ((p1 != p2 || dist > 0.3f) && positi.welk ==1 && arr.Count == 0)
                    {
                        posHero.LookAt(new Vector3(p2.x, 0, p2.z));
                        posHero.position = Vector3.MoveTowards(posHero.position, new Vector3(p2.x, 0, p2.z), Time.deltaTime * 2);
                    }
                }
                    player.tag[i] = positi;
                   // player.poistion[i] = posHero;
          //  }

             /*  if (arr.Count > 0 && zombiSet.od > 0)
                {
                    zombiTag.dist = arr[0].worldPosition;
                    zombiSet.od -= 1;
                    zombiTag.walk = 1;
                    zombiTag.walking = 1;
                }
            }
            /*
            if (positi.dist != Vector3.zero && positi.dist != posHero.position)
            {
                Boot.cam.GetComponent<CameraControl>().onPosition(sett.view.transform.position);
                if (positi.stop == 1)
                {
                    positi.welk = 0;
                    positi.stop = 0;
                    positi.dist = posHero.position;
               //     agent.Stop();
              //      agent.stoppingDistance = agent.radius;

                    player.tag[i] = positi;
                    if (sett.view.GetComponent<Animator>().GetBool("walk"))
                    {
                        sett.view.GetComponent<Animator>().SetBool("walk", false);
                    }
                    player.tag[i] = positi;
                    return;
                }

                if (positi.welk == 1)
                {            
                    positi.welk = 0;
                    player.tag[i] = positi;

              //      agent.destination = positi.dist;
                    if (!sett.view.GetComponent<Animator>().GetBool("walk"))
                    {
                        sett.view.GetComponent<Animator>().SetBool("walk", true);
                       
                    }
                }

            }
            int dist = (int)Vector3.Distance(positi.dist, posHero.position);
            if (positi.dist == posHero.position || dist == 0)
            {

                if (sett.view.GetComponent<Animator>().GetBool("walk"))
                {
                    sett.view.GetComponent<Animator>().SetBool("walk", false);
                }
            }*/
        }
        for (int j = 0; j < zombi.Length; j++)
        {
       
            var zPositi = zombi.tag[j];
          //  var zAgent = zombi.agents[j];
            var zPosHero = zombi.poistion[j];
            var zSett = zombi.sett[j];
            //      GameObject targ = GameObject.FindGameObjectWithTag("Box");
            //    Debug.Log("zPositi not Zero");
            //if(zSett.findGO)
            // zPosHero.LookAt(zSett.findGO.position);
            if (zPositi.walking == 1)
            {
                if (zPositi.dist != Vector3.zero && zPositi.dist != zPosHero.position)
                {
                   
                    Debug.Log("zPositi not Zero");
                    if (zPositi.walk == 1)
                    {
                        Debug.Log("zPositi Walk");
                        zPosHero.LookAt(new Vector3(zPositi.dist.x, 0, zPositi.dist.z));
                        //  zPosHero.position = Vector3.MoveTowards(zPosHero.position, zPositi.dist, Time.deltaTime*4);
                        zPosHero.position = Vector3.MoveTowards(zPosHero.position, new Vector3(zPositi.dist.x,0,zPositi.dist.z), Time.deltaTime * 1);
                        /// 
                        //
                        //  zPosHero.transform.Translate ( Vector3.Normalize(zPositi.dist - zPosHero.position) *Time.deltaTime * 1);
                        //   int angle = Quaternion.Euler(zPositi.dist.x, zPositi.dist.y, zPositi.dist.z);
                        //   zPosHero.Rotate(Vector3.up, angle, Space.Self);
                        //   zPosHero.rotation = Quaternion.LookRotation(zPosHero.forward, new Vector3(zPositi.dist.x, zPositi.dist.y, 0));
                        //  zPosHero.LookAt(new Vector3(0, zPositi.dist.y, 0));// = Quaternion.LookRotation(zPosHero.forward, new Vector3(zPositi.dist.x, zPositi.dist.y, 0));
                        //     zPosHero.rotation = Quaternion.LookRotation(zPositi.dist);
                        if (!zSett.view.GetComponent<Animator>().GetBool("walk"))
                        {
                            zSett.view.GetComponent<Animator>().SetBool("walk", true);
                        }


                    }
                }
                Vector3 st_z = new Vector3(zPositi.dist.x, 0, zPositi.dist.z);
            
                float dist = (float)Vector3.Distance(st_z, zPosHero.position);

                if (st_z == zPosHero.position /*|| dist < .1f*/)
                {
                    //if()
                    Debug.Log("zPositi Stop");
                    //  if (zSett.view.GetComponent<Animator>().GetBool("walk"))
                    //{
                    //zSett.view.GetComponent<Animator>().SetBool("walk", false);
                       zPositi.walk = 0;
                    
                    //}
                }
                if (zPositi.walk == 0 && !zSett.view.GetComponent<Animator>().GetBool("walk"))
                    zPositi.walking = 0;

                zombi.tag[j] = zPositi;
            }
        }
    }
}
