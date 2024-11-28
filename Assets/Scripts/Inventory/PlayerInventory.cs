using System;
using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// Gestion de l'inventaire du personnage
    /// </summary>
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private int weight = 0;
        [SerializeField] private Item[] hubStorage;
        [SerializeField] private Item[] bag;
        [SerializeField] private Item[] equippedGear;
    
        public void AddItem(Item itemToAdd)
        {
            // TODO

            weight += itemToAdd.Weight;
        }

        public void RemoveItem(Item itemToRemove)
        {
            // TODO
            
            weight -= itemToRemove.Weight;
        }

        public void ConsumeItem() // utilisation d'un consommable (effets à voir)
        {
            // TODO
        }

        public void EquipItem()
        {
            // TODO
        }

        private void UpdateTotalWeight()
        {
            foreach (Item itemBag in bag)
            {
                weight += itemBag.Weight;
            }
            foreach (Item itemEquipped in equippedGear)
            {
                weight += itemEquipped.Weight;
            }
        }
    }
}
