using System.Collections;
using UnityEngine;

public class Hoop : Skill
{
    [SerializeField] private HoopProjectile hoopProjectilePrefab;

    protected override void Start()
    {
        fireRate = 0.5f;
        base.Start();
    }

    protected override void FireProjectileAtClosestEnemy()
    {
        Transform closestEnemy = SeekClosestEnemy();
        if (closestEnemy != null)
        {
            HoopProjectile hoopProjectile = Instantiate(Resources.Load<HoopProjectile>("HoopProjectile"), transform.position, Quaternion.identity);
            hoopProjectile.transform.up = closestEnemy.position - transform.position;
            //hoopProjectile.transform.localScale *= 1.2f;
            hoopProjectile.SetStats(CharacterManager.Instance.player.damage * damageMultiplier, projectileSpeed + 3f);
        }
    }
}
