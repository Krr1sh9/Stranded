using UnityEngine;

namespace Platformers
{
    public class EquipmentManager : MonoBehaviour
    {
        [SerializeField] private EquipmentSlot[] equipmentSlots; // Assign all your slots here
        [SerializeField] private FirstPersonController playerController; // Assign your player
        [SerializeField] private CharacterStatss characterStats; // Reference to character stats
        void Start()
        {
            // Find player if not assigned
            //if (playerController == null)
            //{
            //    playerController = FindObjectOfType<FirstPersonController>();
            //}
            // Initial stat calculation when the game starts
            if (equipmentSlots.Length != 0) {
                RecalculateAllStats();
            }
        }

        // This is the core method that gathers stats from all slots
        public void RecalculateAllStats()
        {
            int totalArmorBonus = 0;
            int totalDamageBonus = 0; // Or whatever other stats you have

            // Loop through every single equipment slot
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
             
                    
                    // Get the item currently in the slot
                    InventoryItem equippedItemUI = equipmentSlots[i].GetComponentInChildren<InventoryItem>();
                    Debug.Log("Recalculating Stats for Slot " + i + ": " + equippedItemUI);

                    if (equippedItemUI != null)
                    {
                        // Get the actual data from the item
                        Item itemData = equippedItemUI.item;
                        if (itemData != null)
                        {
                            // Add this item's stats to our totals
                            totalArmorBonus += itemData.defenseBonus;
                            totalDamageBonus += itemData.strengthBonus; // Assuming strength adds to damage
                        }
                    }
                
            }

            // Now, apply the final calculated bonuses to the player's base stats
            // Make sure your FirstPersonController has 'baseDefense' and 'currentDefense' variables
            if (playerController != null)
            {
                characterStats.BaseDefense = characterStats.BaseDefense + totalArmorBonus;
                characterStats.BaseStrength = characterStats.BaseStrength + totalDamageBonus;
                characterStats.StrengthText.text = $"Strength: {characterStats.BaseStrength}";
                characterStats.DefenseText.text = $"Defense: {characterStats.BaseDefense}";

                Debug.Log($"Stats Recalculated! Current Defense: {characterStats.BaseDefense}, Current Strength: {characterStats.BaseStrength}");
            }
        }
    }
}