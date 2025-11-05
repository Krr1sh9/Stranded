//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Platformers
//{
    


//    public class InventoryUI : MonoBehaviour
//    {
//        public bool inventoryOpen = false;  
//        public bool InventoryOpen => inventoryOpen;
//        public GameObject inventoryPanel;
//        public GameObject inventoryTab;
//        public GameObject craftingTab;
//        // Start is called before the first frame update
//        void Start()
//        {
        
//        }

//        // Update is called once per frame
//        void Update()
//        {
//            if (Input.GetKeyDown(KeyCode.I))
//            {
//                if (inventoryOpen) {

//                    CloseInventory();
//                } else {
//                    OpenInventory();


//                }

//        }

//        public void OpenInventory()
//        {
//            ChangeCursorState(false);
//            inventoryOpen = true;
//            gameObject.SetActive(inventoryOpen);
//            //ShowInventoryTab();
//        }

//        public void CloseInventory()
//        {
//            ChangeCursorState(true);
//            inventoryOpen = false;
//            gameObject.SetActive(inventoryOpen);
//        }

//        public void OnCraftingTabClicked() {
//            craftingTab.SetActive(true);
//            inventoryTab.SetActive(false);


//        }

//        public void OnInventoryTabClicked() { 
//            craftingTab.SetActive(false);
//            inventoryTab.SetActive(true);
//        }

//        public void ChangeCursorState(bool lockCursor) {
//            if (lockCursor)
//            {
//                Cursor.lockState = CursorLockMode.Locked;
//                Cursor.visible = false;

//            }
//            else { 
//                Cursor.lockState = CursorLockMode.None;
//                Cursor.visible = true;

//            }
        
//        }
//    }
//}
