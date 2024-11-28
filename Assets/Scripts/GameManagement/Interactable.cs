using System;
using Settings;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement
{
    /// <summary>
    /// Définis le type d'objet de l'interaction
    /// </summary>
    public enum InteractableType
    {
        Enemy, Item, Props, Gate, EndGame, Storage
    }

    /// <summary>
    /// Gestion de l'interaction
    /// </summary>
    public class Interactable : MonoBehaviour
    {
        public EventManager eventManager;
        public int health;
        public bool isDamageable;
        
        [Header("Type")]
        public InteractableType interactionType;

        private readonly Tags _tags = new Tags();
        private readonly Scenes _scenes = new Scenes();

        private void Awake()
        {
            eventManager = GameObject.FindGameObjectWithTag(_tags.EVENT_MANAGER_TAG).GetComponent<EventManager>();
        }

        /// <summary>
        /// Interaction avec un objet "Item"
        /// </summary>
        public void InteractWithItem()
        {
            
        }

        /// <summary>
        /// Interaction avec un objet "Enemy"
        /// </summary>
        public void InteractWithEnemy()
        {
            
        }

        /// <summary>
        /// Interaction avec un objet "Props"
        /// </summary>
        public void InteractWithProps()
        {
            
        }

        /// <summary>
        /// Interaction avec un objet "Gate"
        /// </summary>
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
        
        /// <summary>
        /// Interaction avec un objet "EndGame"
        /// </summary>
        public void InteractWithEndGame()
        {
            eventManager.DesactivateGameObjects();
            SceneManager.LoadScene(_scenes.END_SCENE);
        }

        /// <summary>
        /// Interaction avec un objet "Storage"
        /// </summary>
        public void InteractWithStorage()
        {
            eventManager.EnableStorage();
        }
        
        public void TakeDamage(int damage){}
    }
}
