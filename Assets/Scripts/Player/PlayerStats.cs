using UI;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Gestion des statistiques du joueur
    /// </summary>
    public class PlayerStats : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private int currentHealth = 100;
        [SerializeField] private int maxHealth = 100;
        
        [Header("Mana")]
        [SerializeField] private int currentMana = 100;
        [SerializeField] private int maxMana = 100;
        
        [Header("")]
        [SerializeField] private int armor = 10;
        [SerializeField] private int dodge = 10;
        
        [SerializeField] private EventManager eventManager;

        private bool _isInvincible;

        /// <summary>
        /// Gère la reception de dégâts du joueur
        /// </summary>
        /// <param name="value">La quantité de dégâts à infliger au joueur</param>
        /// <param name="isDodgeable">Acive la possibilité pour le joueur d'esquiver</param>
        /// <param name="isReducible">Acive la possibilité pour le joueur de réduire les dégâts</param>
        public void TakeDamage(int value, bool isDodgeable, bool isReducible)
        {
            #region VerifyDamageType

            if (_isInvincible)
            {
                return;
            }
            
            if (isDodgeable)
            {
                int dodgeThrow = Random.Range(1, 100);
                if (dodgeThrow <= dodge)
                {
                    return;
                }
            }

            if (isReducible && armor > 0)
            {
                value = value / armor;
            }

            #endregion
            

            if (currentHealth <= value) // cas de mort
            {
                currentHealth = 100;
                currentMana = 100;
                eventManager.GameOver();
                return;
            }

            currentHealth -= value;
        }
        
        /// <summary>
        /// Gère la consommation de mana
        /// </summary>
        /// <param name="value">La quantité de mana à consommer</param>
        /// <returns>true si le mana peut être consommé, sinon false</returns>
        public bool ConsumeMana(int value)
        {
            if (currentMana < value)
            {
                return false;
            }

            currentMana -= value;
            return true;
        }

        /// <summary>
        /// Débug affichant les stats du joueur
        /// </summary>
        public string GetStats()
        {
            return "Health : " + currentHealth + "/" + maxHealth + "Mana : " + currentMana + "/" + maxMana +
                   "\nDodge : " + dodge + "\nArmor : " + armor;
        }
    }
}
