using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    /// <summary>
    /// Gestion du menu de fin
    /// </summary>
    public class EndMenu : MonoBehaviour
    {
        private readonly Scenes _scenes = new Scenes();
        
        /// <summary>
        /// Charge la scene de hub du jeu
        /// </summary>
        public void LoadHubScene()
        {
            SceneManager.LoadScene(_scenes.HUB_SCENE);
        }
    }
}
