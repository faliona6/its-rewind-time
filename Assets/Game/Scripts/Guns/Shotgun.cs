using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    public override void Shoot()
    {
        Debug.Log("Shotgun shooting");
        ShootBullet(camOrientation.position, camOrientation.forward, 1);
        ShootBullet(camOrientation.position, camOrientation.forward, 1);

    }
}
