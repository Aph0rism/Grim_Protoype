using UI;
using UnityEngine;

namespace Settings
{
    /// <summary>
    /// Gestion de l'apparition du joueur
    /// </summary>
    public class PlayerSpawn : MonoBehaviour
    {
        private EventManager _eventManager;
        
        private Tags _tags = new Tags();
        
        // Activation des objets de base du jeu et initialisation de la position d'apparition du joueur
        void Awake()
        {
            _eventManager = GameObject.FindGameObjectWithTag(_tags.EVENT_MANAGER_TAG).GetComponent<EventManager>();
            _eventManager.ActivateGameObjects();
            
            GameObject player = GameObject.FindGameObjectWithTag(_tags.PLAYER_TAG);
            player.transform.position = this.transform.position;
        }
    }
}
