using System;
using GameManagement;
using Mono.Cecil;
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

        [Header("Movement")] [SerializeField] private ParticleSystem clickEffect;
        [SerializeField] private LayerMask clickableLayers;

        private float _lookRotationSpeed = 8f;
        private static readonly int IsWalking = Animator.StringToHash(IS_WALKING);
        
        [Header("Attack")] [SerializeField] private ParticleSystem hitEffect;
        [SerializeField] private float attackSpeed = 1.5f;
        [SerializeField] private float attackDelay = 0.3f;
        [SerializeField] private float baseReach = 2f;
        [SerializeField] private int attackDamage = 1;

        Interactable _target;
        bool _playerBusy = false;

        private readonly Scenes _scenes = new Scenes();
        private readonly Tags _tags = new Tags();

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
                if (hit.transform.CompareTag(_tags.INTERACTABLE_TAG))
                {
                    _target = hit.transform.GetComponent<Interactable>();
                    if (clickEffect != null)
                    {
                        Instantiate(clickEffect, hit.transform.position + new Vector3(0, 0.1f, 0),
                            clickEffect.transform.rotation);
                    }
                }
                else
                {
                    _target = null;

                    agent.destination = hit.point;
                    if (clickEffect != null)
                    {
                        Instantiate(clickEffect, hit.point + new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
                    }
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
            
            FollowTarget();
            FaceTarget();
            SetAnimations();
        }

        void FollowTarget()
        {
            if (_target == null)
            {
                return;
            }

            if (Vector3.Distance(_target.transform.position, transform.position) <= baseReach)
            {
                ReachDistance();
            }
            else
            {
                agent.SetDestination(_target.transform.position);
            }
        }

        void FaceTarget()
        {
            if (agent.destination == transform.position) return;

            Vector3 facing = Vector3.zero;
            if (_target != null)
            {
                facing = _target.transform.position;
            }
            else
            {
                facing = agent.destination;
            }

            Vector3 direction = (facing - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation =
                Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _lookRotationSpeed);
        }

        void ReachDistance()
        {
            agent.SetDestination(transform.position);

            if (_playerBusy) return;

            _playerBusy = true;
            
            Debug.Log(_target.interactionType);

            switch (_target.interactionType)
            {
                case InteractableType.Enemy:
                    Invoke(nameof(SendAttack), attackDelay);
                    Invoke(nameof(ResetBusyState), attackSpeed);
                    break;
                case InteractableType.Item:
                    _target.InteractWithItem();
                    _target = null;
                    Invoke(nameof(ResetBusyState), 0.5f);
                    break;
                case InteractableType.Props:
                    _target.InteractWithProps();
                    Invoke(nameof(ResetBusyState), 0.5f);
                    break;
                case InteractableType.Gate:
                    _target.InteractWithGate();
                    Invoke(nameof(ResetBusyState), 0.5f);
                    break;
                case InteractableType.Storage:
                    _target.InteractWithStorage();
                    Invoke(nameof(ResetBusyState), 0.5f);
                    break;
            }
        }

        void SendAttack()
        {
            if (_target == null) return;

            if (_target.health <= 0)
            {
                _target = null;
                return;
            }

            Instantiate(hitEffect, _target.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            _target.TakeDamage(attackDamage);
        }

        void ResetBusyState()
        {
            _playerBusy = false;
            SetAnimations();
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