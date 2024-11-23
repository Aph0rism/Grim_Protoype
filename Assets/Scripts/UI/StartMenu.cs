using System;
using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class StartMenu : MonoBehaviour
    {
        private readonly Scenes _scenes = new Scenes();
        
        public void LoadHubScene()
        {
            SceneManager.LoadScene(_scenes.HUB_SCENE);
        }
    }
}
