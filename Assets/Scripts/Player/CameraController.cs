using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Player
{
    public class CameraController : MonoBehaviour
    {
        private const string PLAYER_TAG = "Player";
        private bool isDragging = false;

        [SerializeField] private float moveSpeed;
        [SerializeField] private float followDistance;
        [SerializeField] private float teleportDistanceThreshold;
        [SerializeField] private Transform player;
        [SerializeField] private Quaternion rotation;
        [SerializeField] private Vector3 offset;
        [SerializeField] private CinemachineInputAxisController inputAxisController;
        [SerializeField] private float sensitivityX = 5f; // Sensibilité horizontale
        [SerializeField] private float sensitivityY = 5f; // Sensibilité verticale

        private void Start()
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
            }
        }

        private void Update()
        {
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

            if (Input.GetMouseButtonDown(2))
            {
                isDragging = true;
                inputAxisController.enabled = true; // Active l'Input Axis Controller
            }

            if (Input.GetMouseButtonUp(2))
            {
                isDragging = false;
                inputAxisController.enabled = false; // Désactive l'Input Axis Controller
            }

            // Contrôle manuel des axes si le clic molette est maintenu
            if (isDragging)
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                inputAxisController.AutoEnableInputs = false;
                inputAxisController.AutoEnableInputs = false;
            }
        }
    }
}
