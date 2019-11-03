using System;
using Unity.Entities;
using UnityEngine;


[Serializable]
public struct CursorData : IComponentData
{
}

public class CursoreComponent : ComponentDataProxy<CursorData> { };