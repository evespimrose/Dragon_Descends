using System.Collections;
using UnityEngine;

public class Hoop : Skill
{
    [SerializeField] private HoopProjectile hoopProjectilePrefab;

    protected override IEnumerator AutoFire()
    {
        fireRate = 0.5f;
        isFiring = true;
        while (isFiring)
        {
            yield return new WaitUntil(() => SeekClosestEnemy() != null);
            yield return new WaitForSeconds(fireRate);

            FireHoopProjectileAtClosestEnemy();
        }
    }

    private void FireHoopProjectileAtClosestEnemy()
    {
        Transform closestEnemy = SeekClosestEnemy();
        if (closestEnemy != null)
        {
            HoopProjectile hoopProjectile = Instantiate(Resources.Load<HoopProjectile>("HoopProjectile"), transform.position, Quaternion.identity);
            hoopProjectile.transform.up = closestEnemy.position - transform.position;
            //hoopProjectile.transform.localScale *= 1.2f;
            hoopProjectile.SetStats(CharacterManager.Instance.player.damage * damageMultiplier, projectileSpeed);
        }
    }
}
