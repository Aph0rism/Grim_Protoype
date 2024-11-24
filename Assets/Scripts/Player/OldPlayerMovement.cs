using System;
using GameManagement;
using Settings;
using UnityEngine;

namespace Player
{
    public class OldPlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 7f;
        [SerializeField] private float jumpForce = 15f;
        [SerializeField] private GameInput gameInput;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform cameraTracer;
        [SerializeField] private LayerMask floorMask;

        private float _raycastLength = 0.2f;
        private float _raycastOffset = 0.1f;
        private bool _isWalking;
        private bool _isGrounded = false;
        private bool _isJumping = false;
        
        private void Update()
        {
            // récupérer les inputs de déplacement du joueur (x et z)
            Vector2 inputVector = gameInput.GetMovementVectorNormalized();

            // gestion de la direction relative à la caméra
            Vector3 cameraForward = cameraTracer.forward;
            Vector3 cameraRight = cameraTracer.right;

            cameraForward.y = 0;
            cameraRight.y = 0;

            Vector3 forwardRelative = inputVector.y * cameraForward;
            Vector3 rightRelative = inputVector.x * cameraRight;
        
            Vector3 moveDir = forwardRelative + rightRelative;

            // utilisation de linearvelocity pour le déplacement du joueur et éviter de traverser les obstacles
            rb.linearVelocity = new Vector3(moveDir.x * moveSpeed, rb.linearVelocity.y, moveDir.z * moveSpeed);

            // Raycast pour déterminer si le joueur est au sol ou non (le debug draw affiche le raycast)
            var position = transform.position;
            Debug.DrawRay(new Vector3(position.x, position.y + _raycastOffset, position.z), -Vector3.up * _raycastLength, Color.red);
            _isGrounded = Physics.Raycast(new Vector3(position.x, position.y + _raycastOffset, position.z), -Vector3.up, _raycastLength, floorMask);

            // vérifications pour activer le saut ou le désactiver
            if (!_isJumping && gameInput.CheckJumpInput() > 0 && _isGrounded)
            {
                _isJumping = true;
            }
            if (!_isGrounded)
            {
                _isJumping = false;
            }

            // variable pour l'animation
            _isWalking = moveDir != Vector3.zero;

            // Rotation fluide du joueur quand il tourne
            float rotateSpeed = 10f;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }
    
        private void FixedUpdate() // frame fixées
        {
            if (_isJumping)
            {
                // annulation de la force verticale avant le saut pour éviter les sauts géants
                var vector3 = rb.linearVelocity;
                vector3.y = 0;
                rb.linearVelocity = vector3;
                
                rb.AddForce(transform.up.normalized * jumpForce, ForceMode.Impulse);
            }
        }
    
        public bool IsWalking() // accesseur
        {
            return _isWalking;
        }
    
        public bool IsGrounded() // accesseur
        {
            return _isGrounded;
        }
    }
}
