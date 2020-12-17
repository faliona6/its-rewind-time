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
        curPlayerDeathEvent += () => { LevelManager.Instance.onPlayerDeath(); };
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
        if (curHealth <= 0)
        {
            if (CompareTag("Player")) // If curPlayer is active
            {
                curPlayerDeathEvent?.Invoke();
            }
        }
    }

}
