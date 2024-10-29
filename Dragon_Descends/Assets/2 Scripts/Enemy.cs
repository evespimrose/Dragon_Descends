using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    private float moveSpeed = 0.7f;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);
    }
}
