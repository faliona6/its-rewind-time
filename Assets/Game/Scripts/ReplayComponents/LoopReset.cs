﻿using Assets.Game.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoopReset : MonoBehaviour
{
    // the job of this script is to reset the "player" back to its original
    // position and spawn a new character
    // player should delete its own camera on reset

    // reference to player so that it can be spawned
    [SerializeField] GameObject Player;

    // can make this a list of vector3s instead, but for now it should just offset the
    // player by the same amount each time.
    [SerializeField] Vector3 respawnOffset;
    [SerializeField] Transform lastRespawn;
    public delegate void Reset();
    public static event Reset OnResetCalls;
    private GameObject currentPlayer;

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        currentPlayer = Instantiate(Player, lastRespawn.position, gameObject.transform.rotation);
        currentPlayer.AddComponent(typeof(PropertyReplayer));
    }

    void ResetLoop()
    {
        // need some way of keeping track of all active players.
        lastRespawn.position += respawnOffset;
        // SpawnPlayer has to happen after the current recorder is set so that
        // current player points to the correct reference.
        SpawnPlayer();
        OnResetCalls?.Invoke();
    }

    private void Update()
    {
        // this is temporary code to reset the player back to its original state
        // and spawn a new character using keyboard input
        if(Input.GetKeyDown(KeyCode.P))
        {
            // Reset all characters back to their starting positions and the player back to the
            // next respawn point
            ResetLoop();
        }
    }
}