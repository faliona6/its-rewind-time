using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// PlayerHat is the script that allows players to jump on each other's heads properly
/// </summary>
public class PlayerHat : MonoBehaviour
{
    // this is a platform on the head to make jumping on top of the head easier without interrupting gameplay.

    private Collider hatCollider;

    private void Start()
    {
        hatCollider = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        if (LoopReset.Instance.CurrentPlayer.GetComponentInChildren<PlayerMovement>().FeetYPos <=
            hatCollider.transform.position.y)
        {
            hatCollider.isTrigger = true;
        }
        else
        {
            hatCollider.isTrigger = false;
        }
    }
}
