using UnityEngine;
using UnityEngine.SceneManagement;

namespace Settings
{
    public class SaveToOtherScenes : MonoBehaviour
    {
        [SerializeField] private GameObject[] objectsToSave;

        private Scenes _scenes = new Scenes();
        void Awake()
        {
            // Appel de DontDestroyOnLoad pour garder les objets d'une scene à l'autre
            foreach (GameObject toSave in objectsToSave)
            {
                GameObject.DontDestroyOnLoad(toSave);
            }

            SceneManager.LoadScene(_scenes.START_SCENE);
        }
    }
}
