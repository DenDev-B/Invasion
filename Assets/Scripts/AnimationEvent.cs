using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
      anim = GetComponentInParent<Animator>();
    }

    void FinishAnim()
    {
        if (anim.GetBool("fire"))
            anim.SetBool("fire", false);
    }
    void FinishZombiFire()
    {
        if (anim.GetBool("fire"))
            anim.SetBool("fire", false);
    }
    void ZombiEventWalkLoop()
    {
        if (anim.GetBool("walk"))
            anim.SetBool("walk", false);
        //Debug.Log("Stop");
    }
}
