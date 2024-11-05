using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : Skill
{
    private BeamProjectile activeBeam;

    protected override void Start()
    {
        fireRate = 7f;
        StartCoroutine(CannonAim());
        StartCoroutine(FireBeam());
    }

    private IEnumerator FireBeam()
    {
        while (true)
        {

            yield return new WaitUntil(() => SeekClosestEnemy() != null);
            

            Transform closestEnemy = SeekClosestEnemy();
            if (closestEnemy != null)
            {
                Vector2 directionToEnemy = (closestEnemy.position - transform.position).normalized;

                activeBeam = Instantiate(Resources.Load<BeamProjectile>("BeamProjectile"), transform.position, Quaternion.identity);
                activeBeam.Initialize(directionToEnemy, transform);
                activeBeam.transform.up = directionToEnemy;
                activeBeam.duration = 3f;
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

    protected override void FireProjectileAtClosestEnemy()
    {
    }
}
