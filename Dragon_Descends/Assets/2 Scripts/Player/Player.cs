using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<BodyPart> parts;
    public Tail tail;

    public Vector2 Target;

    private PlayerStat stat;
    public float moveSpeed = 1f;
    public float hp = 1f;
    public float experience;
    public int level;
    public int damage = 1;

    public float expRequired { get { return level * 10; } }
    public float expAmount { get { return (experience - Requiredexp(level, 0)) / expRequired; } }

    public bool isMaxLv {  get { return level >= 3; } }

    private void Awake()
    {
        stat = GetComponent<PlayerStat>();
        Target = transform.position;
        UpdateStats();
    }
    private void Start()
    {
        BindBodyParts();
    }

    private void Update()
    {
        PlayerInput();
        Move();
        UpdateStats();
        UpdateUI();
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
        level++;
        UIManager.Instance.currLeveltext.text = "Lv." + level.ToString();
        if (level < 3)
            UIManager.Instance.nextLeveltext.text = "Lv." + (level + 1).ToString();
        else
            UIManager.Instance.nextLeveltext.text = "MaxLevel";

        UIManager.Instance.GameSkillLevelUpPauseResume();

        // newBody 부분 레벨업패널과 연동해야 함.
        
    }

    public void BindBodyParts()
    {
        BodyPart part = new();
        for (int i = 0; i < parts.Count; i++)
        {
            print(i);
            if (i == 0)
            {
                parts[i].prevBodyPart = gameObject;
                part = parts[i];
            }
            else
            {
                parts[i].prevBodyPart = part.gameObject;
                part = parts[i];
            }
            if (parts.Count == 1 && parts[i].TryGetComponent<Body>(out Body body))
            {
                Bomber skill = parts[i].AddComponent<Bomber>();
                skill.Cannon = body.cannon;
            }
            parts[i].SetupJoint();
        }
        tail.ChangeChaseBodyPart(parts[parts.Count - 1].gameObject);
    }

    private void UpdateUI()
    {
        UIManager.Instance.expImage.fillAmount = expAmount;
    }

    public void GainExperience(float exp)
    {
        experience += exp;

        if ((experience - Requiredexp(level, 0)) >= expRequired)
        {
            LevelUp();
        }
    }

    private int Requiredexp(int lv, int result)
    {
        if (lv == 0)
        {
            return result;
        }
        else
        {
            result += (lv - 1) * 10;
            return Requiredexp(lv - 1, result);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            OnDeath();
            print("플레이어 죽음!");
        }
    }

    private void OnDeath()
    {

    }
}