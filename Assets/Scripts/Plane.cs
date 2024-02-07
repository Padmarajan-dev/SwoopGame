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

        [SerializeField] private float m_LowerBound;

        [Header("PlatformPoolProps")]

        public List<GameObject> _PlatformsPool;

        private Queue<GameObject> _Platforms = new Queue<GameObject>();

        [SerializeField] private GameObject m_ObjectFromPool;

        private bool m_TriggeredOnce = false;

        [SerializeField] private float m_DistanceBetweenPlatform;

        [Header("Plane crash Props")]

        private bool m_PlaneCrashed;

        public static Action _PlaneCrashedAction;

        [Header("Plane fuel Props")]

        [SerializeField] private float m_MaxFuel;

        [SerializeField] private float m_Fuel;

        private float m_CurrentFuel;

        private bool _FuelBlocker = false;

        [Header("Player Score Props")]

        private int CurrentScore;

        public static Action<int> _UpdateScore;

        void Start()
        {
            Initialize();
        }

        private void OnTriggerEnter(Collider other)
        {
            //while hitting on Obstacles
            Gem gem;
            if (other.GetComponent<Gem>() != null)
            {
                gem = other.GetComponent<Gem>();
                StartCoroutine(gem.KillGem());

                if (gem._Data._ObjectName == "Cloud")
                {
                    _FuelBlocker = true;
                    if (m_CurrentFuel > 0)
                    {
                        m_CurrentFuel -= gem._Data._Fuel;
                        Fuel.Instance.SetFuelValue(false, m_CurrentFuel);
                        _FuelBlocker = false;
                    }
                }

                if (gem._Data._ObjectName == "Fuel")
                {
                    _FuelBlocker = true;
                    m_CurrentFuel = (m_CurrentFuel + gem._Data._Fuel) >= m_MaxFuel ? m_MaxFuel : (m_CurrentFuel + gem._Data._Fuel);
                    Fuel.Instance.SetFuelValue(false, m_CurrentFuel);
                    _FuelBlocker = false;
                    AudioManager.Instance.PlayObstaclesAudio(gem._Data._ObjectName);
                }

                if (gem._Data._ObjectName == "Score" || gem._Data._ObjectName == "star")
                {
                    CurrentScore += gem._Data._Score;
                    _UpdateScore?.Invoke(CurrentScore);
                    AudioManager.Instance.PlayObstaclesAudio(gem._Data._ObjectName);
                }
            }
            //while plane crash
            if (other.tag == "Crash")
            {
                m_PlaneCrashed = true;
                _Platforms.Clear();
                AudioManager.Instance.PlayObstaclesAudio("Crash");
                _PlaneCrashedAction?.Invoke();
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

            if (transform.position.y < m_LowerBound)
            {
                m_PlaneCrashed = true;
                _Platforms.Clear();
                _PlaneCrashedAction?.Invoke();
            }
            MovePlane();
        }

        //to move plane
        private void MovePlane()
        {
            
            if (m_PlaneCrashed)
            {
                m_RigidBody.useGravity = false;
                return;
            }
            AudioManager.Instance.PlayPlaneAudio();
            if (CollidedObject && Vector3.Distance(CollidedObject.transform.position, this.transform.position) > 464.5f)
            {
                CollidedObject.transform.parent.gameObject.SetActive(false);
                _Platforms.Enqueue(CollidedObject.transform.parent.gameObject);
                CollidedObject = null;
                m_TriggeredOnce = false;
            }

            transform.Translate(transform.InverseTransformDirection(transform.forward) * Time.deltaTime * m_Speed);
            if (Input.GetMouseButton(0) && m_CurrentFuel >= 0)
            {
                transform.Translate(transform.InverseTransformDirection(transform.up) * Time.deltaTime * floatForce);
                transform.Rotate(Vector3.right * Time.deltaTime * -PositiveRotationSpeed);
                if (!_FuelBlocker)
                {
                    m_CurrentFuel -= m_Fuel * Time.deltaTime;
                    Fuel.Instance.SetFuelValue(false, m_CurrentFuel);
                }
            }
            else
            {
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
            m_CurrentFuel = m_MaxFuel;
            Fuel.Instance.SetFuelValue(true,m_MaxFuel);
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

