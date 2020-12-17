using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected int curHealth;
    public int startingHealth;

    [SerializeField] protected GunManager gunManager;
    protected EnemyNav enemyNav;

    public void Start()
    {
        curHealth = startingHealth;
        enemyNav = GetComponent<EnemyNav>();
    }

    public void incrimentHealth(int n) // TODO: Maybe move this to joint health script?
    {
        curHealth += n;
        if (curHealth <= 0) {
            Destroy(this.gameObject);
        }
    }
}
