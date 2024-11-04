using System.Collections;
using UnityEngine;

public class Hoop : Skill
{
    [SerializeField] private HoopProjectile hoopProjectilePrefab;

    protected override IEnumerator AutoFire()
    {
        isFiring = true;
        while (true)
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
            // Instantiate and set up the HoopProjectile
            HoopProjectile hoopProjectile = Instantiate(hoopProjectilePrefab, transform.position, Quaternion.identity);
            hoopProjectile.transform.up = closestEnemy.position - transform.position;
            hoopProjectile.transform.localScale *= 1.2f;
            hoopProjectile.SetStats(CharacterManager.Instance.player.damage * damageMultiplier, projectileSpeed);
        }
    }
}
