//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//using UnityEngine.Events;
//using TMPro;

//namespace Platformers
//{
//    public class CardManager : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
//    {
//        public Item item;
//        public bool isOccupied;

//        [SerializeField] bool useAsDrag;
//        [SerializeField] protected GameObject emptyCard;

//        [SerializeField] protected Image itemImage;
//        [SerializeField] protected TMP_Text itemAmountText;

//        public UnityEvent onItemTaken;

//        public virtual bool SetItem(Item item) { 
//            if (isOccupied && !useAsDrag || item == null) return false;

//            this.item = item;
//            itemImage.sprite = item.image;
            
//            this.item.text = item.itemType.ToString();
//            this.isOccupied = true;

//            RefreshDisplay();

//            return true;

//        }
//        public virtual void UnsetItem()
//        {
//            ResetSlot();
//            onItemTaken?.Invoke();
//        }

//        public virtual void ResetSlot() { 
//            item = null;
            
//            this.isOccupied = false;
//            RefreshDisplay();
//        }

//        protected void RefreshDisplay() { 
        
//            emptyCard.SetActive(!isOccupied);
//        }

//        public void OnPointerDown(PointerEventData eventData)
//        {
//            ;
//        }

//        public void OnPointerEnter(PointerEventData eventData)
//        {
//            ;
//        }

//        public void OnPointerExit(PointerEventData eventData)
//        {
//            ;
//        }
//    }
//}
