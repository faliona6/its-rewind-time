using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Gun
{

    Camera fpsCam
    {
        get; set;
    }

    ParticleSystem muzzleFlash
    {
        get;
    }

    Animator gunAnimator
    {
        get;
    }

    void Shoot();
}
