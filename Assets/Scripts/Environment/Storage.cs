using System;
using System.Collections;
using GameManagement;
using Inventory;
using Settings;
using UI;
using UnityEditor;
using UnityEngine;

namespace Environment
{
    public class Storage : MonoBehaviour
    {
        [SerializeField] private bool isInRange;
        [SerializeField] private int capacity;
        [SerializeField] private Item[] storage;
        [SerializeField] private GameInput gameInput;
        [SerializeField] private EventManager eventManager;
        
        private Tags _tags = new Tags();

        private void Awake()
        {
            gameInput = GameObject.FindGameObjectWithTag(_tags.GAME_INPUT_TAG).GetComponent<GameInput>();
            eventManager = GameObject.FindGameObjectWithTag(_tags.EVENT_MANAGER_TAG).GetComponent<EventManager>();
        }

        private void Update()
        {
            if (isInRange)
            {
                if (gameInput.CheckPauseInput())
                {
                    eventManager.GetStorage().SetActive(false);
                    StartCoroutine(DelayPause());
                }
                if (gameInput.CheckInteractionInput() > 0 && Time.timeScale != 0)
                {
                    eventManager.GetStorage().SetActive(true);
                    eventManager.canPause = false;
                }
            }
        }

        public bool AddToStorage(Item itemToAdd)
        {
            // TODO
            return false;
        }
        
        public bool GetFromStorage(Item itemToGet)
        {
            // TODO
            return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_tags.PLAYER_TAG))
            {
                eventManager.GetInteractIndicator().SetActive(true);
                isInRange = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_tags.PLAYER_TAG))
            {
                eventManager.GetInteractIndicator().SetActive(false);
                isInRange = false;
                eventManager.GetStorage().SetActive(false);
                eventManager.canPause = true;
            }
        }

        private IEnumerator DelayPause()
        {
            yield return new WaitForSeconds(0.2f);
            eventManager.canPause = true;
        }
    }
}
