using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunController : MonoBehaviour
{
    [SerializeField] GunManager gunManager;

    void Update()
    {
        if (!gameObject.CompareTag("Player") || !LevelManager.Instance.getIsRunning())
            return;

        if (Input.GetButtonDown("Fire"))
        {
            gunManager.Shoot();
        }
    }
}
