using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.CasualGames.SwoopGame
{
    public class Plane : MonoBehaviour
    {
        [Header("Plane Props")]

        [SerializeField] private float floatForce;

        [SerializeField] private Vector3 m_InitialPos;

        [SerializeField] private Vector3 m_PlaneInitialPos;

        private Rigidbody m_RigidBody;

        [SerializeField] private float m_Speed;

        public float PositiveRotationSpeed = 10;

        public float NegativeRotationSpeed;

        private GameObject CollidedObject;

        [SerializeField] private float m_UpBound;

        [Header("PlatformPoolProps")]

        public List<GameObject> _PlatformsPool;

        private Queue<GameObject> _Platforms = new Queue<GameObject>();

        [SerializeField] private GameObject m_ObjectFromPool;

        private bool m_TriggeredOnce = false;

        [SerializeField] private float m_DistanceBetweenPlatform;

        [Header("Plane crash Props")]

        private bool m_PlaneCrashed;

        public static Action _PlaneCrashed;

        public GameObject target;

        void Start()
        {
            Initialize();
        }

        private void OnTriggerEnter(Collider other)
        {
            Gem gem;
            if (other.GetComponent<Gem>() != null)
            {
                gem = other.GetComponent<Gem>();
                StartCoroutine(gem.KillGem());
            }
   
            if (other.tag == "Crash")
            {
                m_PlaneCrashed = true;
                _Platforms.Clear();
                _PlaneCrashed?.Invoke();
            }

            if (other.tag == "PlatformTrigger" && !m_TriggeredOnce && !m_PlaneCrashed)
            {
                CollidedObject = other.gameObject;

                m_ObjectFromPool = _Platforms.Dequeue();
                m_ObjectFromPool.transform.position = new Vector3(m_InitialPos.x, other.transform.position.y, other.transform.position.z + m_DistanceBetweenPlatform);
                m_ObjectFromPool.transform.gameObject.SetActive(true);
                m_TriggeredOnce = true;
            }

        }
        void Update()
        {
            if (transform.position.y > m_UpBound)
            {
                this.transform.position = new Vector3(this.transform.position.x, m_UpBound, this.transform.position.z);
            }
            MovePlane();
        }

        private void MovePlane()
        {
            if (m_PlaneCrashed)
            {
                m_RigidBody.useGravity = false;
                return;
            }

            if (CollidedObject && Vector3.Distance(CollidedObject.transform.position, this.transform.position) > 464.5f)
            {
                CollidedObject.transform.parent.gameObject.SetActive(false);
                _Platforms.Enqueue(CollidedObject.transform.parent.gameObject);
                CollidedObject = null;
                m_TriggeredOnce = false;
            }

            transform.Translate(transform.InverseTransformDirection(transform.forward) * Time.deltaTime * m_Speed);
            if (Input.GetMouseButton(0))
            {
                //m_RigidBody.useGravity = false;
                transform.Translate(transform.InverseTransformDirection(transform.up) * Time.deltaTime * floatForce);
                transform.Rotate(Vector3.right * Time.deltaTime * -PositiveRotationSpeed);
                //MovePlaneUpward();
            }
            else
            {
                //m_RigidBody.useGravity = true;
                Quaternion targetRotation = Quaternion.Euler(90, 0, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, NegativeRotationSpeed * Time.deltaTime);
                transform.Translate(transform.InverseTransformDirection(-transform.up) * Time.deltaTime * floatForce);
            }
        }

        //to initialize and reset plane
        private void Initialize()
        {
            if (GetComponent<Rigidbody>() != null)
            {
                m_RigidBody = GetComponent<Rigidbody>();
            }

            m_PlaneCrashed = false;

            this.transform.position = m_PlaneInitialPos;
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


    }
}

