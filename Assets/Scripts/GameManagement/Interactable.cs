using System;
using Settings;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement
{
    public enum InteractableType
    {
        Enemy, Item, Props, Gate, Storage
    }

    public class Interactable : MonoBehaviour
    {
        public EventManager eventManager;
        public InteractableType interactionType;
        public int health;
        public bool isDamageable;

        private readonly Tags _tags = new Tags();
        private readonly Scenes _scenes = new Scenes();

        private void Awake()
        {
            eventManager = GameObject.FindGameObjectWithTag(_tags.EVENT_MANAGER_TAG).GetComponent<EventManager>();
        }

        public void InteractWithItem()
        {
            
        }

        public void InteractWithEnemy()
        {
            
        }

        public void InteractWithProps()
        {
            
        }

        public void InteractWithGate()
        {
            if (SceneManager.GetActiveScene().name == _scenes.HUB_SCENE)
            {
                SceneManager.LoadScene(_scenes.DUNGEON_SCENE);
            }
            else
            {
                SceneManager.LoadScene(_scenes.HUB_SCENE);
            }
        }

        public void InteractWithStorage()
        {
            eventManager.GetStorage().SetActive(true);
        }
        
        public void Destroyed(){}
        public void TakeDamage(int damage){}
    }
}
