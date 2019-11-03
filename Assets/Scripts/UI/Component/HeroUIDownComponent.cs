using Unity.Entities;
using System;
using System.Numerics;
using UnityEngine;

[Serializable]
public struct HeroUIDownData : IComponentData
{
    public byte active;
}

public class HeroUIDownComponent : ComponentDataProxy<HeroUIDownData> { };


