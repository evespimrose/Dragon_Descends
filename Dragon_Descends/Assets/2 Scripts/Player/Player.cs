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

    private bool isAlive = true;

    public SpriteRenderer Head;
    public SpriteRenderer DeadHead;

    public float ExpRequired
    {
        get
        {
            return level switch
            {
                0 => 100,
                1 => 500,
                2 => 1500,
                _ => 0f // assuming levels above 3 are not used
            };
        }
    }

    public float ExpAmount
    {
        get
        {
            if (IsMaxLv)
                return 1f;

            float currentLevelRequirement = Requiredexp(level, 0);
            float nextLevelRequirement = Requiredexp(level + 1, 0);
            return (experience - currentLevelRequirement) / (nextLevelRequirement - currentLevelRequirement);
        }
    }

    private int Requiredexp(int lv, int cumulativeExp)
    {
        return lv switch
        {
            0 => cumulativeExp,
            1 => Requiredexp(lv - 1, cumulativeExp + 100),
            2 => Requiredexp(lv - 1, cumulativeExp + 500),
            3 => Requiredexp(lv - 1, cumulativeExp + 1500),
            _ => cumulativeExp
        };
    }

    public bool IsMaxLv => level >= 3;

    private void Awake()
    {
        stat = GetComponent<PlayerStat>();
        Target = transform.position;
        UpdateStats();

        Head.gameObject.SetActive(true);
        DeadHead.gameObject.SetActive(false);
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
        if (isAlive)
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
        }
        else
        {
            Target = transform.position;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            LevelUp();
        }
    }

    private void Move()
    {
        Vector2 currentPosition = transform.position;
        if ((Vector2)Target != currentPosition && isAlive)
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

    public void BindBodyParts()
    {
        BodyPart part = new BodyPart();
        for (int i = 0; i < parts.Count; i++)
        {
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
                Skill skill = parts[i].AddComponent<Skill>();
                skill.Cannon = body.cannon;
            }
            parts[i].SetupJoint();
        }
        tail.ChangeChaseBodyPart(parts[parts.Count - 1].gameObject);
    }

    private void UpdateUI()
    {
        if (IsMaxLv)
        {
            UIManager.Instance.expImage.fillAmount = 1f;
            UIManager.Instance.nextLeveltext.text = "MaxLevel";
        }
        else
            UIManager.Instance.expImage.fillAmount = ExpAmount;
    }

    public void GainExperience(float exp)
    {
        if (IsMaxLv)
        {
            experience = Requiredexp(level, 0);
            return;
        }

        if (isAlive)
            experience += exp;

        if (!IsMaxLv && experience >= Requiredexp(level + 1, 0) && isAlive)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        if (IsMaxLv && !isAlive) return;

        level++;
        UIManager.Instance.currLeveltext.text = "Lv." + level.ToString();
        UIManager.Instance.nextLeveltext.text = level < 3 ? "Lv." + (level + 1).ToString() : "MaxLevel";

        UIManager.Instance.GameSkillLevelUpPauseResume();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile") && collision.TryGetComponent<BombProjectile>(out BombProjectile bombProjectile))
        {
            ParticleSystem particleSystem = bombProjectile.GetComponent<ParticleSystem>();

            if (particleSystem != null)
            {
                var main = particleSystem.main;
                Color startColor = main.startColor.color;
                startColor.a = 0.5f;
                main.startColor = startColor;
            }
        }
        else if (collision.CompareTag("Bush"))
        {
            SpriteRenderer spriteRenderer = collision.GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Color spriteColor = spriteRenderer.color;
                spriteColor.a = 0.3f;
                spriteRenderer.color = spriteColor;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile") && collision.TryGetComponent<BombProjectile>(out BombProjectile bombProjectile))
        {
            ParticleSystem particleSystem = bombProjectile.GetComponent<ParticleSystem>();

            if (particleSystem != null)
            {
                var main = particleSystem.main;
                Color startColor = main.startColor.color;
                startColor.a = 1f;
                main.startColor = startColor;
            }
        }
        else if (collision.CompareTag("Bush"))
        {
            SpriteRenderer spriteRenderer = collision.GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Color spriteColor = spriteRenderer.color;
                spriteColor.a = 1f;
                spriteRenderer.color = spriteColor;
            }
        }
    }

    public void OnDeath()
    {
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        isAlive = false;

        foreach (BodyPart part in parts)
        {
            if (part.TryGetComponent<Skill>(out Skill skill))
            {
                skill.isFiring = false;
                skill.StopFire();
            }
        }

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        Head.gameObject.SetActive(false);
        DeadHead.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        UIManager.Instance.GameOverPauseResume();
    }

    public void ResetPlayer()
    {
        isAlive = true;

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        level = 0;
        experience = 0;

        UIManager.Instance.currLeveltext.text = "Lv." + level.ToString();
        UIManager.Instance.nextLeveltext.text = "Lv." + (level + 1).ToString();
        UIManager.Instance.expImage.fillAmount = 0;

        for (int i = parts.Count - 1; i >= 1; i--)
        {
            Destroy(parts[i].gameObject);
            parts.RemoveAt(i);
        }

        tail.ChangeChaseBodyPart(parts[0].gameObject);
        parts[0].transform.position = transform.position;
        tail.transform.position = transform.position;

        if (parts[0].TryGetComponent<Skill>(out Skill skill))
        {
            print("다시 발사");
            skill.isFiring = true;
            skill.StartFire();
        }

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true;
        }

        Head.gameObject.SetActive(true);
        DeadHead.gameObject.SetActive(false);
    }
}
