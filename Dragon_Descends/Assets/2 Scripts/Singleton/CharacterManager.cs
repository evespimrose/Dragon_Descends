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
            for (int i = 0; i < enemySpawnCount; i++)
            {
                Vector2 ranPos = Random.insideUnitCircle;
                Vector2 spawnPos = (ranPos * (minMaxDist.y - minMaxDist.x)) + (ranPos.normalized * minMaxDist.x);

                bool isPositionValid = false;
                int attemptCount = 0;
                while (!isPositionValid && attemptCount < 10)
                {
                    isPositionValid = true;

                    if (spawnPos.x < -450 || spawnPos.x > 450 || spawnPos.y < -180 || spawnPos.y > 180)
                    {
                        spawnPos = (ranPos * (minMaxDist.y - minMaxDist.x)) + (ranPos.normalized * minMaxDist.x);
                        attemptCount++;
                        continue;
                    }

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

                // 유효한 위치가 아니면 생성하지 않음
                if (!isPositionValid)
                    continue;

                // 적을 생성하고 리스트에 추가
                Enemy newEnemy = Instantiate(enemyPrefab, (Vector2)player.transform.position + spawnPos, Quaternion.Euler(0, 0, -90));
                enemies.Add(newEnemy);
                newEnemy.GetComponent<Enemy>().OnDestroyed += () => enemies.Remove(newEnemy);
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }


    private void SpawnBushes()
    {
        while (Bushes.Count < 500)
        {
            Vector2 spawnPos = new Vector2(Random.Range(-450f, 450f), Random.Range(-180f, 180f));

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
