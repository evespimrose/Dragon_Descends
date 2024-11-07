using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 0.7f;
    public GameObject rendererObject;
    private SpriteRenderer spriteRenderer;

    private float baseExperiencePerKill = 1f;
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
        if (!CharacterManager.Instance.player.IsMaxLv)
            //  + (GameManager.Instance.timeSinceStart / 5f * experienceGainRate * 20) / 10f
            CharacterManager.Instance.player.GainExperience(baseExperiencePerKill + (GameManager.Instance.timeSinceStart / 5f * experienceGainRate * 20) / 10f);     

        isAlive = false;
        CharacterManager.Instance.enemies.Remove(this);
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        gameObject.GetComponent<Enemy>().enabled = false;

        animator.SetTrigger("isDead");

        yield return new WaitForSeconds(3f);

        OnDestroyed?.Invoke();
        
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CharacterManager.Instance.player.OnDeath();
        }
    }

    
}
