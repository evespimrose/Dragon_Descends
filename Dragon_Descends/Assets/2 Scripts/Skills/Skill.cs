using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float fireRate = 1f;
    public bool isFiring = false;

    public float damageMultiplier = 1f;
    public float projectileSpeed = 5f;
    public float projectileSize = 1f;
    public int projectileCount = 1;
    public int piercingCount = 0;

    public Projectile projectilePrefab;
    public GameObject Cannon;

    public float duration = 5f;
    private float detectionRadius = 12.9f;

    public float Offset = -0.7f;

    private Coroutine firecoroutine;
    protected virtual void Start()
    {
        StartFire(AutoFire());
        StartCoroutine(CannonAim());
        
    }

    public virtual void StopFire()
    {
        StopCoroutine(firecoroutine);
    }

    public virtual void StartFire(IEnumerator c)
    {
        firecoroutine = StartCoroutine(c);
    }

    public virtual void StartFire()
    {
        firecoroutine = StartCoroutine(AutoFire());
    }

    protected virtual IEnumerator AutoFire()
    {
        isFiring = true;
        while (isFiring)
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

    protected virtual void FireProjectileAtClosestEnemy()
    {
        Transform closestEnemy = SeekClosestEnemy();
        if (closestEnemy != null)
        {
            Projectile projectile = Instantiate(Resources.Load<Projectile>("Projectile"), transform.position, Quaternion.identity);
            projectile.transform.SetLocalPositionAndRotation(transform.position, transform.rotation);

            projectile.transform.up = closestEnemy.position - transform.position;
            projectile.transform.localScale *= projectileSize;
            projectile.duration = 5f;
            projectile.SetStats(CharacterManager.Instance.player.damage * damageMultiplier, projectileSpeed);
        }
    }

    protected IEnumerator CannonAim()
    {
        while (true)
        {
            yield return new WaitUntil(() => SeekClosestEnemy() != null);

            Transform closestEnemy = SeekClosestEnemy();
            if (closestEnemy != null)
            {
                Cannon.transform.up = closestEnemy.position - Cannon.transform.position;
            }
        }
    }



}
