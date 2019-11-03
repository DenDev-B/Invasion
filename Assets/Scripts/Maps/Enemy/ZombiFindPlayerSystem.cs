using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ZombiFindPlayerSystem : ComponentSystem
{
 //   Pathfinding pathfinding;
    struct Player
    {
        public ComponentArray<Setting> sett;
        public ComponentArray<Transform> tr;
        public ComponentDataArray<HumanoidData> tag;
        public readonly int Length;
    }
    struct Zombi
    {
        public ComponentArray<SettingsEnimy> sett;
        public ComponentArray<Transform> tr;
        public ComponentDataArray<ZombiData> tag;
        public readonly int Length;
    }
    ZombiData zz;

    protected override void OnStartRunning()
    {
       // pathfinding = GetComponent<Pathfinding>();
    }

    [Inject] Player player;
    [Inject] Zombi zombi;
    protected override void OnUpdate()
    {
        if(Boot.PVP)
        {
            return;
        }

        if (Boot.batle && Boot.STEP)
        {
            resetSettings();
            return;
        }

        if (!Boot.batle)
        {
            if(chekActice()==-1)
               if( findNewActivZombi())
                   Boot.onBatle();
        }
        if (Boot.batle)
        {
            if (checkEndStep())
            {
                Boot.onStep(true);
                return;
            }
            int ca = chekActice();
            if (ca==-1)
            {
                findNewActivZombi();
            }
            ca = chekActice();
            if (ca == -1)
            {
                // Boot.offBatle();
                if (!checkEndStep())
                {
                    Boot.onStep(true);
                    return;
                }
                return;
            }
            // добавить проверку на жизни персонажа
            var zombiTr = zombi.tr[ca];
            var zombiSet = zombi.sett[ca];
            var zombiTag = zombi.tag[ca];
          
            float max = zombiSet.radiusShow + 1;
            int idTarget = -1;
             Boot.cam.GetComponent<CameraControl>().onPosition(zombiTr.position);
            for (int j = 0; j < player.Length; j++)
            {
                var plTr = player.tr[j];
                var plSett = player.sett[j];
                Debug.Log("find player" + j +"  " + plSett.viewHero);
                if (plTr.GetComponent<LifeComponent>().life <= 0)
                {
                    plTr.GetComponent<LifeComponent>().onKillPlayer();
                    return;
                }

                float dist = Vector3.Distance(zombiTr.position, plTr.position);
                if (dist < zombiSet.radiusShow)
                {
                    if (dist < max)
                    {
                        idTarget = j;
                        max = dist;
                    }
                }

            }
            if (idTarget == -1)
                return;
            var playerTr = player.tr[idTarget];
            var playerTrs = player.sett[idTarget];
            Debug.Log(" player" + idTarget + "  " + playerTrs.viewHero);
            if (idTarget > -1)// игрок подходит для цели.
            {
                zombiSet.findGO = playerTr;
           //     zombiTr.rotation = Quaternion.Euler(0, 90, 0);
                //    zombiTr.rotation = Quaternion.Euler(playerTr.transform.position.x, playerTr.transform.position.y, playerTr.transform.position.z);
                //  zombiTr.Rotate(Vector3.up, playerTrs.transform.position.x, Space.World);
                if (max>1.5f)
                {
                  //  Debug.Log("dist "+max);
                    if (zombiTag.walking == 0 )
                    {
                     //   Debug.Log("walk");
                        List<Node> arr = GameObject.Find("Grid").GetComponent<Pathfinding>().FindPath(zombiTr.position, playerTr.position);
                        if (arr.Count > 0 && zombiSet.od > 0)
                        {
                            zombiTag.dist = arr[0].worldPosition;
                            zombiSet.od -= 1;
                            zombiTag.walk = 1;
                            zombiTag.walking = 1;
                        }
                      /*  if(arr.Count==1)
                        {
                            Debug.Log("arr.Count==1");
                        }*/
                       // zombi.tag[ca] = zombiTag;
                    }
                }
                else
                {
                    if (zombiSet.od > zombiSet.damageOd)
                    {
                        if (playerTr.GetComponent<LifeComponent>().life > 0)
                        {
                            if (!zombiSet.view.GetComponent<Animator>().GetBool("fire"))
                            {
                                Debug.Log("Fire ");
                                zombiSet.od -= zombiSet.damageOd;
                                playerTr.GetComponent<LifeComponent>().onKickPlayer(5);
                                zombiSet.view.GetComponent<Animator>().SetBool("fire", true);
                                
                                // zombiTag.fire = 1;
                            }
                        }
                        else
                            Debug.Log("");

                     }
                        else
                        {
                            Debug.Log("End step");
                            zombiSet.od = 0;
                            zombiTag.activ = -1;
                        }
              
                }
                zombi.tag[ca] = zombiTag;
            }
           



        }

          

        }

    
    private void resetSettings()
    {
        for (int i = 0; i < zombi.Length; i++)
        {
            var zombiTag = zombi.tag[i];
            var zombiSet = zombi.sett[i];
            if (zombiTag.activ == -1)
            {
                zombiTag.activ = 0;
                zombi.tag[i] = zombiTag;
                zombiSet.od = zombiSet.maxOD;
               
            }
        }
    }

    private bool checkEndStep()
    {
        bool ch = true;
        for (int i = 0; i < zombi.Length; i++)
        {
            var zombiTag = zombi.tag[i];
            if (zombiTag.activ != -1)
            {
                ch = false;
                break;
            }
        }
        return ch;
    }

    private int chekActice()
    {
        for (int i = 0; i < zombi.Length; i++)
        {
            var zombiTag = zombi.tag[i];
            if (zombiTag.activ==1)
               if(checkOd(i))
                return i;
        }
        return -1;
    }

    private bool checkOd(int i)
    {
        var zt = zombi.tag[i];
        var zo = zombi.sett[i];
        if (zt.activ == 1 && zo.od > 0)
        {
            return true;
        }
        return false;
    }


    private bool findNewActivZombi() // поиск и активация зомби.
    {
        for (int i = 0; i < zombi.Length; i++)
        {
            var zombiTr = zombi.tr[i];
            var zombiSet = zombi.sett[i];
            var zombiTag = zombi.tag[i];
            if (zombiTag.activ == -1)
                continue;
          //  float max = zombiSet.radiusShow + 1;
          //  int idTarget = -1;
            for (int j = 0; j < player.Length; j++)
            {
                var playerTr = player.tr[j];
                var playerData = player.tag[j];
                float dist = Vector3.Distance(zombiTr.position, playerTr.position);
                if (dist < zombiSet.radiusShow)
                {
                    /*  if (dist < max)
                       {
                           idTarget = j;
                           max = dist;
                       }*///
                    if(!Boot.batle && playerData.dist != playerTr.transform.position)
                    {
                        playerData.stop = 1;
                        player.tag[j] = playerData;
                    }
                    if (zombiTag.activ == 1 && Boot.batle)
                    {
                        zombiTag.activ = -1;
                       // zombi.tag[i] = zombiTag;
                    }
                    else
                    {
                        zombiTag.activ = 1;
                        zombi.tag[i] = zombiTag;
                        return true;
                    }
                }
                else
                {
                    if (zombiTag.activ == 0 && Boot.batle)
                    {
                        zombiTag.activ = -1;
                        zombi.tag[i] = zombiTag;
                    }
                }
            }
          /*  if(idTarget>=0)
            {
                zombiTag.activ = 1;
                zombiTag.targetID = idTarget;
                zombi.tag[i] = zombiTag;
            }*/
        }
        return false;
    }
    /*
private void zombiGoStep(Vector3 start, Vector3 end)
{
   PathRequestManager.RequestPath(start, end, OnPathFound);
}

public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
{
   if (pathSuccessful)
   {
       Vector3[] path = newPath;
       zz.dist = newPath[0];
   }
}*/
}
