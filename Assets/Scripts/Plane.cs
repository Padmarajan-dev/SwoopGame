using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace com.CasualGames.SwoopGame
{
    public class Plane : MonoBehaviour
    {

        [SerializeField] private float floatForce;

        [SerializeField] private Vector3 m_InitialPos;
      
        private Rigidbody m_RigidBody;

        public float verticalInput = 0;

        float Adder = 0.05f;

        [SerializeField] private float m_Speed;

        [SerializeField] private float m_RotateSpeed;

        public float PositiveRotationSpeed = 10;

        [SerializeField] float currentAngle = 0.0f;

        public float NegativeRotationSpeed;

        private GameObject CollidedObject;

        public List<GameObject> _PlatformsPool;

        private Queue<GameObject> _Platforms = new Queue<GameObject>();

        [SerializeField] private GameObject m_ObjectFromPool;

        private bool m_TriggeredOnce = false;

        [SerializeField] private float m_DistanceBetweenPlatform;

        void Start()
        {
            if (GetComponent<Rigidbody>() != null)
            {
                m_RigidBody = GetComponent<Rigidbody>();
            }

            foreach (GameObject p in _PlatformsPool)
            {
                GameObject platform = Instantiate(p);
                platform.SetActive(false);
                _Platforms.Enqueue(platform);
            }
            m_ObjectFromPool = _Platforms.Dequeue();
            m_ObjectFromPool.transform.position = m_InitialPos;
            m_ObjectFromPool.transform.gameObject.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.tag == "PlatformTrigger" && !m_TriggeredOnce)
            {
                CollidedObject = other.gameObject;

                m_ObjectFromPool = _Platforms.Dequeue();
                m_ObjectFromPool.transform.position = new Vector3(other.transform.position.x+4f, other.transform.position.y, other.transform.position.z + m_DistanceBetweenPlatform);
                m_ObjectFromPool.transform.gameObject.SetActive(true);
                m_TriggeredOnce = true;
            }

        }

        //private void Update()
        //{
        //    RotateAroundHill();

        //    if (Input.GetMouseButton(0))
        //    {
        //        FlipAndMoveUp();
        //    }
        //    else
        //    {
        //        FallDown();
        //    }
        //    //movY = Input.GetAxis("Vertical");
        //}

        //// Update is called once per frame
        //void FixedUpdate()
        //{



        //}


        //void MovePlane()
        //{

        //    //Vector3 positionOnOrbit = RotateAroundTarget.transform.position; /*+ Quaternion.AngleAxis(currentAngle += m_Speed * Time.deltaTime, -RotateAroundTarget.transform.up)*/ /** radius;*/

        //    //this.transform.position = positionOnOrbit;
        //    //print("look at: " + positionOnOrbit + Vector3.forward);
        //    //transform.LookAt(positionOnOrbit + Vector3.forward);

        //    ////if (isMousePressed)
        //    ////{
        //    //    transform.Rotate(-Vector3.forward * verticalInput * Time.deltaTime * -m_RotateSpeed);
        //    ////}
        //    ///
        //    transform.RotateAround(RotateAroundTarget.transform.position, Vector3.down, 30 * Time.deltaTime);
        //    if (isMousePressed)
        //    {
        //        transform.Rotate(Vector3.right * Time.deltaTime * -PositiveRotationSpeed);
        //        transform.Translate(Vector3.up * Time.deltaTime * floatForce);
        //    }
        //    else
        //    {
        //        Quaternion targetRotation = Quaternion.Euler(90, 182.184f, 0);
        //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, NegativeRotationSpeed * Time.deltaTime);
        //        transform.Translate(-transform.up * Time.deltaTime * floatForce);
        //    }

        //    //Vector3 newPosition = transform.position;
        //    //newPosition.z = initialZPosition;
        //    //transform.position = newPosition;

        //}

        //void RotateAroundHill()
        //{
        //    transform.RotateAround(RotateAroundTarget.transform.position, Vector3.up, 30f * Time.deltaTime);
        //}

        //void FlipAndMoveUp()
        //{
        //    transform.Rotate(Vector3.right * Time.deltaTime * -PositiveRotationSpeed);
        //    transform.Translate(Vector3.up * Time.deltaTime * floatForce, Space.World);
        //}

        //void FallDown()
        //{
        //    Quaternion targetRotation = Quaternion.Euler(90, 182.184f, 0);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, NegativeRotationSpeed * Time.deltaTime);
        //    transform.Translate(-transform.up * Time.deltaTime * floatForce, Space.World);
        //}

        void Update()
        {
            if (CollidedObject && Vector3.Distance(CollidedObject.transform.position, this.transform.position) > 464.5f)
            {
                CollidedObject.transform.parent.gameObject.SetActive(false);
                _Platforms.Enqueue(CollidedObject.transform.parent.gameObject);
                CollidedObject = null;
                m_TriggeredOnce = false;
            }

            transform.Translate(transform.InverseTransformDirection(transform.forward) * Time.deltaTime * m_Speed);
            //if (Input.GetMouseButton(0))
            //{
            //    transform.Translate(transform.InverseTransformDirection(transform.up) * Time.deltaTime * floatForce);
            //    transform.Rotate(Vector3.right * Time.deltaTime * -PositiveRotationSpeed);
            //    m_RigidBody.useGravity = false;
            //}
            //else
            //{
            //    m_RigidBody.useGravity = true;
            //    Quaternion targetRotation = Quaternion.Euler(90, 0, 0);
            //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, NegativeRotationSpeed * Time.deltaTime);
            //}
        }
    }
}

