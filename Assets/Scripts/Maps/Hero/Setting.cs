using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AI;

public class Setting : MonoBehaviour
{
    [System.Serializable]
    public class InventarArr
    {
        public Item item;
        public int count;
    }

    public GameObject player;

    public List<InventarArr> inventar;
   // public List<Item> inventar;
    public GameObject view;
    public GameObject life;
    public int maxOD;
    public int od;
  //  public int shopLeftWeapo;
//    public int shopRigthtWeapo;

    public Transform parent;// можно заменить на player
    public string viewHero;
    public string playerName;
    public int photonID;

    public GameObject weapon;
    public Transform parentWeapon;
    public byte weaponActive;
    public string nameWeaponLeft;
    public string nameWeaponRight;
    public GameObject viewWeaponLeft;
    public GameObject viewWeaponRight;
    public int comanda=0;
    GameObject boot;

    public Transform curTarget;


    void Start()
    {
        //   shopLeftWeapo = 15;
        // shopRigthtWeapo = 15;
        // weaponActive = 0;
        if (Boot.PVP) { 
            string wl = (string)PhotonNetwork.player.customProperties["lWeapon"];
              if (wl!=null)
                 nameWeaponLeft = wl+"V2";
            Debug.Log(" lWeapon " + wl);

            string wr = (string)PhotonNetwork.player.customProperties["rWeapon"];
            if (wr != null)
                nameWeaponRight = wr + "V2";
            Debug.Log(" rWeapon " + wr); 
           }
         boot = GameObject.FindGameObjectWithTag("App");
        if (viewHero.Length>0)
          LoadView();
        od = maxOD;

    }
    public void addInventar(Item _it,int count=1)
    {
        int _count = inventar.Count;
        for (int i = 0; i < _count; i++)
        {
          var  it = inventar[i];
            if (it.item.name == _it.name)
            {
                it.count += count;
                return;
            }
        }
        InventarArr ia = new InventarArr();
        ia.item = _it;
        ia.count = count;
        inventar.Add(ia);
    }
    
    public int coountOnInventar(string name)
    {
        int _count = inventar.Count;
        for (int i = 0; i < _count; i++)
        {
            var it = inventar[i];
            if (it.item.name == name)
            {
                return it.count;
            }
        }
         return 0;
    }
    public int dellItemInventar(string name, int count)
    {

        int _count = inventar.Count;
        for (int i = 0; i < _count; i++)
        {
            var it = inventar[i];
            if (it.item.name == name)
            {
                if (it.count > count)
                {
                    it.count -= count;
                    return  count;
                }
                else if (it.count == count)
                {
                    inventar.Remove(it);
                    return count;
                }else if (it.count >0 && it.count  < count)
                {
                    int r = it.count;
                    inventar.Remove(it);
                    return r;
                }
                else
                    return 0;
            }
        }
        return 0;
    }


        // Frame update example: Draws a 10 meter long green line from the position for 1 frame.
        void Update()
        {
            if(hhh && eee)
            {

            //Vec  forward = forward.TransformDirection(Vector3.forward) * 10;
            //   Debug.DrawRay(new Vector3(hhh.position.x, hhh.position.y+1f, hhh.position.z), eee.position, Color.red);
             
            }
           
        }

