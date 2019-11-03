using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class BatleSystem : ComponentSystem
{
    struct Player
    {
        public ComponentArray<Setting> sett;
        public ComponentDataArray<BatleData> data;
        public ComponentDataArray<ActiveData> activ;
        public readonly int Length;
    }

    [Inject]Player player;
    protected override void OnUpdate()
    {
        for (int i = 0; i < player.Length; i++)    //show OD
        {
            var sett = player.sett[i];
            var data = player.data[i];
            var act = player.activ[i];
            if(player.Length==1)
            {
                if(act.showOd==0)
                {
                    act.showOd = 1;
                    player.activ[i]=act;
                  //  data.od = sett.maxOD;
                    CreateOD(sett.maxOD, sett.od);
                }                
            }  
            
        }

    }

    private void CreateOD(int maxOD,int od)
    {
        panelOD.instance.createOD(maxOD, od);
    }
}
