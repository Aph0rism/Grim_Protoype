using System;
using UnityEngine;

namespace Player
{
    public class CameraController : MonoBehaviour
    {
        private const string PLAYER_TAG = "Player";
        
        [SerializeField] private float moveSpeed;
        [SerializeField] private float followDistance;
        [SerializeField] private float teleportDistanceThreshold;
        [SerializeField] private Transform player;
        [SerializeField] private Quaternion rotation;
        [SerializeField] private Vector3 offset;

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
        }
    }
}
