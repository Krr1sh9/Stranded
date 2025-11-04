using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//namespace Platformers
//{
//    public class EquipmentSlot : MonoBehaviour,IDropHandler
//    {
//        // Start is called before the first frame update
//        public EquipmentType currentItem;

//        public int armourModifier;
//        public int damageModifier;


        

//        public void OnDrop(PointerEventData eventData)
//        {
//            if (transform.childCount == 0)
//            {
//                InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
//                item.parentAfterDrag = transform;
                
//            }
//        }
//    }





//    public enum EquipmentType
//    {

//        Helmet,
//        ChestPlate,
//        Leggings,
//        Boots,
//    }
//}


namespace Platformers
{
    public class EquipmentSlot : MonoBehaviour, IDropHandler
    {
        // We no longer need these variables, as stats are on the item itself
        // public int armourModifier;
        // public int damageModifier;

            // This tells the slot what type of item it can accept
            [SerializeField] private EquipmentType slotType;

        private EquipmentManager equipmentManager;

        void Awake()
        {
            // Find the manager so we can talk to it
            equipmentManager = Object.FindFirstObjectByType<EquipmentManager>();
            if (equipmentManager == null)
            {
                Debug.LogError("EquipmentSlot could not find an EquipmentManager in the scene!");
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            // 1. Get the InventoryItem component from the object being dragged.
            InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();

            // If what we dropped wasn't even an inventory item, do nothing.
            //if (item == null)
            //{
            //    return;
            //}

            // 2. Check if this equipment slot is already full.
            if (transform.childCount > 0)
            {
                Debug.Log("Equipment slot is already occupied. Implement swapping logic here.");
                // The item will automatically snap back to its original parent because we didn't change anything.
                return;
            }

            // 3. The slot is empty, so now we validate the item's type.
            // NOTE: Make sure your Item script has a public variable like 'public EquipmentType equipmentType;'
            if (item.item.equipmentType == this.slotType)
            {
                // --- SUCCESS CASE: The types match! ---
                Debug.Log("Correct item type! Placing item in slot.");

                // A. Immediately move the item's UI into this slot.
                item.transform.SetParent(this.transform);
                item.transform.localPosition = Vector3.zero; // Center it perfectly in the slot.

                // B. Because we have successfully parented it here, we also need to update
                //    'parentAfterDrag' in case the user immediately drags it out again.
                item.parentAfterDrag = this.transform;

                // C. Trigger the stat update/equip logic.
                UpdateEquipmentStats();
            }
            else
            {
                // --- FAILURE CASE: The types do NOT match! ---
                Debug.Log($"Wrong item type! Tried to drop a {item.item.equipmentType} on a {this.slotType} slot.");

                // We do nothing. The 'OnEndDrag' method on the InventoryItem will use its
                // original 'parentAfterDrag' and snap the item back to where it came from.
            }
        }

        //public void OnDrop(PointerEventData eventData)
        //{
        //    InventoryItem droppedItemUI = eventData.pointerDrag.GetComponent<InventoryItem>();
        //    if (droppedItemUI == null) return;

        //    Item itemData = droppedItemUI.item;
        //    if (itemData == null) return;

        //    // Validate that the item type matches the slot type
        //    if (itemData.equipmentType == this.slotType)
        //    {
        //        // If the slot is empty, we can drop the item.
        //        if (transform.childCount == 0)
        //        {
        //            droppedItemUI.parentAfterDrag = transform;
        //            // The item will be parented after the drag ends.
        //            // We call the update method right after the drop.
        //            UpdateEquipmentStats();
        //        }
        //        // NOTE: To handle swapping, you would add more logic here to unequip the existing item.
        //    }
        //}

        // --- THIS IS THE METHOD YOU WANTED TO IMPLEMENT ---
        /// <summary>
        /// Notifies the EquipmentManager to recalculate all player stats.
        /// This should be called whenever an item is equipped or unequipped from this slot.
        /// </summary>
        public void UpdateEquipmentStats()
        {
            // We use a small delay to ensure the UI parenting has finished before we calculate.
            StartCoroutine(DelayedStatUpdate());
        }

        private System.Collections.IEnumerator DelayedStatUpdate()
        {
            // Wait for the end of the frame to ensure the item has been parented correctly in the UI
            yield return new WaitForEndOfFrame();

            if (equipmentManager != null)
            {
                // Tell the manager to do the heavy lifting
                equipmentManager.RecalculateAllStats();
            }
        }
    }

    // Ensure your enum is accessible. It's often best in its own file.
    public enum EquipmentType
    {
        None, // Good to have a default
        Helmet,
        ChestPlate,
        Leggings,
        Boots,
        RightHand,
        LeftHand
    }
}