using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //So you can use SceneManager

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }


    private LoopReset loopReset = default;
    [SerializeField] float resetDelay = 2.0f;
    public int numClones;
    public float timePerClone;
    public event Action LevelEndEvent;
    private bool isRunning = true;

    private float time;     // Time since level has started

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
        
        // make sure you manually destroy this and the loopReset before changing level.
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        time = timePerClone;
        loopReset = GetComponent<LoopReset>();
    }

    // Getters
    public float getTimeRemaining()
    {
        return time;
    }

    public List<Transform> getPlayerTransforms()
    {
        return loopReset.GetPlayerTransforms();
    }

    public int getNumRemainingClones()
    {
        return numClones - loopReset.NumClones;
    }

    public GameObject getCurPlayer()
    {
        return loopReset.CurrentPlayer;
    }

    public bool getIsRunning()
    {
        return isRunning;
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// Made it an IEnumerator so that players don't freeze in midair
    /// </summary>
    /// <returns></returns>
    public IEnumerator ResetLevel()
    {
        yield return new WaitForSeconds(resetDelay);
        resetLoop();
    }

    // Go in some game/scene manager later
    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void resetLoop()
    {
        loopReset.ResetLoop();
        time = timePerClone;

        // End level if we run out of clones
        if (loopReset.NumClones > numClones)
            endLevel();
    }

    private void endLevel()
    {
        LevelEndEvent?.Invoke();
        isRunning = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Debug.Log("Level Ended");
    }
}
