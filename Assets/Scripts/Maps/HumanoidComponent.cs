using Unity.Entities;
using System;
using System.Numerics;
using UnityEngine;

[Serializable]
public struct HumanoidData : IComponentData
//public struct HumanoidData : ISharedComponentData
{
  //  public bool activ;
   public UnityEngine.Vector3 dist;
//   public UnityEngine.Vector3 distStep;
    // public UnityEngine.Vector3 parent;
   public int activ;
   public int welk;
   public int welking;
   public int fire;
    public int stop;
    public byte died;
}

public class HumanoidComponent : ComponentDataProxy<HumanoidData> { };
//public class HumanoidComponent : SharedComponentDataWrapper<HumanoidData> { };

