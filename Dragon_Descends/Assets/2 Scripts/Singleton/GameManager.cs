using System.Collections;
using System.Collections.Generic;
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
    }
}
