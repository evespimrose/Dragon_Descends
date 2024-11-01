using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float fireRate = 1f;
    protected bool isFiring = false;

    public float damageMultiplier = 1f;
    public float projectileSpeed = 5f;
    public float projectileSize = 1f;
    public int projectileCount = 1;
    public int piercingCount = 0;

    public Projectile projectilePrefab;
    public GameObject Cannon;

    private float detectionRadius = 12.9f;

    private void Start()
    {
        StartCoroutine(CannonAim());
        StartCoroutine(AutoFire());
    }

    protected virtual IEnumerator AutoFire()
    {
        isFiring = true;
        while (true)
        {
            yield return new WaitUntil(() => SeekClosestEnemy() != null);
            yield return new WaitForSeconds(fireRate);

            FireProjectileAtClosestEnemy();
        }
    }

    protected Transform SeekClosestEnemy()
    {
        List<Enemy> enemies = CharacterManager.Instance.enemies;
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < enemies.Count; ++i)
        {
            float currentDistance = Vector2.Distance(transform.position, enemies[i].transform.position);

            if (currentDistance < closestDistance && currentDistance <= detectionRadius)
            {
                closestDistance = currentDistance;
                closestEnemy = enemies[i].transform;
            }
        }

        return closestEnemy;
    }

    private void FireProjectileAtClosestEnemy()
    {
        Transform closestEnemy = SeekClosestEnemy();
        if (closestEnemy != null)
        {
            Projectile projectile = Instantiate(Resources.Load<Projectile>("Projectile"), transform.position, Quaternion.identity);
            projectile.transform.SetLocalPositionAndRotation(transform.position, transform.rotation);

            projectile.transform.up = closestEnemy.position - transform.position;
            projectile.transform.localScale *= projectileSize;

            Projectile projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.SetStats(CharacterManager.Instance.player.damage * damageMultiplier, 1f);
        }
    }

    private IEnumerator CannonAim()
    {
        while (true)
        {
            yield return new WaitUntil(() => SeekClosestEnemy() != null);
            Transform closestEnemy = SeekClosestEnemy();
        if (closestEnemy != null)
            {
                Vector2 direction = closestEnemy.position - Cannon.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Cannon.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            }
        }
    }

}
