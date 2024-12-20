using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonManager<GameManager>
{
    public float timeSinceStart;
    [SerializeField] private List<Projectile> projectiles = new List<Projectile>();
    [SerializeField] private List<Skill> skills;
    public List<Projectile> projs => projectiles;


    private Camera mainCamera;

    private void Start()
    {
        timeSinceStart = 0f;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        timeSinceStart += Time.deltaTime;
        WaveController();
    }

    private void WaveController()
    {
        // 0 ~ 2분 동안 5초당 10마리 생성
        if (timeSinceStart <= 120f)
        {
            CharacterManager.Instance.SetWave(5f, 10);
        }
        // 2 ~ 5분 동안 5초당 20마리 생성
        else if (timeSinceStart > 120f && timeSinceStart <= 300f)
        {
            CharacterManager.Instance.SetWave(5f, 20);
        }
        // 5 ~ 10분 동안 1초당 20마리 생성
        else if (timeSinceStart > 300f && timeSinceStart <= 600f)
        {
            CharacterManager.Instance.SetWave(1f, 20);
        }
        // 10분 이후 1초당 50마리 생성
        else if (timeSinceStart > 600f && timeSinceStart <= 900f)
        {
            CharacterManager.Instance.SetWave(1f, 50);
        }
        else if(timeSinceStart > 900f)
        {
            StartCoroutine(InitiateClear());
        }
    }
    private IEnumerator InitiateClear()
    {
        Player p = CharacterManager.Instance.player;

        foreach (BodyPart part in p.parts)
        {
            if (part.TryGetComponent<Skill>(out Skill skill))
            {
                skill.isFiring = false;
            }
        }

        if (p.TryGetComponent<Collider2D>(out var collider))
        {
            collider.enabled = false;
        }
        yield return new WaitForSeconds(2f);
        UIManager.Instance.GameOverPauseResume(false);
    }
    
}
