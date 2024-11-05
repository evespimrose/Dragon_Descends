using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected float damage = 1;

    [SerializeField] protected float moveSpeed = 5f;
    public float duration = 5f;

    protected virtual void Start()
    {
        StartCoroutine(OnMyDestroy(duration));
    }

    protected virtual void Update()
    {
        Move(Vector2.up);
    }
    public void Move(Vector2 dir)
    {
        transform.Translate((moveSpeed) * Time.deltaTime * dir);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
            StartCoroutine(OnMyDestroy());
        }
    }

    public void SetStats(float damagemul, float movemul)
    {
        damage *= damagemul;
        moveSpeed *= movemul;
    }

    public void IgnoreCollisionWith(Enemy enemy)
    {
        Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
        if (enemyCollider != null)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), enemyCollider);
        }
    }

    public virtual IEnumerator OnMyDestroy(float duration = 0f)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
