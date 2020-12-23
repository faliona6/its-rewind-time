using Assets.Game.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Remoting.Messaging;
using UnityEditor;
using UnityEngine.SceneManagement;

public class LoopReset : MonoBehaviour
{
    /// <summary>
    /// Singleton class that handles the persistance of both itself and the players.
    /// </summary>

    private static LoopReset _instance;

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
    
    // this can cause errors if spawning too fast because it gets updated in the middle of the method that sets it.
    // turn it into a stack of all players that have not been updated yet :)
    private GameObject currentPlayer;
    private List<Transform> _playerTransforms = new List<Transform>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        SpawnPlayer();
        
        //
        DontDestroyOnLoad(gameObject);
    }
    
    private static LoopReset Instance
    {
        get { return _instance; }
    }

    public List<Transform> GetPlayerTransforms()
    {
        return _playerTransforms;
    }

    public int NumClones => _playerTransforms.Count;

    public GameObject CurrentPlayer => currentPlayer;

    public void SpawnPlayer()
    {
        currentPlayer = Instantiate(Player, lastRespawn.position, gameObject.transform.rotation);
        currentPlayer.AddComponent(typeof(PropertyReplayer));
        DontDestroyOnLoad(currentPlayer);
        _playerTransforms.Add(currentPlayer.transform);
    }

    public void ResetLoop()
    {
        lastRespawn.position += respawnOffset;
        // SpawnPlayer has to happen after the current recorder is set so that
        // current player points to the correct reference.
        SpawnPlayer();
        OnResetCalls?.Invoke();
        
        // set scene to itself, but reset with players maintained.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Destroys all players and the loopReset object it is called on.
    /// User will most likely want to load a completely new Scene after this is called.
    /// </summary>
    public void DestroyLoop()
    {
        // delete the references to all maintained players and this object
        // most efficient way to manage this because players are set to not destroy on scene load.
        foreach (var player in _playerTransforms)
        {
            Destroy(player.gameObject);
        }

        _instance = null;
        
        Destroy(gameObject);
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
