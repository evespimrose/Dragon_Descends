using System.Collections;
using UnityEngine;

public class Bomber : Skill
{
    [SerializeField] private BombProjectile bombProjectilePrefab;

    protected override IEnumerator AutoFire()
    {
        isFiring = true;
        while (true)
        {
            yield return new WaitUntil(() => SeekClosestEnemy() != null);
            yield return new WaitForSeconds(fireRate);

            FireBombProjectileAtClosestEnemy();
        }
    }

    private void FireBombProjectileAtClosestEnemy()
    {
        Transform closestEnemy = SeekClosestEnemy();
        if (closestEnemy != null)
        {
            // Instantiate and configure the BombProjectile
            BombProjectile bombProjectile = Instantiate(Resources.Load<BombProjectile>("BombProjectile"), transform.position, Quaternion.identity);
            bombProjectile.transform.SetLocalPositionAndRotation(transform.position, transform.rotation);

            bombProjectile.transform.up = closestEnemy.position - transform.position;
            bombProjectile.transform.localScale *= projectileSize;
            bombProjectile.duration = 5f;  // Set custom duration if needed
            bombProjectile.SetStats(CharacterManager.Instance.player.damage * damageMultiplier, 1f);  // Apply skill's damage multiplier
        }
    }
}
