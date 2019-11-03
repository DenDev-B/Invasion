using System;
using Unity.Entities;
using UnityEngine;


[Serializable]
public struct ActiveData : IComponentData
{
    public byte infoShow;
    public int showOd;
}

public class ActiveComponent : ComponentDataProxy<ActiveData> { };