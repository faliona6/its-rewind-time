using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Enemy
{
    void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (enemyNav.getState() == EnemyNav.State.ChaseTarget)
        {
            gunManager.Shoot();
        }
    }
}
