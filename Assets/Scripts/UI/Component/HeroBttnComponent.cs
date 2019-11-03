using Unity.Entities;
using System;
using System.Numerics;
using UnityEngine;

[Serializable]
public struct HeroBttnData : IComponentData
{
    public byte button;
}

public class HeroBttnComponent : ComponentDataProxy<HeroBttnData> { };
