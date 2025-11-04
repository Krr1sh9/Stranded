using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformers
{
    public class InventoryManager : MonoBehaviour
    {


        // Start is called before the first frame update

        public InventorySlot[] inventorySlots;
        public GameObject inventoryPrefab;
        public int maxItemCount = 4;
        int selectedSlot = -1;
        int playerGold = 100;
        private Camera mainCamera;
        [SerializeField] private Transform weaponHolder;

        [SerializeField] private InputActionAsset Hotbar;

        [HideInInspector] private InputAction hotbar1Action;
        [HideInInspector] private InputAction hotbar2Action;
        [HideInInspector] private InputAction hotbar3Action;
        [HideInInspector] private InputAction hotbar4Action;
        [HideInInspector] private InputAction hotbar5Action;
        private GameObject currentWeaponObject = null;
        [SerializeField] private WeaponSwitching weaponSwitcher;




        public void Awake()
        {
            hotbar1Action = Hotbar.FindActionMap("UI").FindAction("Hotbar");
            hotbar2Action = Hotbar.FindActionMap("UI").FindAction("Hotbar1");
            hotbar3Action = Hotbar.FindActionMap("UI").FindAction("Hotbar2");
            hotbar4Action = Hotbar.FindActionMap("UI").FindAction("Hotbar3");
            hotbar5Action = Hotbar.FindActionMap("UI").FindAction("Hotbar4");



            Debug.Log("Hotbar Action: " + hotbar1Action);
            mainCamera = Camera.main;

        }

        private void Start()
        {

            ChangeSelectedSlot(0);
        }

        public void OnEnable()
        {
            // Enable each action and subscribe a method to its 'performed' event
            hotbar1Action?.Enable();
            hotbar2Action?.Enable();
            hotbar3Action?.Enable();
            hotbar4Action?.Enable();
            hotbar5Action?.Enable();
            hotbar1Action.performed += ctx => ChangeSelectedSlot(0);
            hotbar2Action.performed += ctx => ChangeSelectedSlot(1);
            hotbar3Action.performed += ctx => ChangeSelectedSlot(2);
            hotbar4Action.performed += ctx => ChangeSelectedSlot(3);
            hotbar5Action.performed += ctx => ChangeSelectedSlot(4);
            

            // ...subscribe more if needed
        }

        public void OnDisable()
        {
            // IMPORTANT: Unsubscribe from events to prevent errors
            hotbar1Action.performed -= ctx => ChangeSelectedSlot(0);
            hotbar2Action.performed -= ctx => ChangeSelectedSlot(1);
            hotbar3Action.performed -= ctx => ChangeSelectedSlot(2);
            hotbar4Action.performed -= ctx => ChangeSelectedSlot(3);
            hotbar5Action.performed -= ctx => ChangeSelectedSlot(4);    

            hotbar1Action?.Disable();
            hotbar2Action?.Disable();
            hotbar3Action?.Disable();
            hotbar4Action?.Disable();
            hotbar5Action?.Disable();


        }


        

        private void OnHotbar(InputAction.CallbackContext context)
        {
            // Read the axis value as a float (e.g., 1.0, 2.0, etc.).
            Debug.Log(context.ReadValue<float>());
            float value = context.ReadValue<float>();

            // Convert the float to an integer.
            int numberPressed = (int)value;

            Debug.Log($"Hotbar key {numberPressed} was pressed!");

            // Convert the 1-based number to a 0-based array index.
            int slotIndex = numberPressed - 1;

            // Check if the index is valid for our array.
            if (slotIndex >= 0 && slotIndex < inventorySlots.Length)
            {
                ChangeSelectedSlot(slotIndex);
            }
        }

        public void ChangeSelectedSlot(int newValue)
        {
            // --- 1. Handle UI Highlighting ---
            if (selectedSlot >= 0 && selectedSlot < inventorySlots.Length)
            {
                inventorySlots[selectedSlot].Deselect();
            }

            inventorySlots[newValue].Select();
            selectedSlot = newValue;
            Debug.Log("Hotbar slot " + (newValue + 1) + " selected.");

            // --- 2. Destroy the Old Weapon ---
            // Instead of destroying a component, we destroy the entire GameObject.
            if (currentWeaponObject != null)
            {
                Destroy(currentWeaponObject);
                currentWeaponObject = null;
            }

            // --- 3. Instantiate the New Weapon ---
            // Get the item from the newly selected UI slot.
            InventoryItem itemInSlot = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>();

            if (weaponSwitcher != null)
            {
                // We tell the WeaponSwitching script to select the weapon
                // at the same index as our newly selected UI slot.
                weaponSwitcher.SelectWeapon(selectedSlot);
            }
            else
            {
                Debug.LogWarning("WeaponSwitching reference is not set on the InventoryManager!");
            }
        }
        public void BuyItem(Item item)
        {
            if (playerGold >= item.price)
            {
                playerGold -= item.price;
                AddItems(item);
                Debug.Log("Bought " + item.itemName + ". Remaining gold: " + playerGold);
                // Here you would update the UI
            }
            else
            {
                Debug.Log("Not enough gold to buy " + item.itemName);
            }
        }
        public bool AddItems(Item item)
        {

            for (int i = 0; i < inventorySlots.Length; i++)
            {
                InventoryItem inventorySlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();
                if (inventorySlot != null && inventorySlot.item == item && inventorySlot.count < maxItemCount && inventorySlot.item.stackable == true)
                {
                    inventorySlot.count++;
                    inventorySlot.RefreshCount();
                    return true;
                }
            }


            for (int i = 0; i < inventorySlots.Length; i++)
            {
                InventorySlot slot = inventorySlots[i];
                InventoryItem inventoryItem = slot.GetComponentInChildren<InventoryItem>();
                if (inventoryItem == null)
                {
                    SpawnNewItem(item, slot);
                    return true;
                }
            }
            return false;

        }

        public void SpawnNewItem(Item item, InventorySlot slot)
        {
            GameObject newItem = Instantiate(inventoryPrefab, slot.transform);
            InventoryItem items = newItem.GetComponent<InventoryItem>();
            Debug.Log(items);
            items.InitialiseItem(item);
        }
        public Item GetSelectedItem(bool use)
        {
            InventorySlot slot = inventorySlots[selectedSlot];
            InventoryItem item = slot.GetComponentInChildren<InventoryItem>();
            if (item != null)
            {
                Item newItem = item.item;
                if (use == true)
                {
                    item.count--;
                    if (item.count <= 0)
                    {
                        Destroy(item.gameObject);
                    }
                    else
                    {
                        item.RefreshCount();
                    }
                }

            }
            return null;


        }

        
    }
}
