using System;
using Settings;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private const string IS_WALKING = "IsWalking";
        
        private Animator _animator;
    
        [SerializeField] private GameInput input;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private NavMeshAgent agent;

        [Header("Movement")]
        [SerializeField] private ParticleSystem clickEffect;
        [SerializeField] private LayerMask clickableLayers;

        private float _lookRotationSpeed = 8f;
        private static readonly int IsWalking = Animator.StringToHash(IS_WALKING);
        
        private readonly Scenes _scenes = new Scenes();

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            AssignInputs();
        }

        void AssignInputs()
        {
            input.GetInput().Player.Move.performed += ctx => ClickToMove();
        }

        void ClickToMove()
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayers)) 
            {
                agent.destination = hit.point;
                if (clickEffect != null)
                {
                    Instantiate(clickEffect, hit.point + new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
                }
            }
        }

        void Update() 
        {
            if (SceneManager.GetActiveScene().name == _scenes.HUB_SCENE ||
                SceneManager.GetActiveScene().name == _scenes.DUNGEON_SCENE)
            {
                agent.enabled = true;
            }
            FaceTarget();
            SetAnimations();
        }

        void FaceTarget()
        {
            Vector3 direction = (agent.destination - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _lookRotationSpeed);
            }
        }

        void SetAnimations()
        {
            if (agent.velocity == Vector3.zero)
            {
                _animator.SetBool(IsWalking, false);
            }
            else
            {
                _animator.SetBool(IsWalking, true);
            }
        }
    }
}