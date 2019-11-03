using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<LifeComponent>().life = GetComponent<LifeComponent>().full_life;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void onDmg(int dmg)
    {
        Debug.Log("TOWER dmg "+ dmg);
        if (GetComponent<LifeComponent>().life <= 0)
        {
            LoadingPVP.Load(LoadingScens.Menu);
            return;
        }
        GetComponent<LifeComponent>().life -= dmg;

        if (GetComponent<LifeComponent>().life <= 0)
        {
            LoadingPVP.Load(LoadingScens.Menu);
            return;
        }
    }
}
