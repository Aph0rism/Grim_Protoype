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
    /// <summary>
    /// Gestion des controles du personnage
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        private const string IS_WALKING = "IsWalking";

        private Animator _animator;

        [SerializeField] private GameInput input;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float baseReach = 2f;

        [Header("Movement")] [SerializeField] private ParticleSystem clickEffect;
        [SerializeField] private LayerMask clickableLayers;

        private float _lookRotationSpeed = 8f;
        private static readonly int IsWalking = Animator.StringToHash(IS_WALKING);
        
        [Header("Attack")] [SerializeField] private ParticleSystem hitEffect;
        [SerializeField] private float attackSpeed = 1.5f;
        [SerializeField] private float attackDelay = 0.3f;
        [SerializeField] private int attackDamage = 1;

        Interactable _target;
        bool _playerBusy = false;

        private readonly Scenes _scenes = new Scenes();
        private readonly Tags _tags = new Tags();

        // Initialisation de l'animator et de l'agent de pathfinding
        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
        }
        
        private void Start()
        {
            AssignInputs();
        }

        /// <summary>
        /// Intialise le point and click
        /// </summary>
        void AssignInputs()
        {
            input.GetInput().Player.Move.performed += ctx => ClickToMove();
        }

        /// <summary>
        /// Vérifie le point chosi et initialise le déplacement
        /// </summary>
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

        // Sécurité pour la gestion du pathfinding puis gestion du déplacement et du visuel
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

        /// <summary>
        /// Déplacement vers le lieu choisi et vérification de la distance d'atteint pour l'interaction
        /// </summary>
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

        /// <summary>
        /// Rotation du personnage vers le lieu choisi
        /// </summary>
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

        /// <summary>
        /// Choix de l'interaction en fonction du type de l'objet ciblé
        /// </summary>
        void ReachDistance()
        {
            agent.SetDestination(transform.position);

            if (_playerBusy) return;

            _playerBusy = true;
            
            Debug.Log(_target.interactionType);

            #region InteractionType

            switch (_target.interactionType)
            {
                case InteractableType.Enemy:
                    Invoke(nameof(SendAttack), attackDelay);
                    _target = null;
                    Invoke(nameof(ResetBusyState), attackSpeed);
                    break;
                case InteractableType.Item:
                    _target.InteractWithItem();
                    _target = null;
                    Invoke(nameof(ResetBusyState), 0.5f);
                    break;
                case InteractableType.Props:
                    _target.InteractWithProps();
                    _target = null;
                    Invoke(nameof(ResetBusyState), 0.5f);
                    break;
                case InteractableType.Gate:
                    _target.InteractWithGate();
                    _target = null;
                    agent.enabled = false;
                    Invoke(nameof(ResetBusyState), 0.5f);
                    break;
                case InteractableType.EndGame:
                    _target.InteractWithEndGame();
                    _target = null;
                    agent.enabled = false;
                    Invoke(nameof(ResetBusyState), 0.5f);
                    break;
                case InteractableType.Storage:
                    _target.InteractWithStorage();
                    _target = null;
                    Invoke(nameof(ResetBusyState), 0.5f);
                    break;
            }

            #endregion
        }

        /// <summary>
        /// Attaque du personnage
        /// </summary>
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

        /// <summary>
        /// Délai pour les interactions
        /// </summary>
        void ResetBusyState()
        {
            _playerBusy = false;
            SetAnimations();
        }

        /// <summary>
        /// Gestion des animations du personnage
        /// </summary>
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