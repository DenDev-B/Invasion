using System;
using Unity.Entities;


[Serializable]
public struct ZombiData : IComponentData
{
    public int walk;
    public int walking;
    public UnityEngine.Vector3 dist;
    public int activ;
    public byte died;
    public int fire;
    public int targetID;
}

public class ZombiComponent : ComponentDataProxy<ZombiData> { };