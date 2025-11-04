// Attach this script to your ItemDetailsPanel GameObject
using Platformers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Platformers {
    public class DropItUI : MonoBehaviour
    {
        [Header("UI References")]
        public Image itemIcon;
        public TMP_Text itemNameText;
        public TMP_Text itemDescriptionText; // You'll need to add a 'description' field to your Item SO
        public Button useButton;
        public Button closeButton;

        public Item currentItem; // Store the item we are currently displaying

        private void Awake()
        {
            // Add a listener to the close button so it calls the Hide() method
            closeButton.onClick.AddListener(Hide);
        }

        /// <summary>
        /// This is the main public method to populate the panel with data from an Item.
        /// </summary>
        public void Show(Item item)
        {
            currentItem = item;

            // Update the UI elements with the provided item's data
            itemIcon.sprite = currentItem.image;
            itemNameText.text = currentItem.ID;
            // itemDescriptionText.text = currentItem.description; // Example

            // --- Handle the "Use" Button ---
            // First, remove any old listeners to prevent bugs from previous items
            useButton.onClick.RemoveAllListeners();
            // Then, add a new listener that calls the OnUseButtonPressed method for the current item
            useButton.onClick.AddListener(OnUseButtonPressed);

            // Example: Only show the "Use" button if the item is a consumable
            // useButton.gameObject.SetActive(currentItem.isConsumable);

            // Finally, make the entire panel visible
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Hides the panel.
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
            currentItem = null;
        }

        private void OnUseButtonPressed()
        {
            if (currentItem != null)
            {
                Debug.Log($"Using item: {currentItem.ID}");
                // Here, you would call your central InventoryManager to handle the item logic
                // e.g., InventoryManager.Instance.UseItem(currentItem);

                // Hide the panel after using the item
                Hide();
            }
        }
    }
}