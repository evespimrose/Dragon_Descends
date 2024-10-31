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
    

    private void Start()
    {
        spriteRenderer = rendererObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        SetTargetDirection();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Projectile"))
        {
            CharacterManager.Instance.player.GainExperience(Mathf.Round(baseExperiencePerKill + (GameManager.Instance.timeSinceStart / 5f * experienceGainRate * 10)) / 10f);
        }
    }
}
