using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public GameObject prevBodyPart;
    private Rigidbody2D rb;
    private DistanceJoint2D Joint;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
        }
        rb.gravityScale = 0;

        Joint = GetComponent<DistanceJoint2D>();
        if (Joint == null)
        {
            Joint = gameObject.AddComponent<DistanceJoint2D>();
        }

        
    }
    private void Start()
    {
        if (prevBodyPart == null)
        {
            prevBodyPart = CharacterManager.Instance.player.gameObject;
        }
        }

    private void SetupSpringJoint()
    {
        if (prevBodyPart != null)
        {
            Joint.connectedBody = prevBodyPart.GetComponent<Rigidbody2D>();

            Joint.distance = 0.5f;
        }
        else
        {
            Joint.connectedBody = CharacterManager.Instance.player.GetComponent<Rigidbody2D>();
            Joint.distance = 0.5f;
        }
    }

    private void Update()
    {
        if (prevBodyPart == null)
        {
            Joint.connectedBody = CharacterManager.Instance.player.GetComponent<Rigidbody2D>();
        }
    }
}
