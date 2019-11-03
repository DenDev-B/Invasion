using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePanel : MonoBehaviour
{
   public Transform contrrolPanelDown;
   public void nextStep(bool start=false)
    {
        GameObject[] gs = GameObject.FindGameObjectsWithTag("Player");
        for(int i=0;i<gs.Length;i++)
        {
            gs[i].GetComponent<Setting>().od = gs[i].GetComponent<Setting>().maxOD;
            if(gs[i].GetComponent<ActiveComponent>())
            {
                panelOD.instance.createOD(gs[i].GetComponent<Setting>().od, gs[i].GetComponent<Setting>().od);
            }
        }
        GameObject[] en = GameObject.FindGameObjectsWithTag("Enemy");
        for (int j = 0; j < en.Length; j++)
        {
            en[j].GetComponent<SettingsEnimy>().od = en[j].GetComponent<SettingsEnimy>().maxOD;
        }
        if (start)
        {
            Boot.onStep(true);
        }else
        {
            Boot.onStep(false);
        }
    }

    public void endBattle()
    {
        Debug.Log("end Batle");
        contrrolPanelDown.GetComponent<ControllerPanelDown>().endBatle();
    }
}
