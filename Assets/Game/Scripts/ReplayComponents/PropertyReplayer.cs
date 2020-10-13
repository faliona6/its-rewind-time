﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Game.Scripts;

public class PropertyReplayer : ResetComponent
{
    // default state is set to record, as Object needs to be recorded before replayed.
    private ReplayableState state;

    // Controls how often the state of the character is saved.
    // Actions should be saved on the same frame they are performed for maximum accuracy.
    // A higher number results in less accurate results, but better performance.
    [SerializeField] int framesPerSave = 10;
    // players attributes to keep track of (pos and rotation for now.)
    private Transform pTransform;

    protected override void Start()
    {
        base.Start();
        // create initial instance of the state.
        // Can only replay if given reference to a prerecorded object
        pTransform = GetComponentInChildren<PlayerMovement>().gameObject.transform;
        state = new RecordState(this);
    }

    public Transform GetOrientation()
    {
        return pTransform;
    }

    public override void OnReset()
    {
        state.OnLoopReset();
    }

    public void SwitchToReplay(RecordState record)
    {
        // If there is a rigidbody on the player, it should be set to kinematic
        // player controller components should be moved or removed from this object
        // this object should stop recording actions (shoot, jump) from the player
        var replay = new ReplayState(record, this);
        state = replay;
    }

    // All properties should be loaded/stored on FixedUpdate, as this will result in consistent enough replays
    // (Fixed update is time-based)
    // move record to coroutine that returns on fixedupdate.
    void FixedUpdate()
    {
        // If this script is in record state, we need some way of subscribing to
        // the method calls from the main player script.
        state?.FixedAction();
    }

    public int GetFramesPerSave()
    {
        return framesPerSave;
    }
}