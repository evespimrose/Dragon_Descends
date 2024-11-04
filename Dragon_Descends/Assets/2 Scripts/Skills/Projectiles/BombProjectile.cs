using System.Collections;
using UnityEngine;

public class BombProjectile : Projectile
{
    [SerializeField] private float explosionRadius = 1.5f;
    private bool hasExploded = false;
    private Animator animator;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(OnMyDestroy(false, 5f));
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

        while (elapsedTime < 2f)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / 2f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        DamageEnemiesInRadius();
        StartCoroutine(OnMyDestroy(true, 2f));
    }

    private void DamageEnemiesInRadius()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    }

    public override IEnumerator OnMyDestroy(float duration)
    {
        if (animator != null)
        {
            animator.SetTrigger("OnDestroy");
        }

        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    public IEnumerator OnMyDestroy(bool flag, float duration)
    {
        if (flag && animator != null)
        {
            animator.SetTrigger("OnDestroy");
        }

        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
