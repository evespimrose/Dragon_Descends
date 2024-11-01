using System.Collections;
using UnityEngine;

public class GameManager : SingletonManager<GameManager>
{
    public float timeSinceStart;

    private void Start()
    {
        timeSinceStart = 0f;
    }

    private void Update()
    {
        timeSinceStart = Time.time;
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
        else if (timeSinceStart > 600f)
        {
            CharacterManager.Instance.SetWave(1f, 50);
        }
    }
}
