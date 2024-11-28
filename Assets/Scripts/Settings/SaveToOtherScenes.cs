using UnityEngine;
using UnityEngine.SceneManagement;

namespace Settings
{
    /// <summary>
    /// Applique DontDestroyOnLoad à des objets spécifiques dans une scène de transition
    /// </summary>
    public class SaveToOtherScenes : MonoBehaviour
    {
        [SerializeField] private GameObject[] objectsToSave;

        private Scenes _scenes = new Scenes();
        
        /*
         Appel de DontDestroyOnLoad sur les objets spécifiés pour conserver leurs états d'une scène à l'autre et
         chargement de la scène de menu de départ
         */
        void Awake()
        {
            foreach (GameObject toSave in objectsToSave)
            {
                GameObject.DontDestroyOnLoad(toSave);
            }

            SceneManager.LoadScene(_scenes.START_SCENE);
        }
    }
}
