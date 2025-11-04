//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//namespace Platformers
//{
//    public class ItemSlot : MonoBehaviour
//    {

//        public Image icon;
//        public ItemsSO item;

//        public void AddItem(ItemsSO newItem)
//        {
//            item = newItem;
//            icon.sprite = item.icon;
//            icon.enabled = true;
//        }

//        //public void UseItem() {
//        //    if (item != null) {
//        //        item.
            
//        //    }
//        //}
//        // Start is called before the first frame update
//        void Start()
//        {
        
//        }

//        public void OnDrop(PointerEventData eventData) {

//            if (eventData.pointerDrag != null) {
//                eventData.pointerDrag.GetComponent<InventoryItemButton>().anchored = this.transform;
//            }
        
//        }

//        // Update is called once per frame
//        void Update()
//        {
        
//        }
//    }
//}
