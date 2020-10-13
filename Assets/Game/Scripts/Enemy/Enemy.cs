using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected int curHealth;
    public int startingHealth;

    public void Start()
    {
        curHealth = startingHealth;
    }

    public void incrimentHealth(int n)
    {
        curHealth += n;
        if (curHealth <= 0) {
            Destroy(this.gameObject);
        }
    }
}
