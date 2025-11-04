using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Removed: using UnityEngine.UIElements; // Not needed for UI components

namespace Platformers
{
    public class ShopManager : MonoBehaviour
    {
        public int[,] shopItems = new int[5, 5];
        public float coins;
        public TextMeshProUGUI coinsTxt;

        // Assign this in the Inspector!
        [SerializeField] private FirstPersonController playerController;
        public ScrollRect itemScrollView;

        public static bool isShopOpen = false;
        public InventoryManager inventoryManager;
        public Button closeButton;

        void Awake()
        {
            // If playerController is not assigned in Inspector, try to find it.
            // This is safer than relying on GetComponent directly in Awake/Start if it's on a different GameObject.
            if (playerController == null)
            {
                playerController = GameObject.Find("FPSController").GetComponent<FirstPersonController>();
                if (playerController == null)
                {
                    Debug.LogWarning("ShopManager: FirstPersonController not found in the scene. Player controls may not be managed correctly by the shop.", this);
                }
            }

            if (itemScrollView != null)
            {
                itemScrollView.gameObject.SetActive(false); // Ensure shop is closed at start
            }
            else
            {
                Debug.LogError("ShopManager: itemScrollView is not assigned!", this);
                enabled = false; // Disable script if critical component is missing
            }

            if (coinsTxt == null)
            {
                Debug.LogError("ShopManager: coinsTxt is not assigned!", this);
                // Don't disable, but text won't update
            }
            if (inventoryManager == null)
            {
                // This might be found dynamically if it's not assigned in the Inspector
                inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
                if (inventoryManager == null)
                {
                    Debug.LogError("ShopManager: InventoryManager not assigned and not found in scene!", this);
                }
            }
        }


        void Start()
        {
            UpdateCoinsText();

            shopItems[1, 0] = 0;
            shopItems[1, 1] = 1; // Item IDs or types
            shopItems[1, 2] = 2; // Fixed incorrect indexing from original code
            shopItems[1, 3] = 3;
            shopItems[1, 4] = 4;
            
            shopItems[2, 0] = 5;
            shopItems[2, 1] = 10; // Prices
            shopItems[2, 2] = 20;
            shopItems[2, 3] = 30; // Corrected original code (was [3,3] and [4,4])
            shopItems[2, 4] = 40;

            shopItems[3, 0] = 0;
            shopItems[3, 1] = 0; // Quantities (player's owned quantity, or shop's stock)
            shopItems[3, 2] = 0;
            shopItems[3, 3] = 0;
            shopItems[3, 4] = 0;
        }

        void UpdateCoinsText()
        {
            if (coinsTxt != null)
            {
                coinsTxt.text = "Coins: " + coins.ToString(); // Changed "coins:" to "Coins:" for consistency
            }
        }

        public void Buy()
        {
            // Defensive check for EventSystem and ButtonRef
            if (UnityEngine.EventSystems.EventSystem.current == null)
            {
                Debug.LogError("ShopManager: EventSystem.current is null! Cannot process buy.", this);
                return;
            }

            GameObject ButtonRef = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            if (ButtonRef == null)
            {
                Debug.LogWarning("ShopManager: Buy called but no button is currently selected in EventSystem.", this);
                return;
            }

            ButtonInfo buttonInfo = ButtonRef.GetComponent<ButtonInfo>();
            if (buttonInfo == null)
            {
                Debug.LogError("ShopManager: Selected GameObject for Buy does not have a ButtonInfo component.", ButtonRef);
                return;
            }

            int itemID = buttonInfo.ItemID;
            float itemPrice = shopItems[2, itemID]; // Using itemID for price

            if (coins >= itemPrice)
            {
                coins -= itemPrice;
                shopItems[3, itemID] += 1; // Increment quantity
                Debug.Log($"Bought ItemID {itemID}. New quantity: {shopItems[3, itemID]}");

                if (inventoryManager != null)
                {
                    inventoryManager.AddItems(buttonInfo.item);
                }
                else
                {
                    Debug.LogWarning("ShopManager: InventoryManager is null, cannot add item to inventory.", this);
                }

                UpdateCoinsText();
                buttonInfo.QuantityTxt.text = shopItems[3, itemID].ToString();
            }
            else
            {
                Debug.Log("Not enough coins to buy ItemID " + itemID);
            }
        }

        public void OpenShop()
        {
            if (itemScrollView != null) itemScrollView.gameObject.SetActive(true);

            if (playerController != null)
            {
                playerController.SetControlsEnabled(false); // Correct: Only disable controls once
            }


            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            isShopOpen = true;
            Debug.Log("Shop opened. Player controls disabled, cursor unlocked.");
        }

        public void CloseShop()
        {
            if (itemScrollView != null) itemScrollView.gameObject.SetActive(false);

            if (playerController != null)
            {
                playerController.SetControlsEnabled(true); // Correct: Only enable controls once
            }

            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            isShopOpen = false;
            closeButton.gameObject.SetActive(false);
            
            Debug.Log("Shop closed. Player controls enabled, cursor locked.");
        }

        public void ShowCoinsText()
        {
            if (coinsTxt != null)
            {
                coinsTxt.gameObject.SetActive(true);
            }
        }

        public void HideCoinsText()
        {
            if (coinsTxt != null)
            {
                coinsTxt.gameObject.SetActive(false);
            }
        }

        public void ToggleCoinsText()
        {
            if (coinsTxt != null)
            {
                bool isActive = coinsTxt.gameObject.activeSelf;
                coinsTxt.gameObject.SetActive(!isActive);
            }
        }
    }
}