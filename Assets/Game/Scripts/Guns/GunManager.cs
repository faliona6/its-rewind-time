using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public Gun gun;
    private GameObject gunGameObject;
    public Camera fpsCam;
    private Transform gunParent;

    [Header("Starting Weapon")]
    public bool Shotgun; public bool Pistol;

    private void Start()
    {
        gunParent = transform;

        // Enable starting gun
        if (Shotgun)
            enableGun("Shotgun");
        else if (Pistol)
            enableGun("Pistol");
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire"))
        {
            Shoot();
        }
        // Pistol
        if (Input.GetButtonDown("Fire2"))
        {
            enableGun("Pistol");
        }
        // Shotgun
        if (Input.GetButtonDown("Fire3"))
        {
            enableGun("Shotgun");
        }
    }

    // Shoot function that shoots with any gun
    private void Shoot()
    {
        gun.Shoot();
        if (gun.gunAnimator.GetBool("isShooting"))
            return;
        gun.gunAnimator.SetTrigger("isShooting");
        gun.muzzleFlash.Play();
    }

    private void enableGun(string name)
    {
        gunGameObject?.SetActive(false);
        Debug.Log(name);
        gunGameObject = gunParent.Find(name).gameObject;
        gun = gunGameObject.GetComponent<Gun>();
        gun.fpsCam = fpsCam;
        gunGameObject.SetActive(true);
    }
}
