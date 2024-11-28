using System;
using Settings;
using Unity.Cinemachine;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Gestion de la caméra
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineInputAxisController inputAxisController;
        [SerializeField] private Transform player;
        
        [Header("Settings")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float followDistance;
        [SerializeField] private float teleportDistanceThreshold;
        [SerializeField] private float sensitivityX = 5f; // Sensibilité horizontale
        [SerializeField] private float sensitivityY = 5f; // Sensibilité verticale
        
        [SerializeField] private Quaternion rotation;
        [SerializeField] private Vector3 offset;
        
        private bool _isDragging = false;
        
        private readonly Tags _tags = new Tags();

        // Sécurité pour récupérer le joueur
        private void Start()
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag(_tags.PLAYER_TAG).transform;
            }
        }

        private void Update()
        {
            #region CameraFollow

            if (Vector3.Distance(transform.position, player.position) > teleportDistanceThreshold)
            {
                // téléporte la caméra si elle est loin (supérieur à la distance teleportDistanceThreshold)
                transform.position = player.position + offset + -transform.forward * followDistance;
            }
            else
            {
                // suivi de la caméra fluide
                Vector3 position = Vector3.Lerp(transform.position, 
                    player.position + offset + -transform.forward * followDistance, Time.deltaTime * moveSpeed);
                transform.position = position;
            }

            // garde la rotation précise
            transform.rotation = rotation;

            #endregion

            #region Dragging

            if (Input.GetMouseButtonDown(2))
            {
                _isDragging = true;
                inputAxisController.enabled = true; // Active l'Input Axis Controller
            }

            if (Input.GetMouseButtonUp(2))
            {
                _isDragging = false;
                inputAxisController.enabled = false; // Désactive l'Input Axis Controller
            }

            // Contrôle manuel des axes si le clic molette est maintenu
            if (_isDragging)
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                inputAxisController.AutoEnableInputs = false;
                inputAxisController.AutoEnableInputs = false;
            }

            #endregion
        }
    }
}
