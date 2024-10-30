using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    public float fireRate;
    public float projectileSpeed;

    private void Update()
    {
        projectileSpeed = moveSpeed * 5;
    }
}
