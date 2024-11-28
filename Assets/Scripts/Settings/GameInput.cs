using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Settings
{
    /// <summary>
    /// Gestion des inputs du jeu
    /// </summary>
    public class GameInput : MonoBehaviour
    {
        private PlayerInputActions _playerInputActions;

        // Initialisation du système d'input du joueur
        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Enable();
        }

        
        /// <summary>
        /// Récupère et normalise le vecteur de mouvement avec l'input WASD ou du joystick de manette
        /// </summary>
        /// <returns>Le vectour normalisé de la direction du mouvement du personnage</returns>
        public Vector2 GetMovementVectorNormalized()
        {
            Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        
            inputVector = inputVector.normalized;

            return inputVector;
        }

        /// <summary>
        /// Récupère le script des inputs
        /// </summary>
        /// <returns>Le script des inputs</returns>
        public PlayerInputActions GetInput()
        {
            return _playerInputActions;
        }

        /// <summary>
        /// Lit l'input du saut
        /// </summary>
        /// <returns>true si la touche du saut et enfoncée, false sinon</returns>
        public float CheckJumpInput()
        {
            return _playerInputActions.Player.Jump.ReadValue<float>();
        }
        
        /// <summary>
        /// Lit l'input de l'interaction
        /// </summary>
        /// <returns>true si la touche de l'interaction vient d'être pressée, false sinon</returns>
        public bool CheckInteractionInput()
        {
            return _playerInputActions.Player.Interact.WasPressedThisFrame();
        }
        
        /// <summary>
        /// Lit l'input de pause
        /// </summary>
        /// <returns>true si la touche de pause vient d'être pressée, false sinon</returns>
        public bool CheckPauseInput()
        {
            return _playerInputActions.Player.Pause.WasPressedThisFrame();
        }
        
        /// <summary>
        /// Lit l'input de l'inventaire
        /// </summary>
        /// <returns>true si la touche de l'inventaire vient d'être pressée, false sinon</returns>
        public bool CheckInventoryInput()
        {
            return _playerInputActions.Player.Inventory.WasPressedThisFrame();
        }
        
        /// <summary>
        /// Lit l'input du menu du personnage
        /// </summary>
        /// <returns>true si la touche du menu du personnage vient d'être pressée, false sinon</returns>
        public bool CheckPlayerInput()
        {
            return _playerInputActions.Player.Character.WasPressedThisFrame();
        }
        
        /// <summary>
        /// Active le mouvement
        /// </summary>
        public void EnableMove()
        {
            _playerInputActions.Player.Move.Enable();
        }

        /// <summary>
        /// Désactive le mouvement
        /// </summary>
        public void DisableMove()
        {
            _playerInputActions.Player.Move.Disable();
        }
        
        // sécurité pour la gestion des inputs
        private void OnDestroy()
        {
            _playerInputActions.Player.Disable();
        }
    }
}
