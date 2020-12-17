using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    private Gun gun;
    private GameObject gunGameObject;
    public Transform camOrientation;
    private Transform gunParent;

    public enum CurrentGun
    {
        Pistol,
        Shotgun
    }
    public CurrentGun startingGun;

    private void Start()
    {
        gunParent = transform;

        switch(startingGun)
        {
            case CurrentGun.Shotgun:
                enableGun("Shotgun");
                break;
            case CurrentGun.Pistol:
                enableGun("Pistol");
                break;
        }
    }

    // Shoot function that shoots with any gun
    public void Shoot()
    {
        // Check if currently shooting from gun Animator
        if (gun.GunAnimator.GetBool("isShooting"))
            return;
        // Call shoot from gun interface
        gun.Shoot();
        // Set gun animation
        gun.GunAnimator.SetTrigger("isShooting");
        gun.MuzzleFlash.Play();
    }

    private void enableGun(string name)
    {
        // Deactivate previous gun
        gunGameObject?.SetActive(false);
        gunGameObject = gunParent.Find(name).gameObject;

        // Activate current gun
        gun = gunGameObject.GetComponent<Gun>();
        gun.CamOrientation = camOrientation;
        gunGameObject.SetActive(true);
    }
}
