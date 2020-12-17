using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //So you can use SceneManager

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }


    [SerializeField] LoopReset loopReset;
    public int numClones;
    public float timePerClone;
    public event Action levelEndEvent;
    private bool isRunning = true;

    private float time;     // Time since level has started

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    private void Start()
    {
        time = timePerClone;
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
        return numClones - loopReset.GetNumClones();
    }

    public GameObject getCurPlayer()
    {
        return loopReset.getCurPlayer();
    }

    public bool getIsRunning()
    {
        return isRunning;
    }

    private void Update()
    {
        if (!isRunning)
            return;
        time -= Time.deltaTime;
        if (time <= 0)
        {
            resetLoop();
        }
    }

    public void onPlayerDeath()
    {
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
        if (loopReset.GetNumClones() > numClones)
            endLevel();
    }

    private void endLevel()
    {
        levelEndEvent?.Invoke();
        isRunning = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Debug.Log("Level Ended");
    }
}
