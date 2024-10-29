using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonManager<Player>
{
    List<BodyPart> parts;

    public List<BodyPart> allParts => parts;

    public Transform Target;

    public float playerMoveSpeed = 1f;

    public int hp = 1;

    protected override void Awake()
    {
        base.Awake();
        Target.position = transform.position;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 dir = (Target.position - transform.position).normalized;
        transform.Translate(dir * playerMoveSpeed * Time.deltaTime);
    }
}
