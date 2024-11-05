using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamProjectile : Projectile
{
    private Vector2 fixedDirection;
    private HashSet<Enemy> enemiesInContact = new HashSet<Enemy>();
    private float damagePerFrame = 0.1f;
    private Transform sourceSkill;

    // Initialize the beam with the initial direction and Skill reference
    public void Initialize(Vector2 direction, Transform skill)
    {
        fixedDirection = direction.normalized;
        sourceSkill = skill;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(DamageEnemiesOverTime());
    }

    protected override void Update()
    {
        if (sourceSkill != null)
        {
            transform.position = sourceSkill.position;
        }
        transform.Translate(fixedDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null && !enemiesInContact.Contains(enemy))
            {
                enemiesInContact.Add(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null && enemiesInContact.Contains(enemy))
            {
                enemiesInContact.Remove(enemy);
            }
        }
    }

    private IEnumerator DamageEnemiesOverTime()
    {
        while (true)
        {
            yield return null;

            var enemiesSnapshot = new List<Enemy>(enemiesInContact);

            for (int i = 0; i < enemiesSnapshot.Count; i++)
            {
                if (enemiesInContact.Contains(enemiesSnapshot[i]))
                {
                    enemiesSnapshot[i].TakeDamage(damagePerFrame);
                }
            }
        }
    }

}
