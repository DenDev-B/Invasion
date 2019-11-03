using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ComandSystem : ComponentSystem
{
    struct Player
    {
        public ComponentArray<Setting> sett;
        public ComponentDataArray<ComandData> comand;
        public ComponentArray<ActiveComponent> agents; //
        public readonly int Length;
    }
  
  //  private bool ch;
   // private bool sh;

    [Inject] Player player;
    protected override void OnUpdate()
    {
       int  comandA = 0;
       int comandB = 0;
       int comandC = 0;
       // ch = false; 
       bool sh = false; 
        for (int i = 0; i < player.Length; i++)
        {
         //   Debug.Log("players  " + player.Length);
            var com = player.comand[i];
           
            if (com.comand == 0)
                comandC++;
            if (com.comand == 1)
            {
                comandA++;
            }
            if (com.comand == 2)
            {
                comandB++;
            }
            Debug.Log("Comand -->" + comandA + "    " + comandB + "    " + comandC);
            if (i == player.Length - 1)
                sh = true;
        }
       
       // if (ch && sh)
        if (comandC>0 && (comandA + comandB + comandC == player.Length))
        {
            Debug.Log("sh --- -Comand " + comandA + "    " + comandB);
            for (int i = 0; i < player.Length; i++)
            {
                var com = player.comand[i];
                var set = player.sett[i];
                if (com.comand == 0)
                {
                    if(comandA<= comandB)
                    {
                        com.comand = 1;
                    }else
                        com.comand = 2;

                    player.comand[i] = com;
                    set.comanda = com.comand;
                    Debug.Log("You      comand-->"+ com+"     settt " + player.Length + "Comand " + comandA + "    " + comandB);
                    return;
                }
            }
           // Debug.Log("players return");
        }

    }
}
