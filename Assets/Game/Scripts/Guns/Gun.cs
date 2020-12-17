using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    protected Transform camOrientation;

    [SerializeField] protected ParticleSystem muzzleFlash;
    [SerializeField] protected Animator gunAnimator;
    [SerializeField] protected GameObject impactEffect; // TODO: Impact Effect based on material

    public LayerMask hitLayerMask;

    public Transform CamOrientation
    {
        get { return camOrientation; }
        set => camOrientation = value;
    }
    public ParticleSystem MuzzleFlash
    {
        get { return muzzleFlash; }
    }
    public Animator GunAnimator
    {
        get { return gunAnimator; }
    }

    public abstract void Shoot();

    protected void ShootBullet(Vector3 startPos, Vector3 dir, int damage)
    {
        startPos += dir.normalized * 0.1f;
        RaycastHit hit;
        // Check if anything is hit, shooting ray from forward vector of camera
        if (Physics.Raycast(startPos, dir, out hit, 1000f, ~hitLayerMask))
        {
            GameObject iEffect = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(iEffect, 2f);

            if (hit.transform.CompareTag("Enemy"))
            {
                Enemy e = hit.transform.GetComponent<Enemy>();
                e.incrimentHealth(-damage);
            }
            if (hit.transform.CompareTag("Player") || hit.transform.CompareTag("Clone"))
            {
                PlayerHealth playerHealth = hit.transform.GetComponent<PlayerHealth>();
                Debug.Log("hitting player");
                playerHealth.changeHealth(-damage);
            }
        }
    }
}
