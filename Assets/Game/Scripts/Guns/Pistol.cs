using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    public override void Shoot()
    {
        Debug.Log("Pistol shooting");
        ShootBullet(camOrientation.position, camOrientation.forward, 4);
    }
}
