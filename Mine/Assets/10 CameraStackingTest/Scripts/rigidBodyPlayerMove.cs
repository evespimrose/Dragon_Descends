using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Myproject
{
    [RequireComponent(typeof(Rigidbody))]
    public class rigidBodyPlayerMove : MonoBehaviour
    {
        private Rigidbody rb;
        public float moveSpeed;
        public float turnSpeed;

        private Vector2 mousePosCache;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");

            Move(inputY * Time.fixedDeltaTime * moveSpeed);
            Turn(inputX * Time.fixedDeltaTime * turnSpeed);
        }
        private void Move(float speed)
        {
            rb.MovePosition(rb.position + new Vector3(0,0,speed));
        }

        private void Turn(float angle)
        {
            rb.MoveRotation(Quaternion.Euler(0,angle,0));

        }
    }
}
