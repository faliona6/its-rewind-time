using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Game.Scripts;

public class PropertyReplayer : ResetComponent
{
    // default state is set to record, as Object needs to be recorded before replayed.
    private ReplayableState state;
    // Class will currently save every physics update.
    private Transform pTransformPosition;
    // separate gameObject to keep track of rotation (along y axis)
    private Transform pTransformRotation;
    // gameObject to keep track of camera rotation (x,y,z axes)
    private Transform pCamRotation;

    protected override void Start()
    {
        base.Start();
        // create initial instance of the state.
        // Can only replay if given reference to a prerecorded object
        var playerMovement = GetComponentInChildren<PlayerMovement>();
        pTransformPosition = playerMovement.transform;
        pTransformRotation = playerMovement.orientation;
        pCamRotation = playerMovement.playerCam;
        state = new RecordState(this);
    }

    public Transform GetPositionTransform()
    {
        return pTransformPosition;
    }

    public Transform GetRotationTransform()
    {
        return pTransformRotation;
    }
    public Transform GetCamTransform()
    {
        return pCamRotation;
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
}
