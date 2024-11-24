using System;
using GameManagement;
using Settings;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Environment
{
    public class Gate : MonoBehaviour
    {
        [SerializeField] private EventManager eventManager;
        [SerializeField] private GameInput gameInput;
        
        private readonly Tags _tags = new Tags();
        private readonly Scenes _scenes = new Scenes();

        private void Awake()
        {
            gameInput = GameObject.FindGameObjectWithTag(_tags.GAME_INPUT_TAG).GetComponent<GameInput>();
            eventManager = GameObject.FindGameObjectWithTag(_tags.EVENT_MANAGER_TAG).GetComponent<EventManager>();
            
            eventManager.ActivateGameObjects();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(_tags.PLAYER_TAG))
            {
                eventManager.GetInteractIndicator().SetActive(true);
                if (gameInput.CheckInteractionInput() > 0)
                {
                    eventManager.GetInteractIndicator().SetActive(false);
                    if (SceneManager.GetActiveScene().name == _scenes.HUB_SCENE)
                    {
                        SceneManager.LoadScene(_scenes.DUNGEON_SCENE);
                    }
                    else
                    {
                        SceneManager.LoadScene(_scenes.HUB_SCENE);
                    }
                }
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_tags.PLAYER_TAG))
            {
                eventManager.GetInteractIndicator().SetActive(false);
            }
        }
    }
}
