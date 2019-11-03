using System;
using Unity.Entities;
using UnityEngine;


[Serializable]
public struct ComandData : IComponentData
{
    public int comand; //0 - никто 
}

public class ComandComponent : ComponentDataProxy<ComandData> { };