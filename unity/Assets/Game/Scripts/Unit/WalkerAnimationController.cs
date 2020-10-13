using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerAnimationController : MonoBehaviour, IAnimationController
{
    private CSC_ACS_Actions animator;
    private Animations currentAnimation;

    public void Start()
    {
        this.animator = gameObject.GetComponentInChildren<CSC_ACS_Actions>();
    }

    public void Start(Animations animation)
    {
        if (this.currentAnimation == animation)
        {
            return;
        }

        this.currentAnimation = animation;

        switch (animation)
        {
            case Animations.Walk:
                animator.WalkForwad();
                break;
            default:
                animator.Idle();
                break;
        }
    }

    public void Stop(Animations animation)
    {
        animation = Animations.Idle;
        switch (animation)
        {
            case Animations.Walk:
                animator.Idle();
                break;
            default:
                animator.Idle();
                break;
        }
    }

    
}
