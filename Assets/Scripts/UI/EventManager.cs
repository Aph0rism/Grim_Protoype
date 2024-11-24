using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class EventManager : MonoBehaviour
    {
        [SerializeField] public bool canPause = true;
        [SerializeField] private GameInput gameInput;
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject storageMenu;
        [SerializeField] private GameObject interactIndicator;
        
        private GameObject _gameSystem;
        private GameObject _player;
        private GameObject _playerCanvas;
        private GameObject _mainCamera;
        
        private readonly Tags _tags = new Tags();
        private readonly Scenes _scenes = new Scenes();
        
        private void Awake()
        {
            _gameSystem = GameObject.FindGameObjectWithTag(_tags.GAME_SYSTEM_TAG);
            _player = GameObject.FindGameObjectWithTag(_tags.PLAYER_TAG);
            _playerCanvas = GameObject.FindGameObjectWithTag(_tags.PLAYER_CANVAS_TAG);
            _mainCamera = GameObject.FindGameObjectWithTag(_tags.MAIN_CAMERA_TAG);
            
            storageMenu.SetActive(false);
            pauseMenu.SetActive(false);
            interactIndicator.SetActive(false);
            
            DesactivateGameObjects();
        }

        private void Update()
        {
            if (gameInput.CheckPauseInput() && canPause)
            {
                if (pauseMenu.activeSelf)
                {
                    Resume();
                }
                else
                {
                    pauseMenu.SetActive(true);
                    Time.timeScale = 0;
                }
            }
        }
        
        public void LoadStartScene()
        {
            pauseMenu.SetActive(false);
            DesactivateGameObjects();
            SceneManager.LoadScene(_scenes.START_SCENE);
            Time.timeScale = 1;
        }
        public void Exit()
        {
            Application.Quit();
        }

        public void Resume()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }

        public GameObject GetStorage()
        {
            return storageMenu;
        }
        
        public GameObject GetInteractIndicator()
        {
            return interactIndicator;
        }

        private void DesactivateGameObjects()
        {
            _player.SetActive(false);
            _playerCanvas.SetActive(false);
            _mainCamera.SetActive(false);
        }
        
        public void ActivateGameObjects()
        {
            _player.SetActive(true);
            _playerCanvas.SetActive(true);
            _mainCamera.SetActive(true);
        }
    }
}
