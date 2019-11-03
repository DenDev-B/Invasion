using UnityEngine;

public class HandWeapons : MonoBehaviour
{
    [SerializeField]
    public Transform hand;
    public int od;
    public bool active;

    public Transform Hand { get; internal set; }
}
