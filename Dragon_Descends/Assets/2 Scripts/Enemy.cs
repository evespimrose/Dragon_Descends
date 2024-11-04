using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 0.7f;
    public GameObject rendererObject;
    private SpriteRenderer spriteRenderer;

    private float baseExperiencePerKill = 5f;
    private float experienceGainRate = 1f;
    public float hp = 1;
    private bool isAlive = true;

    public delegate void EventHandler();
    public event EventHandler OnDestroyed;

    public Animator animator;

    private void Start()
    {
        spriteRenderer = rendererObject.GetComponent<SpriteRenderer>();
        OnDestroyed += () => {
            if(!CharacterManager.Instance.player.isMaxLv)
                CharacterManager.Instance.player.GainExperience(Mathf.Round(baseExperiencePerKill + (GameManager.Instance.timeSinceStart / 5f * experienceGainRate * 10)) / 10f);
        };
    }

    private void Update()
    {
        SetTargetDirection();
        if(isAlive)
            Move();

        SpriteVectorflip();
    }

    private void Move()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }

    public void SetTargetDirection()
    {
        target = CharacterManager.Instance.player.transform;
    }

    private void SpriteVectorflip()
    {
        if (transform.position.x > target.position.x) spriteRenderer.flipY = true;
        else spriteRenderer.flipY = false;
    }
    public void TakeDamage(float damageAmount)
    {
        hp -= damageAmount;

        if (hp <= 0)
        {
            StartCoroutine(OnDeath());
        }
    }

    private IEnumerator OnDeath()
    {
        isAlive = false;
        print("OnDeath!");
        CharacterManager.Instance.enemies.Remove(this);
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        gameObject.GetComponent<Enemy>().enabled = false;
        //gameObject.GetComponent<Rigidbody2D>().gameObject.SetActive(false);
        animator.SetTrigger("isDead");

        yield return new WaitForSeconds(3f);

        OnDestroyed?.Invoke();
        
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //GameEnd

            //print("플레이어 죽음!");
        }
    }

    
}
