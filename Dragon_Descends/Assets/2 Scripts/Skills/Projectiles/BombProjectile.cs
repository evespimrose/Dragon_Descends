using System.Collections;
using System.Linq;
using UnityEngine;

public class BombProjectile : Projectile
{
    [SerializeField] private float explosionRadius = 1.5f;
    private bool hasExploded = false;
    private Animator animator;

    protected override void Start()
    {
        StartCoroutine(OnMyDestroy(5f));
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !hasExploded)
        {
            hasExploded = true;
            StartCoroutine(ExpandAndDamageEnemies());
        }
    }

    private IEnumerator ExpandAndDamageEnemies()
    {
        float elapsedTime = 0f;
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = new Vector3(explosionRadius, explosionRadius, 1f);
        moveSpeed = 0f;

        CircleCollider2D circleCollider = gameObject.GetComponent<CircleCollider2D>();
        float originalRadius = circleCollider.radius;

        while (elapsedTime < 2f)
        {
            // Interpolate the scale for the explosion effect
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / 2f);

            // Update the circleCollider radius proportionally to the local scale's x component
            circleCollider.radius = Mathf.Lerp(originalRadius, explosionRadius, elapsedTime / 2f);

            elapsedTime += Time.deltaTime;

            // Damage enemies within the updated radius
            DamageEnemiesInRadius();

            yield return null;
        }

        StartCoroutine(OnMyDestroy(2f));
    }

    private void DamageEnemiesInRadius()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        for (int i = 0;i< enemies.Count();++i)
        {
            if (enemies[i].CompareTag("Enemy"))
                enemies[i].GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    public override IEnumerator OnMyDestroy(float duration)
    {
        yield return new WaitForSeconds(duration);

        float elapsedTime = 0f;
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = new Vector3(0, 0, 1f);

        while (elapsedTime < 0.5f)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
