using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Animations
{
    Walk,
    Idle,
    Fire,
}

public interface IAnimationController {

    void Start(Animations animation);
    void Stop(Animations animation);

}
