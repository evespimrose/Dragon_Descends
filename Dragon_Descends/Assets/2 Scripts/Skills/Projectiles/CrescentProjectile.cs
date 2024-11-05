using System.Collections;
using UnityEngine;

public class CrescentProjectile : Projectile
{
    private int hitCount = 3;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
            hitCount--;

            if (hitCount <= 0)
            {
                StartCoroutine(OnMyDestroy());
            }
        }
    }
}