    private Transform hhh;
    private Transform eee;
    public int procentKillEnemy(GameObject emn)
    {
        int rezult = 0;
        if (!emn)
            return 0; 
        SettingsEnimy se = emn.GetComponent<SettingsEnimy>();
        if (!se)
            return 0;
        EnemySkelet esk;
        if(Boot.PVP)
            esk = se.GetComponent<EnemySkelet>();
        else
            esk= se.view.GetComponent<EnemySkelet>();
        if (!esk)
            return 0;

        HeroSkelet hsk = view.GetComponent<HeroSkelet>();
   //    Transform hhh2 = hsk.headRey.transform;
    //    hhh = view.transform;
        //eee = esk.head.transform;
     //   eee = emn.transform;

        RaycastHit hit;

        Ray ray = new Ray(hsk.headRey.position, esk.head.position - hsk.headRey.position);
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Enemy" || hit.collider.name == esk.head.name) { 
              //  Debug.Log("emn head " + hit.collider.tag);
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                rezult += 16;
            }
        }
        else
        {
            Debug.Log("Not Hand");
        }

        ray = new Ray(hsk.headRey.position, esk.spaine.position - hsk.headRey.position);
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
         //   Debug.Log("emn spaine " + hit.collider.tag);
           
            if (hit.collider.tag == "Enemy" || hit.collider.name == esk.spaine.name)
            {
             //   Debug.Log("emn spaine");
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                rezult += 16;
            }
        }
        else
        {
            Debug.Log("Not Spine");
        }
        ray = new Ray(hsk.headRey.position, esk.lHand.position - hsk.headRey.position);
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Enemy" || hit.collider.name == esk.lHand.name)
            {
              //  Debug.Log("emn " + hit.collider.tag);
                Debug.DrawLine(ray.origin, hit.point, Color.green);
                rezult += 16;
            }
        }
        else
        {
            Debug.Log("Not left Hand");
        }
        ray = new Ray(hsk.headRey.position, esk.rHand.position - hsk.headRey.position);
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Enemy" || hit.collider.name == esk.rHand.name)
            {
              //  Debug.Log("emn " + hit.collider.tag);
                Debug.DrawLine(ray.origin, hit.point, Color.green);
                rezult += 16;
            }
        }
        else
        {
            Debug.Log("Not right Hand");
        }
        ray = new Ray(hsk.headRey.position, esk.lCaft.position - hsk.headRey.position);
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Enemy" || hit.collider.name == esk.lCaft.name)
            {
              //  Debug.Log("emn " + hit.collider.tag);
                Debug.DrawLine(ray.origin, hit.point, Color.blue);
                rezult += 16;
            }
        }
        else
        {
            Debug.Log("Not lrft Caft");
        }
        ray = new Ray(hsk.headRey.position, esk.rCaft.position - hsk.headRey.position);
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Enemy" || hit.collider.name == esk.rCaft.name)
            {
              //  Debug.Log("emn right Caft");
                Debug.DrawLine(ray.origin, hit.point, Color.blue);
                rezult += 16;
            }
        }
        else
        {
            Debug.Log("Not right Caft");
        }
       
        return rezult;
    }
    public int procentKillPlayer(GameObject emn)
    {
        int rezult = 0;
        if (!emn)
            return 0;
        Setting se = emn.GetComponent<Setting>();
        if (!se)
            return 0;
        EnemySkelet esk = se.view.GetComponent<EnemySkelet>();
        if (!esk)
            return 0;

        HeroSkelet hsk = view.GetComponent<HeroSkelet>();
        //    Transform hhh2 = hsk.headRey.transform;
        //    hhh = view.transform;
        //eee = esk.head.transform;
        //   eee = emn.transform;

        RaycastHit hit;

        Ray ray = new Ray(hsk.headRey.position, esk.head.position - hsk.headRey.position);
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player_Enemy" || hit.collider.name == esk.head.name)
            {
               // Debug.Log("Player head " + hit.collider.tag);
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                rezult += 16;
            }
        }
        else
        {
            Debug.Log("Not Hand");
        }

        ray = new Ray(hsk.headRey.position, esk.spaine.position - hsk.headRey.position);
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
            //   Debug.Log("emn spaine " + hit.collider.tag);

            if (hit.collider.tag == "Player_Enemy" || hit.collider.name == esk.spaine.name)
            {
              //  Debug.Log("Player spaine");
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                rezult += 16;
            }
        }
        else
        {
          //  Debug.Log("Not Spine");
        }
        ray = new Ray(hsk.headRey.position, esk.lHand.position - hsk.headRey.position);
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player_Enemy" || hit.collider.name == esk.lHand.name)
            {
               // Debug.Log("emn " + hit.collider.tag);
                Debug.DrawLine(ray.origin, hit.point, Color.green);
                rezult += 16;
            }
        }
        else
        {
          //  Debug.Log("Not left Hand");
        }
        ray = new Ray(hsk.headRey.position, esk.rHand.position - hsk.headRey.position);
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player_Enemy" || hit.collider.name == esk.rHand.name)
            {
              //  Debug.Log("emn " + hit.collider.tag);
                Debug.DrawLine(ray.origin, hit.point, Color.green);
                rezult += 16;
            }
        }
        else
        {
         //   Debug.Log("Not right Hand");
        }
        ray = new Ray(hsk.headRey.position, esk.lCaft.position - hsk.headRey.position);
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player_Enemy" || hit.collider.name == esk.lCaft.name)
            {
               // Debug.Log("emn " + hit.collider.tag);
                Debug.DrawLine(ray.origin, hit.point, Color.blue);
                rezult += 16;
            }
        }
        else
        {
          //  Debug.Log("Not lrft Caft");
        }
        ray = new Ray(hsk.headRey.position, esk.rCaft.position - hsk.headRey.position);
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player_Enemy" || hit.collider.name == esk.rCaft.name)
            {
            //    Debug.Log("emn right Caft");
                Debug.DrawLine(ray.origin, hit.point, Color.blue);
                rezult += 16;
            }
        }
        else
        {
          // Debug.Log("Not right Caft");
        }

        return rezult;
    }


    public void LoadWeapons(int type = 0) //0 - full, 1-left, // 2 - righgt
    {
        
       String nameWeapon = nameWeaponLeft;
        if (type == 0 || type == 1)
        {
            if (nameWeapon == nameWeaponLeft && (nameWeaponLeft == null || nameWeaponLeft.Length == 0 || !parentWeapon))
            { }
            else { 
            if (viewWeaponLeft)
                Destroy(viewWeaponLeft);
            viewWeaponLeft = Instantiate(Resources.Load<GameObject>("Weapon/" + nameWeapon));
            viewWeaponLeft.transform.SetParent(player.transform);
            viewWeaponLeft.transform.position = player.transform.position;
            viewWeaponLeft.name = string.Format(nameWeapon);
            if (weaponActive == 0)
                LoadWeapon();

             SettingsWeapon wSetL = viewWeaponLeft.GetComponent<SettingsWeapon>();
             if (wSetL.shop < wSetL.shopFull)
                {
                    int full = coountOnInventar(wSetL.cartridge);
                    if (full > 0)
                    {
                        int need = wSetL.shopFull - wSetL.shop;
                        // if(full >= need)
                        //{
                        int add = dellItemInventar(wSetL.cartridge, need);
                        wSetL.shop = add;
                      //  drawWeaponL(_sett.nameWeaponLeft);
                        // }
                    }
                }
            }
        }

        nameWeapon = nameWeaponRight;
        if (type == 0 || type == 1)
        {
            if (nameWeapon == nameWeaponRight && (nameWeaponRight == null || nameWeaponRight.Length == 0 || !parentWeapon))
            { }
            else
            {
                if (viewWeaponRight)
                    Destroy(viewWeaponRight);
                viewWeaponRight = Instantiate(Resources.Load<GameObject>("Weapon/" + nameWeapon));
                viewWeaponRight.transform.SetParent(player.transform);
                viewWeaponRight.transform.position = player.transform.position;
                viewWeaponRight.name = string.Format(nameWeapon);
                if (weaponActive == 0)
                    LoadWeapon();

                SettingsWeapon wSetR = viewWeaponRight.GetComponent<SettingsWeapon>();
                if (wSetR.shop < wSetR.shopFull)
                {
                    int full = coountOnInventar(wSetR.cartridge);
                    if (full > 0)
                    {
                        int need = wSetR.shopFull - wSetR.shop;
                        // if(full >= need)
                        //{
                        int add = dellItemInventar(wSetR.cartridge, need);
                        wSetR.shop = add;
                       // drawWeaponR(_sett.nameWeaponRight);
                        // }
                    }
                }
            }
        }
    }
    public void LoadWeapon()
    {
        String nameWeapon = nameWeaponLeft;
        if (weaponActive == 1)
            nameWeapon = nameWeaponRight;

        if (nameWeapon == nameWeaponLeft && (nameWeaponLeft == null || nameWeaponLeft.Length == 0 || !parentWeapon))
            return;

        if (nameWeapon == nameWeaponRight && (nameWeaponRight == null || nameWeaponRight.Length == 0 || !parentWeapon))
            return;

        if (weapon)
            Destroy(weapon);

        if (weaponActive == 0)
        {
            weapon = Instantiate(viewWeaponLeft.GetComponent<SettingsWeapon>().view);
        } else if (weaponActive == 1)
        {
            weapon = Instantiate(viewWeaponRight.GetComponent<SettingsWeapon>().view);
        } else
            return;
     
       // weapon = Instantiate(R);

        weapon.transform.SetParent(parentWeapon);

        var wp = weapon.transform.position;
        var wr = weapon.transform.rotation;

        weapon.name = string.Format(nameWeapon);
        weapon.transform.position = parentWeapon.transform.position;
        weapon.transform.rotation = parentWeapon.transform.rotation;
    }

    private void LoadView()
    {
        if (!Boot.PVP)
        {
            if (view)
                Destroy(view);
            view = Instantiate(Resources.Load<GameObject>("Hero/" + viewHero));
            view.transform.SetParent(parent);
            view.transform.position = parent.position;
        }
        view.name = string.Format(viewHero);
        HandWeapons ww = view.GetComponent<HandWeapons>();
        parentWeapon = ww.hand;
        LoadWeapons();
       // LoadWeapon();

      //  player.AddComponent<HeroUIDownComponent>();
      //  player.AddComponent<HeroBttnComponent>();
      //  player.AddComponent<ActiveComponent>();
    }

    public void dellComponent()
    {
        /*if (GetComponent<HeroBttnComponent>())
        {

         }*/
    }

   
}
