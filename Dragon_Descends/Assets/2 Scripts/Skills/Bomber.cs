using System.Collections;
using UnityEngine;

public class Bomber : Skill
{
    [SerializeField] private BombProjectile bombProjectilePrefab;


    protected override void Start()
    {
        fireRate = 0.796f;
        base.Start();
    }

    protected override void FireProjectileAtClosestEnemy()
    {
        Transform closestEnemy = SeekClosestEnemy();
        if (closestEnemy != null)
        {
            BombProjectile bombProjectile = Instantiate(Resources.Load<BombProjectile>("BombProjectile"), transform.position, Quaternion.identity);
            bombProjectile.transform.SetLocalPositionAndRotation(transform.position, transform.rotation);

            bombProjectile.transform.up = closestEnemy.position - transform.position;
            bombProjectile.transform.localScale *= projectileSize;
            bombProjectile.duration = 5f;
            bombProjectile.SetStats(CharacterManager.Instance.player.damage * damageMultiplier, projectileSpeed + 3f);
        }
    }
}
