using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Settings
{
    public class GameInput : MonoBehaviour
    {
        private PlayerInputActions _playerInputActions;

        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Enable();
        }

        public Vector2 GetMovementVectorNormalized()
        {
            Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        
            inputVector = inputVector.normalized;

            return inputVector;
        }

        public PlayerInputActions GetInput()
        {
            return _playerInputActions;
        }

        public float CheckJumpInput()
        {
            return _playerInputActions.Player.Jump.ReadValue<float>();
        }
        
        public float CheckInteractionInput()
        {
            return _playerInputActions.Player.Interact.ReadValue<float>();
        }
        
        public bool CheckPauseInput()
        {
            return _playerInputActions.Player.Pause.WasPressedThisFrame();
        }

        private void OnDestroy()
        {
            _playerInputActions.Player.Disable();
        }
    }
}
