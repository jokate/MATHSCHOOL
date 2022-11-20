using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggerFortutorial : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer renderr;
    public void LeftAnimSet()
    {
        anim.SetFloat("xdir", -1);
        anim.SetFloat("ydir", 0);
        renderr.flipX = true;
    }
    public void RightAnimSet()
    {
        anim.SetFloat("xdir", 1);
        anim.SetFloat("ydir", 0);
        renderr.flipX = false;
    }
    public void UpAnimSet()
    {
        anim.SetFloat("xdir", 0);
        anim.SetFloat("ydir", 1);
        renderr.flipX = false;
    }
    public void DownAnimSet()
    {
        anim.SetFloat("xdir", 0);
        renderr.flipX = false;
        anim.SetFloat("ydir", -1);
    }
}
