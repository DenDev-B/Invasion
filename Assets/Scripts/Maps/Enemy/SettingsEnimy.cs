using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsEnimy : MonoBehaviour
{
    public GameObject enimy;
    public GameObject enimyRad;
    public GameObject view;
    public Transform findGO;
    public string viewEnemy;
    public int radiusShow;
    public int maxOD;
    public int od;
    public int damage;
    public int damageOd;
    

    void Start()
    {
        if (viewEnemy.Length > 0)
            LoadView();
    }

    private void LoadView()
    {
        if (view)
            Destroy(view);
        view = Instantiate(Resources.Load<GameObject>("Enemy/" + viewEnemy));
        view.transform.SetParent(enimy.transform);
        view.transform.position = enimy.transform.position;
        view.name = string.Format(viewEnemy);
      
        enimyRad.transform.localScale =new Vector3(radiusShow, 0.1f, radiusShow);
       
    }
}
