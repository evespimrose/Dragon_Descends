using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<BodyPart> parts;
    public Vector2 Target;

    private PlayerStat stat;
    public float moveSpeed = 1f;
    public float hp = 1f;
    public float experience;
    public float level;

    private void Awake()
    {
        BodyPart part = new();
        for (int i = 0; i < parts.Count; i++)
        {
            if(i == 0)
            {
                part = parts[i];
            }
            else
            {
                parts[i].prevBodyPart = part.gameObject;
                part = parts[i];
            }
        }
        stat = GetComponent<PlayerStat>();
        Target = transform.position;
        UpdateStats();
    }

    private void Update()
    {
        PlayerInput();
        Move();
        UpdateStats();
    }

    private void PlayerInput()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10.0f;
            Target = Camera.main.ScreenToWorldPoint(mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Target = transform.position;
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            LevelUp();
        }
    }

    private void Move()
    {
        Vector2 currentPosition = transform.position;
        if ((Vector2)Target != currentPosition)
        {
            Vector2 direction = (Target - currentPosition).normalized;
            transform.position = Vector2.MoveTowards(currentPosition, Target, moveSpeed * Time.deltaTime);
        }
    }

    private void UpdateStats()
    {
        hp = stat.hp;
        moveSpeed = stat.moveSpeed;
    }

    public void LevelUp()
    {
        BodyPart newBody = Instantiate(Resources.Load<BodyPart>("BodyPrefab"), parts[parts.Count - 2].transform.position, Quaternion.identity);
        newBody.prevBodyPart = parts[parts.Count - 2].gameObject;

        parts.Insert(parts.Count - 1, newBody);
        parts[parts.Count - 1].prevBodyPart = newBody.gameObject; // Tail의 prevBodyPart를 새로 추가된 Body로 업데이트
    }
}