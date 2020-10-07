using Assets.Game.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private List<ReplayState> replays;
    private GameObject currentPlayer;

    void Start()
    {
        replays = new List<ReplayState>();
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
        currentPlayer.GetComponent<PropertyReplayer>().OnLoopReset();
        SpawnPlayer();
    }

    public void AddToReplays(ReplayState replay)
    {
        replays.Add(replay);
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
