using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : SingletonManager<CharacterManager>
{
    public Player player;

    public List<Enemy> enemies;

    [SerializeField] private Enemy enemyPrefab;

    private float spawnDelay = 0.5f;
    private int enemySpawnCount = 10;

    Vector2 minMaxDist = new Vector2(15, 17f);

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
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
                while (!isPositionValid && attemptCount < 10) // 최대 10번까지 위치 재설정 시도
                {
                    isPositionValid = true;
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

                Enemy newEnemy = Instantiate(enemyPrefab, (Vector2)player.transform.position + spawnPos, Quaternion.Euler(0, 0, -90));
                enemies.Add(newEnemy);
                newEnemy.GetComponent<Enemy>().OnDestroyed += () => enemies.Remove(newEnemy);
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void SetWave(float sd, int esc)
    {
        spawnDelay = sd;
        enemySpawnCount = esc;
    }
}
