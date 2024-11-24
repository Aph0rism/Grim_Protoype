using Settings;
using UnityEngine;

namespace GameManagement
{
    public class PlayerSpawn : MonoBehaviour
    {
        private Tags _tags = new Tags();
        
        void Start()
        {
            GameObject.FindGameObjectWithTag(_tags.PLAYER_TAG).transform.position = this.transform.position;
        }
    }
}
