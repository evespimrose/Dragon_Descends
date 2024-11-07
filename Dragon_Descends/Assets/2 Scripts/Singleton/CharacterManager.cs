using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : SingletonManager<CharacterManager>
{
    public Player player;

    public List<Enemy> enemies;
    public List<Bush> Bushes;

    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Bush bushPrefab;

    private float spawnDelay = 0.5f;
    private int enemySpawnCount = 10;

    Vector2 minMaxDist = new(40f, 46f);

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
        SpawnBushes();
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // 몬스터 스폰수만큼 생성
            for (int i = 0; i < enemySpawnCount; i++)
            {
                // 생성 좌표 탐색
                Vector2 ranPos = Random.insideUnitCircle;
                Vector2 spawnPos = (ranPos * (minMaxDist.y - minMaxDist.x)) + (ranPos.normalized * minMaxDist.x);

                bool isPositionValid = false;
                int attemptCount = 0;
                while (!isPositionValid && attemptCount < 10)
                {
                    isPositionValid = true;
                    // 맵 바깥에선 생성 불가
                    if (spawnPos.x < Map.Instance.minX || spawnPos.x > Map.Instance.maxX || spawnPos.y < Map.Instance.minY || spawnPos.y > Map.Instance.maxY)
                    {
                        spawnPos = (ranPos * (minMaxDist.y - minMaxDist.x)) + (ranPos.normalized * minMaxDist.x);
                        attemptCount++;
                        continue;
                    }
                    // 몬스터 주위엔 생성 불가
                    for (int j = 0; j < enemies.Count; j++)
                    {
                        float distance = Vector2.Distance(spawnPos + (Vector2)player.transform.position, enemies[j].transform.position);
                        if (distance < 4.3f)
                        {
                            Vector2 direction = (spawnPos - (Vector2)enemies[j].transform.position).normalized;
                            spawnPos = (Vector2)enemies[j].transform.position + direction * 4.3f;
                            isPositionValid = false;
                            break;
                        }
                    }
                    attemptCount++;
                }

                if (!isPositionValid)
                    continue;
                // 몬스터 생성
                Enemy newEnemy = Instantiate(enemyPrefab, (Vector2)player.transform.position + spawnPos, Quaternion.Euler(0, 0, -90));
                enemies.Add(newEnemy);
                newEnemy.GetComponent<Enemy>().OnDestroyed += () => enemies.Remove(newEnemy);
            }
            // 다음 생성 주기까지 대기
            yield return new WaitForSeconds(spawnDelay);
        }
    }


    private void SpawnBushes()
    {
        while (Bushes.Count < 500)
        {
            Vector2 spawnPos = new Vector2(Random.Range(Map.Instance.minX, Map.Instance.maxX), Random.Range(Map.Instance.minY, Map.Instance.maxY));

            bool isPositionValid = true;

            foreach (Bush bush in Bushes)
            {
                if (Vector2.Distance(spawnPos, bush.transform.position) < 5f)
                {
                    isPositionValid = false;
                    break;
                }
            }

            if (isPositionValid)
            {
                Bush newBush = Instantiate(Resources.Load<Bush>("Bush"), spawnPos, Quaternion.identity);
                Bushes.Add(newBush);
            }
        }
    }

    public void SetWave(float sd, int esc)
    {
        spawnDelay = sd;
        enemySpawnCount = esc;
    }
}
