using UnityEngine;

namespace Inventory
{
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
