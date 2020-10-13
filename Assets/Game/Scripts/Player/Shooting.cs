using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField]
    private Animator gunAnimator;
    [SerializeField]
    private ParticleSystem muzzleFlash;
    public Camera fpsCam;

    public GameObject impactEffect; // TODO: we want this to be based on material we hit
    public LayerMask hitLayerMask;

    // TODO:
    // - make design nicer for implimenting different guns
    // - Bullets
    // - Reloading
    // - Hit range per gun

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire"))
        {
            Shoot();
        }
    }

    // TODO: To make it easier for clones, make gun position and forward vector parameters
    void Shoot()
    {
        if (gunAnimator.GetBool("isShooting"))
            return;
        gunAnimator.SetTrigger("isShooting");
        muzzleFlash.Play();

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
