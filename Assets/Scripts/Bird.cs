using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.CasualGames.SwoopGame
{
    public class Bird : MonoBehaviour
    {
        [SerializeField] private float m_Speed;

        [SerializeField] private float m_JumpSpeed;

        private Rigidbody m_Rigidbody;

        public float verticalInput = 0;
        private float movX, movY = 1;
        float Adder = 0.05f;

        private void Start()
        {
            if (GetComponent<Rigidbody>() != null)
            {
                m_Rigidbody = GetComponent<Rigidbody>();
            }
        }
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                m_Rigidbody.useGravity = false;
                verticalInput = Mathf.Lerp(verticalInput, Mathf.Clamp((verticalInput + Adder), -1f, 1f), 1);
            }
            else
            {
                m_Rigidbody.useGravity = true;
                verticalInput = Mathf.Lerp(verticalInput, Mathf.Clamp((verticalInput - Adder), -1f, 1f), 1);
            }

            movY = verticalInput;
        }
        // Update is called once per frame
        void FixedUpdate()
        {
            Jump();
            MoveBird();
        }

        public void MoveBird()
        {
            m_Rigidbody.velocity = new Vector3(m_Speed, -m_Rigidbody.velocity.x);
        }

        public void Jump()
        {
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.y, m_JumpSpeed * movY);
        }
    }
}
