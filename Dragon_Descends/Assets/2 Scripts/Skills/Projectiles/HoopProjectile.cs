using System.Collections;
using UnityEngine;

public class HoopProjectile : Projectile
{
    [SerializeField] private int splitProjectileCount = 4;
    private bool hasHitEnemy = false;
    private Enemy initialHitEnemy;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasHitEnemy && collision.CompareTag("Enemy"))
        {
            hasHitEnemy = true;
            initialHitEnemy = collision.GetComponent<Enemy>();

            // Inflict damage to the first hit enemy
            initialHitEnemy.TakeDamage(damage);

            SpawnSplitProjectiles();

            StartCoroutine(OnMyDestroy(0f));
        }
    }

    private void SpawnSplitProjectiles()
    {
        for (int i = 0; i < splitProjectileCount; i++)
        {
            float angle = 360f / splitProjectileCount * i;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;

            Projectile splitProjectile = Instantiate(Resources.Load<Projectile>("Projectile"), transform.position, Quaternion.identity);
            splitProjectile.transform.up = direction;
            splitProjectile.transform.localScale *= 0.7f;
            splitProjectile.SetStats(damage, 1f);
            splitProjectile.IgnoreCollisionWith(initialHitEnemy);
        }
    }

}
