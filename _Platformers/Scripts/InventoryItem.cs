using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace Platformers
{
    public class InventoryItem : MonoBehaviour,IBeginDragHandler, IEndDragHandler,IDragHandler,IPointerClickHandler
    {

        // Start is called before the first frame update

    

        [Header("UI")]
        public Image image;
        public TextMeshProUGUI countText;
        public InventoryManager inventoryManager;


        [HideInInspector]public Transform parentAfterDrag;
        [HideInInspector] public int count = 1;
        [HideInInspector] public Item item;
        public InputAction mouse;
        public GameObject healths;
        public GameObject hungers;
        private HealthManager healthManager;
        private StaminaManager staminaManager;
        private GameObject player;
        public GameObject droppedItemPrefab;
        private CharacterStatss characterStatss;
       

        public void OnEnable()
        {
         
            mouse.Enable();
        
        }
        public void OnDisable()
        {
         
            mouse.Disable();

        }

        public void Awake()
        {
            healths = GameObject.Find("HealthManager");
            hungers = GameObject.Find("StaminaManager");
            healthManager = healths.GetComponent<HealthManager>();
            staminaManager = hungers.GetComponent<StaminaManager>();
            player = GameObject.Find("FPSController");
            characterStatss = GameObject.Find("Characters").GetComponent<CharacterStatss>();

        }




        public void InitialiseItem(Item newItem)
        {
            item = newItem;
            image.sprite = newItem.image;
            image.color = new Color(9, 95, 154, 400);
            RefreshCount();


        }

        

        public void RefreshCount()
        {
            countText.text = count.ToString();
            bool isActive = count > 1;
            countText.gameObject.SetActive(isActive);
        }
        public void OnBeginDrag(PointerEventData eventData)
        {

            Debug.Log("Beginning to drag item: " + item.itemName);
            // Store the original parent before detaching
            parentAfterDrag = transform.parent;

            // Try to get the EquipmentSlot component from our original parent.
            EquipmentSlot startingSlot = parentAfterDrag.GetComponent<EquipmentSlot>();


            // If we successfully found a component, it means we are unequipping.
            if (startingSlot != null)
            {
                Debug.Log("Unequipping item by dragging it out of an equipment slot.");

                // This tells the system to recalculate stats now that this slot is empty.
                startingSlot.UpdateEquipmentStats();
            }

            // Detach from parent to drag freely
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            image.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            image.raycastTarget = true;

            transform.SetParent(parentAfterDrag);

            
            
        }

        public void OnClick(PointerEventData eventData)
        {
            Debug.Log("Clicked on item: ");
        }

        public void OnPointerClick(PointerEventData eventData)
        {

            if (eventData.button == PointerEventData.InputButton.Right)
            {

                if (FirstPersonController.isInventoryOpen) {
                    UseSelectedItem();
                }
                
                

            }
            else if (eventData.button == PointerEventData.InputButton.Middle)
            {

                if (FirstPersonController.isInventoryOpen) {
                    DropItem();
                }
            }

            // We only want to process clicks, not drags.
            // The 'dragging' flag is set by the Event System.
            //if (eventData.dragging) return;

            //// If an item exists and someone is listening...
            //if (item != null)
            //{
            //    // ...fire the event and pass our 'item' data as the message.
            //    OnItemClicked?.Invoke(item);
            //}


        }
        //public void DropItem() {
        //    if (Keyboard.current.rKey.wasPressedThisFrame) {
        //        Vector2 offset = new Vector2(1f, 0f);

        //        GameObject droppedItem = Instantiate(inventoryManager.inventoryPrefab, player.transform.position, Quaternion.identity);
        //        ItemObject itemObject = droppedItem.GetComponent<ItemObject>();
        //        itemObject.item = item;
        //        itemObject.position = player.transform.position + (Vector3)offset;
        //        Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
        //        rb.AddForce(player.transform.forward * 2f, ForceMode.VelocityChange);
        //        count--;
        //        if (count <= 0)
        //        {
        //            Destroy(gameObject);
        //        }
        //        else
        //        {
        //            RefreshCount();
        //        }


        //    }
        
        //}

        public void DropItem()
        {
   
            
                Debug.Log("Dropping Item: ");
                // Define the offset (e.g., slightly in front of the player, adjusted for 3D)
                // You might want this to be a public variable for easy tweaking in the Inspector
                Vector3 dropOffset = player.transform.forward * 4.5f + player.transform.up * 2f; // 1.5 units forward, 0.5 units up


                // Calculate the final drop position
                Vector3 targetDropPosition = player.transform.position + dropOffset;
                Debug.Log("Drop Position: " + targetDropPosition);
            // Instantiate the dropped item (which should have the ItemObject script attached)
            GameObject droppedGameObject = Instantiate(droppedItemPrefab, targetDropPosition, Quaternion.identity);
            Debug.Log("Dropped GameObject: " + droppedGameObject.transform.position);

            // Get the ItemObject component from the instantiated GameObject
            ItemObjects itemObjectComponent = droppedGameObject.GetComponent<ItemObjects>();

            // Check if ItemObject component exists, which it should if the prefab is set up correctly
            if (droppedGameObject.TryGetComponent<ItemObjects>(out var itemObject))
                {
                    // Pass the item data from this InventoryItem to the new ItemObject
                    itemObjectComponent.SetItem(item,targetDropPosition);
                    itemObjectComponent.position = targetDropPosition;
                // Use the new SetItem method

                if (droppedGameObject.TryGetComponent<Rigidbody>(out var rb))
                {
                    // Add force relative to player's forward direction
                   rb.AddForce(player.transform.forward * 5f, ForceMode.VelocityChange);; // Increased force for more noticeable effect
                }
            }
                else
                {
                    Debug.LogError("Dropped Item Prefab is missing an ItemObject component!", droppedGameObject);
                }


                // Decrement count and refresh UI
                count--;
                if (count <= 0)
                {
                    Destroy(gameObject); // Destroy the UI inventory item slot if empty
                }
                else
                {
                    RefreshCount();
                }
            
        }
       
        public void UseSelectedItem()
        {

            InventorySlot slot = transform.parent.GetComponent<InventorySlot>();
            InventoryItem item = slot.GetComponentInChildren<InventoryItem>();
            if (item != null)
            {
                Item newItem = item.item;


                if (newItem.actionType == ActionType.Consumable) 
                {
                    Debug.Log(healthManager);

                    healthManager.Heal(10);
                    staminaManager.ChangeHunger(10);
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
           
        }



    }
}
