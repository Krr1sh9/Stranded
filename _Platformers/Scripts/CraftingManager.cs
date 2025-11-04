using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

namespace Platformers
{
    public class CraftingManager : MonoBehaviour
    {
        [Header("References")]
        // This is the parent object of your crafting slots
        public Transform craftingGridParent;
        public CraftingResultSlot resultSlot; // For displaying the result

        [Header("Data")]
        public CraftingRecipeSO[] recipes;
        public InventoryManager inventoryManager;


        // This holds the actual UI slot components
        private CraftingSlots[] slots;

        public CraftingSlots[] CraftingSlots;
        public Button Craft;
        [HideInInspector]bool allEmpty = false;
        void Start()
        {
            // Get all the slots ONCE at the start.
            // Note the 's' in GetComponentsInChildren
            slots = craftingGridParent.GetComponentsInChildren<CraftingSlots>();
            Craft.gameObject.SetActive(false);
            CheckEmpty(31);


        }

        public void Update()
        {
            CheckEmpty(38);
        }
        public void CheckEmpty(int line) {

            allEmpty = false;


            for (int i = 0; i < slots.Length; i++)
            {
                InventoryItem itemInSlot = slots[i].GetComponentInChildren<InventoryItem>();
                if (itemInSlot != null)
                {
                    allEmpty = true;
                    break;
                }
                
            }
            
            if (!allEmpty)
            {
               
                Craft.gameObject.SetActive(false);
            }
            else
            {
      
                Craft.gameObject.SetActive(true);
            }


        }
        /// <summary>
        /// This method should be called whenever the crafting grid changes.
        /// It checks for a valid recipe and updates the UI.
        /// </summary>
        public void UpdateCrafting()
        {
            // 1. Get the current items from the UI grid
            Item[] currentItems = new Item[slots.Length];
            for (int i = 0; i < slots.Length; i++)
            {
                // Find the InventoryItem visual in the slot
                InventoryItem itemInSlot = slots[i].GetComponentInChildren<InventoryItem>();

                // --- FIX for Nulls ---
                // If an item exists, get its data. Otherwise, the entry remains null.
                if (itemInSlot != null)
                {
                    currentItems[i] = itemInSlot.item; // Assuming itemData holds the SO
                }
            }

            // 2. Find a matching recipe
            CraftingRecipeSO matchedRecipe = FindMatchingRecipe(currentItems);
      

            //for (int i = 0; i < currentItems.Length; i++)
            //{

            //    if (currentItems[i] != null) {
            //        Debug.Log("Current Item " + i + ": " + currentItems[i]);
            //    }
            //}

            // 3. Update the result slot
            if (matchedRecipe != null)
            {
                inventoryManager.AddItems(matchedRecipe.result);// Show the potential result
                foreach (CraftingSlots slot in slots)
                {
                    InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                    if (itemInSlot != null)
                    {
                        // You would typically decrease the item's count here,
                        // and only destroy if the count is <= 0.

                        // --- FIX for Destroying ---
                        // Destroy the GameObject, not the ScriptableObject asset
                        Destroy(itemInSlot.gameObject);

                    }
                }

                Craft.gameObject.SetActive(false);

            }
            else
            {
                Debug.Log("No match");
            }
        }

        private CraftingRecipeSO FindMatchingRecipe(Item[] itemsInGrid)
        {

      
            // Loop through each recipe blueprint
            foreach (CraftingRecipeSO recipe in recipes)
            {
        
                // Check if the recipe's length matches our grid's length
                //if (recipe.recipeArray.Length != itemsInGrid.Length)
                //{
                //    continue; // Skip this recipe if dimensions don't match
                //}

                //for (int i = 0; i < recipe.recipeArray.Length; i++)
                //{
                //    // If any item doesn't match, this recipe is not a match
                //    if (recipe.recipeArray[i] != itemsInGrid[i])
                //    {
                //        Debug.Log("Recipe Item " + i + ": " + recipe.recipeArray[i]);
                //    }
                //    else
                //    {
                //        Debug.Log("Matched Item " + i + ": " + itemsInGrid[i]);
                //    }
                //}

                // --- FIX for Array Comparison ---
                // We must compare the arrays element by element.
                bool isMatch = true;
                for (int i = 0; i < recipe.recipeArray.Length; i++)
                {
                    // If any item doesn't match, this recipe is not a match
                    if (recipe.recipeArray[i] != itemsInGrid[i])
                    {
                        isMatch = false;
                        break; // No need to check the rest of this recipe
                    }
                }

                // If, after the loop, isMatch is still true, we found it!
                if (isMatch)
                {
                    return recipe;
                }
            }
            // If we check all recipes and find no match, return null
            return null;
        }

        /// <summary>
        /// This is called when the player takes the item from the result slot.
        /// It consumes the ingredients from the crafting grid.
        /// </summary>
        //public void OnCraftingComplete()
        //{
        //    // (You would call this from your result slot script when the player picks up the item)

        //    // Loop through all the slots in the crafting grid
        //    foreach (CraftingSlots slot in slots)
        //    {
        //        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        //        if (itemInSlot != null)
        //        {
        //            // You would typically decrease the item's count here,
        //            // and only destroy if the count is <= 0.

        //            // --- FIX for Destroying ---
        //            // Destroy the GameObject, not the ScriptableObject asset
        //            Destroy(itemInSlot.gameObject);

        //        }
        //    }

        //    Craft.gameObject.SetActive(false);

        //    // After consuming ingredients, re-check the grid (it should now be empty)
        //    UpdateCrafting();
        //}
    }
}