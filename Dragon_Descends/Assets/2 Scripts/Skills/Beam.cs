using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : Skill
{
    private BeamProjectile activeBeam;

    protected override void Start()
    {
        StartCoroutine(CannonAim());
        StartCoroutine(AutoFire());
    }

    protected override IEnumerator AutoFire()
    {
        fireRate = 0.909f;
        isFiring = true;
        while (true)
        {
            yield return new WaitUntil(() => isFiring);
            yield return new WaitUntil(() => SeekClosestEnemy() != null);
            

            Transform closestEnemy = SeekClosestEnemy();
            if (closestEnemy != null)
            {
                Vector2 directionToEnemy = (closestEnemy.position - transform.position).normalized;

                activeBeam = Instantiate(Resources.Load<BeamProjectile>("BeamProjectile"), transform.position, Quaternion.identity);
                activeBeam.Initialize(directionToEnemy, transform);
                activeBeam.transform.up = directionToEnemy;
                activeBeam.duration = 0.5f;
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

    protected override void FireProjectileAtClosestEnemy()
    {
    }
}
