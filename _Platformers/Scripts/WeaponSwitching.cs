using UnityEditor.UIElements;
using UnityEngine;

namespace Platformers
{
    // This script's ONLY job is to visually enable/disable weapon models.
    // It receives commands from other scripts (like InventoryManager).
    public class WeaponSwitching : MonoBehaviour
    {
        // This method is now PUBLIC so other scripts can call it.
        // It takes an index to know which weapon to show.

        public InventorySlot[] toolbar;
        
        public Item[] items;

        public GameObject itemHolder;

        void Awake()
        {
            // --- 1. Find the necessary GameObjects ---

            // Find the parent UI object
            GameObject toolbarObject = GameObject.Find("Toolbar");
            //if (toolbarObject == null)
            //{
            //    Debug.LogError("Could not find 'Toolbar' GameObject!");
            //    return;
            //}

            //// Find the 3D holder object
            //itemHolder = GameObject.Find("ItemHolder");
            //if (itemHolder == null)
            //{
            //    Debug.LogError("Could not find 'ItemHolder' GameObject!");
            //    return;
            //}

            //// --- 2. Get UI Data and Initialize Arrays ---

            //toolbar = toolbarObject.GetComponentsInChildren<InventorySlot>();
            //items = new Item[toolbar.Length];

            //// --- 3. Iterate, Populate Arrays, and Instantiate 3D Models ---

            //Debug.Log("Initializing toolbar and instantiating 3D items...");

            //// Loop through each slot found in the UI
            //for (int i = 0; i < toolbar.Length; i++)
            //{
            //    // --- Get the Item Data ---
            //    InventoryItem inventoryItemComponent = toolbar[i].GetComponentInChildren<InventoryItem>();
            //    if (inventoryItemComponent != null && inventoryItemComponent.item != null)
            //    {
            //        // A. Populate our 'items' array for game logic
            //        Item currentItem = inventoryItemComponent.item;
            //        items[i] = currentItem;

            //        // --- Instantiate the 3D Model ---

            //        // B. Check if the item has a 3D prefab assigned to it
            //        if (currentItem.itemModel != null)
            //        {
            //            // C. Instantiate the prefab. The second argument makes it a child of the itemHolder.
            //            GameObject itemModel = Instantiate(currentItem.itemModel, itemHolder.transform);

            //            // D. Optional but recommended: Rename the new object for clarity in the Hierarchy
            //            itemModel.name = currentItem.itemName + " (Model)";

            //            // E. IMPORTANT: Disable the new model immediately. The WeaponSwitching script will enable the correct one.
            //            itemModel.SetActive(false);
            //        }
            //        else
            //        {
            //            Debug.LogWarning("Item '" + currentItem.itemName + "' in slot " + i + " has no 3D prefab assigned.");
            //            // We can create a placeholder so the indices don't get messed up.
            //            GameObject placeholder = new GameObject(currentItem.itemName + " (Placeholder)");
            //            placeholder.transform.SetParent(itemHolder.transform);
            //            placeholder.SetActive(false);
            //        }
            //    }
            //    else
            //    {
            //        Debug.LogWarning("Slot " + i + " is empty or missing an InventoryItem component.");
            //        // Create an empty placeholder to maintain the order.
            //        GameObject placeholder = new GameObject("Empty Slot (Placeholder)");
            //        placeholder.transform.SetParent(itemHolder.transform);
            //        placeholder.SetActive(false);
            //    }
            //}
        }
        public void SelectWeapon(int weaponIndex)
        {
            // First, do a safety check on the index.
            if (weaponIndex < 0 || weaponIndex >= transform.childCount)
            {
                // If the index is invalid (e.g., -1 for an empty slot), disable all weapons.
                UnequipAll();
                return;
            }

            // Loop through all the child weapon objects.
            for (int i = 0; i < transform.childCount; i++)
            {
                // Get the child at the current index 'i'.
                Transform weapon = transform.GetChild(i);

                // Enable the weapon if its index matches the one we want to select.
                // Disable it otherwise.
                weapon.gameObject.SetActive(i == weaponIndex);
            }

            Debug.Log(transform.GetChild(weaponIndex).name + " is now the active weapon model.");
        }

        // A public method to hide all weapons.
        public void UnequipAll()
        {
            foreach (Transform weapon in transform)
            {
                weapon.gameObject.SetActive(false);
            }
            Debug.Log("All weapons unequipped.");
        }

        // We can add a Start method to ensure we begin with no weapons active,
        // letting the InventoryManager decide what to equip first.
        void Start()
        {
            UnequipAll();
        }
    }
}