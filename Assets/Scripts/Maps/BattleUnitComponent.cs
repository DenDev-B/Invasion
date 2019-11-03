using System;
using Unity.Entities;
using UnityEngine;


[Serializable]
public struct BatleData : IComponentData
{
    public int step;
}

public class BattleUnitComponent : ComponentDataProxy<BatleData> { };