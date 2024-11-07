using System.Collections;
using UnityEngine;

public class Crescent : Skill
{
    protected override void Start()
    {
        fireRate = 0.3f;
        base.Start();
    }
    protected override void FireProjectileAtClosestEnemy()
    {
        Transform closestEnemy = SeekClosestEnemy();
        if (closestEnemy != null)
        {
            CrescentProjectile projectile = Instantiate(Resources.Load<CrescentProjectile>("CrescentProjectile"), transform.position, Quaternion.identity);
            projectile.transform.SetLocalPositionAndRotation(transform.position, transform.rotation);

            projectile.transform.up = closestEnemy.position - transform.position;
            projectile.transform.localScale *= projectileSize;
            projectile.duration = 5f;
            projectile.SetStats(CharacterManager.Instance.player.damage * damageMultiplier, projectileSpeed + 5f);
        }
    }
}
