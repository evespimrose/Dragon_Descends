using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    protected Transform SeekClosestEnemy()
    {
        List<Enemy> enemies = CharacterManager.Instance.enemies;
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < enemies.Count; ++i)
        {
            float currentDistance = Vector2.Distance(transform.position, enemies[i].transform.position);

            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closestEnemy = enemies[i].transform;
            }
        }

        return closestEnemy;
    }
}
