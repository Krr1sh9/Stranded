//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Platformers
//{
//    public class DemoScript : MonoBehaviour
//    {

//        public InventoryManager inventoryManager;
//        public Item[] items;
//        // Start is called before the first frame update
//        public void PickupItme(int id)
//        {
//            bool result = inventoryManager.AddItems(items[id]);
//            if (result) {
//                Debug.Log("Item added to inventory");
//            } else {
//                Debug.Log("Item could not be added to inventory");
//            }

//        }
//        public void GetSelectedItem() { 
//            Item receivedItem = inventoryManager.GetSelectedItem(false);
//            if (receivedItem != null) {
//                Debug.Log("Selected item is: " + receivedItem);
//            } else {
//                Debug.Log("No item selected");
//            }


//        }
//        public void UseSelectedItem() { 
//            Item receivedItem = inventoryManager.GetSelectedItem(true);
//            if (receivedItem != null) {
//                Debug.Log("Used item: " + receivedItem);
//            } else {
//                Debug.Log("No item selected");
//            }
//        }
 
//    }
//}
