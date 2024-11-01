using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected float damage = 1;

    [SerializeField] private float moveSpeed = 5f;

    void Update()
    {
        Move(Vector2.up);
    }
    public void Move(Vector2 dir)
    {
        transform.Translate((moveSpeed) * Time.deltaTime * dir);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print("총알 맞음! " + collision.gameObject.name);
        if (collision.CompareTag("Enemy"))
        {
            print("총알 적한테 맞음! ");
            collision.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public void SetStats(float damagemul, float movemul)
    {
        damage *= damagemul;
        moveSpeed *= movemul;
    }
}
