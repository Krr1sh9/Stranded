using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;


namespace Platformers
{
    

    public class TradeUIManager : MonoBehaviour
    {
        // --- INSPECTOR REFERENCES ---
        // The parent object that will hold all the item slots.
        // Assign the "ItemListContainer" GameObject here.
        public Transform npcItemContainer;

        // The prefab for a single item slot.
        // It must have the ItemSlotUI.cs script on it.
        public GameObject itemSlotPrefab;

        // --- PRIVATE REFERENCES ---
        // A reference to the main trade window panel.
        private GameObject tradeWindow;

        // A reference to the player's inventory.
        private InventorySlot[] inventorySlots;

        void Awake()
        {
            // Since this script is on the NPC_Panel, its parent is the TradeWindow.
            // This makes the reference automatic and less prone to breaking.
            tradeWindow = transform.parent.gameObject;

            // Find the player's inventory once.
            inventorySlots = FindObjectsByType<InventorySlot>(FindObjectsSortMode.None);

            // Ensure the window is closed when the game starts.
            if (tradeWindow != null)
            {
                tradeWindow.SetActive(false);
            }
        }

        public void OpenTradeWindow(List<Item> npcItems)
        {
            // Clear any old items from the list
            foreach (Transform child in npcItemContainer)
            {
                Destroy(child.gameObject);
            }

            // Create and populate a slot for each item the NPC has for sale
            foreach (var item in npcItems)
            {
                GameObject slotObject = Instantiate(itemSlotPrefab, npcItemContainer);
                ItemSlotUI itemSlot = slotObject.GetComponent<ItemSlotUI>();

                if (itemSlot != null)
                {
                    // Tell the slot which item to display
                    itemSlot.SetItem(item);

                    // Add a listener to the slot's button to call our buy function
                    itemSlot.buyButton.onClick.AddListener(() => OnBuyButtonClicked(item));
                }
            }

            // Show the main trade window
            tradeWindow.SetActive(true);
        }

        void OnBuyButtonClicked(Item item)
        {
            if (inventorySlots != null)
            {
                foreach (var slot in inventorySlots)
                {
                    InventoryItem itemComponent = slot.GetComponentInChildren<InventoryItem>();
                    if (itemComponent != null && itemComponent.item == item)
                    {
                        itemComponent.inventoryManager.BuyItem(itemComponent.item);
                        break;
                    }
                }
            }
        }

        public void CloseTradeWindow()
        {
            tradeWindow.SetActive(false);
        }
    }
}
