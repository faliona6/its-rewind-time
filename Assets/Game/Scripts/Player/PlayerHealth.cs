using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int startingHealth;
    private int curHealth;

    public event Action curPlayerDeathEvent; 

    public int CurHealth
    {
        get { return curHealth; }
    }

    private void Start()
    {
        curHealth = startingHealth;
        curPlayerDeathEvent += GetComponent<PlayerMovement>().KillPlayer;
        curPlayerDeathEvent += () => { StartCoroutine(LevelManager.Instance.ResetLevel()); };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            changeHealth(-100);
        }
    }

    public void changeHealth(int deltaHealth)
    {
        curHealth += deltaHealth;
        // make this code an event that happens every time health is changed for more modular code.
        // curplayerhealthevent()
        if (curHealth <= 0)
        {
            if (CompareTag("Player")) // If curPlayer is active
            {
                curPlayerDeathEvent?.Invoke();
            }
        }
    }

}
