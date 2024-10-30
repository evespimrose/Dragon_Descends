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

    private void Start()
    {
        // "Renderer" 오브젝트에서 SpriteRenderer 컴포넌트 가져오기
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
}
