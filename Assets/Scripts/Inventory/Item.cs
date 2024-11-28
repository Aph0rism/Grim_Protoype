using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// Objet de base du jeu
    /// </summary>
    [CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
    public class Item : ScriptableObject
    {
        public int Id;
        public int Weight;
        
        public string Name;
        public string Type;
        public string Description;
    }
}
