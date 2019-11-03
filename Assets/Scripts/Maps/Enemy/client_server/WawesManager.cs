using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave
{
    public int MobCount { get; set; }
    public Wave(int count)
    {
        MobCount = count;
    }
}

public class WawesManager : MonoBehaviour
{
    public readonly static List<Wave> Waves = new List<Wave>(new[]
    {
        new Wave(1),
        new Wave(3),
        new Wave(6),
        new Wave(8)
    });
   
}
