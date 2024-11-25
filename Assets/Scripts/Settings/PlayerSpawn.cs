using UI;
using UnityEngine;

namespace Settings
{
    public class PlayerSpawn : MonoBehaviour
    {
        private Tags _tags = new Tags();
        private EventManager _eventManager;
        
        void Awake()
        {
            _eventManager = GameObject.FindGameObjectWithTag(_tags.EVENT_MANAGER_TAG).GetComponent<EventManager>();
            _eventManager.ActivateGameObjects();
            GameObject.FindGameObjectWithTag(_tags.PLAYER_TAG).transform.position = this.transform.position;
        }
    }
}
