using System.Collections;
using Player;
using Settings;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Input = UnityEngine.Windows.Input;

namespace UI
{
    /// <summary>
    /// Gestion des menus et des principaux états du jeu
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        [SerializeField] private GameInput gameInput;
        [SerializeField] private PlayerStats playerStats;
        
        [Header("Menus")]
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject storageMenu;
        [SerializeField] private GameObject inventoryMenu;
        [SerializeField] private GameObject playerMenu;
        
        private GameObject _player;
        private GameObject _playerCanvas;
        private GameObject _mainCamera;
        
        private readonly Tags _tags = new Tags();
        private readonly Scenes _scenes = new Scenes();
        
        // Initialisation et désactivation des menus et objets de base du jeu
        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag(_tags.PLAYER_TAG);
            _playerCanvas = GameObject.FindGameObjectWithTag(_tags.PLAYER_CANVAS_TAG);
            _mainCamera = GameObject.FindGameObjectWithTag(_tags.MAIN_CAMERA_TAG);
            
            storageMenu.SetActive(false);
            pauseMenu.SetActive(false);
            inventoryMenu.SetActive(false);
            playerMenu.SetActive(false);
            
            DesactivateGameObjects();
        }

        private void Update()
        {
            #region CheckPauseInput

            if (gameInput.CheckPauseInput())
            {
                if (CheckMenus())
                {
                    if (pauseMenu.activeSelf)
                    {
                        Resume();
                    }
                    else
                    {
                        gameInput.DisableMove();
                        pauseMenu.SetActive(true);
                        Time.timeScale = 0;
                    }
                }
                else
                {
                    DisableMenus();
                }
            }

            #endregion

            #region CheckMenus

            if (!pauseMenu.activeSelf)
            {
                if (gameInput.CheckInventoryInput())
                {
                    if (inventoryMenu.activeSelf)
                    {
                        DisableInventory();
                    }
                    else
                    {
                        gameInput.DisableMove();
                        inventoryMenu.SetActive(true);
                    }
                }
                
                if (gameInput.CheckPlayerInput())
                {
                    if (playerMenu.activeSelf)
                    {
                        DisablePlayer();
                    }
                    else
                    {
                        gameInput.DisableMove();
                        playerMenu.SetActive(true);
                    }
                }

                if (gameInput.CheckInteractionInput())
                {
                    playerStats.TakeDamage(100, true, true);
                    Debug.Log(playerStats.GetStats());
                }
            }

            #endregion
        }
        
        /// <summary>
        /// Charge et initialise la scène du menu de départ
        /// </summary>
        public void LoadStartScene()
        {
            pauseMenu.SetActive(false);
            DesactivateGameObjects();
            gameInput.EnableMove();
            SceneManager.LoadScene(_scenes.START_SCENE);
            Time.timeScale = 1;
        }
        
        /// <summary>
        /// Arrête le jeu
        /// </summary>
        public void Exit()
        {
            Application.Quit();
        }
        
        /// <summary>
        /// Gère la mort du joueur
        /// </summary>
        public void GameOver()
        {
            DesactivateGameObjects();
            SceneManager.LoadScene(_scenes.GAME_OVER_SCENE);
        }

        /// <summary>
        /// Reprend le jeu et enlève la pause
        /// </summary>
        public void Resume()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            StartCoroutine(DelayEnableMove());
        }

        /// <summary>
        /// Active le menu de stockage et désactive le déplacement
        /// </summary>
        public void EnableStorage()
        {
            gameInput.DisableMove();
            storageMenu.SetActive(true);
        }

        /// <summary>
        /// Désactive tous les menus et active le déplacement
        /// </summary>
        private void DisableMenus()
        {
            storageMenu.SetActive(false);
            inventoryMenu.SetActive(false);
            playerMenu.SetActive(false);
            StartCoroutine(DelayEnableMove());
        }
        
        /// <summary>
        /// Désactive le menu de stockage et vérifie si aucun autre menu est actif pour activer le déplacement
        /// </summary>
        public void DisableStorage()
        {
            storageMenu.SetActive(false);
            if (CheckMenus())
            {
                StartCoroutine(DelayEnableMove());
            }
        }
        
        /// <summary>
        /// Désactive le menu d'inventaire et vérifie si aucun autre menu est actif pour activer le déplacement
        /// </summary>
        public void DisableInventory()
        {
            inventoryMenu.SetActive(false);
            if (CheckMenus())
            {
                StartCoroutine(DelayEnableMove());
            }
        }
        
        /// <summary>
        /// Désactive le menu du personnage et vérifie si aucun autre menu est actif pour activer le déplacement
        /// </summary>
        public void DisablePlayer()
        {
            playerMenu.SetActive(false);
            if (CheckMenus())
            {
                StartCoroutine(DelayEnableMove());
            }
        }

        /// <summary>
        /// Vérifie si tous les menus sont désactivés
        /// </summary>
        /// <returns>true si tous les menus sont désactivés, false sinon</returns>
        private bool CheckMenus()
        {
            return !storageMenu.activeSelf && !inventoryMenu.activeSelf && !playerMenu.activeSelf;
        }

        /// <summary>
        /// Désactive les objets de base du jeu
        /// </summary>
        public void DesactivateGameObjects()
        {
            _player.SetActive(false);
            _playerCanvas.SetActive(false);
            _mainCamera.SetActive(false);
        }
        
        /// <summary>
        /// Active les objets de base du jeu
        /// </summary>
        public void ActivateGameObjects()
        {
            _player.SetActive(true);
            _playerCanvas.SetActive(true);
            _mainCamera.SetActive(true);
        }

        /// <summary>
        /// Active le mouvement après un court délai
        /// </summary>
        private IEnumerator DelayEnableMove()
        {
            yield return new WaitForSeconds(0.2f);
            if (CheckMenus())
            {
                gameInput.EnableMove();
            }
        }
    }
}
