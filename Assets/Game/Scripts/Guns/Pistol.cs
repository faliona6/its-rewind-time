using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour, Gun
{
    private Camera fpsCam;
    [SerializeField]
    private ParticleSystem muzzleFlash;
    [SerializeField]
    private Animator gunAnimator;
    [SerializeField]
    private GameObject impactEffect; // TODO: Impact Effect based on material
    public LayerMask hitLayerMask;

    public Pistol(Camera fpsCam)
    {
        this.fpsCam = fpsCam;
    }

    Camera Gun.fpsCam
    {
        get { return fpsCam; }
        set => fpsCam = value;
    }
    ParticleSystem Gun.muzzleFlash
    {
        get { return muzzleFlash; }
    }
    Animator Gun.gunAnimator
    {
        get { return gunAnimator; }
    }

    public void Shoot()
    {
        Debug.Log("Pistol shooting");
        RaycastHit hit;
        // Check if anything is hit, shooting ray from forward vector of camera
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, 1000f, ~hitLayerMask))
        {
            Debug.Log(hit.transform.gameObject.name + " hit!");
            GameObject iEffect = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(iEffect, 2f);

            if (hit.transform.CompareTag("Enemy"))
            {
                Enemy e = hit.transform.GetComponent<Enemy>();
                e.incrimentHealth(-1);
            }
        }
    }
}
