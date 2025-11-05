using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Platformers
{
    public class CraftingItem : MonoBehaviour
    {
        // Start is called before the first frame update

        [Header("UI")]
        public Image image;
        public TextMeshProUGUI countText;
        public InventoryManager inventoryManager;


        [HideInInspector] public Transform parentAfterDrag;
        [HideInInspector] public int count = 1;
        [HideInInspector] public Item item;

       
        public void RefreshCount()
        {
            countText.text = count.ToString();
            bool isActive = count > 1;
            countText.gameObject.SetActive(isActive);
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            image.raycastTarget = false;
            parentAfterDrag = transform.parent;

            transform.SetParent(transform.root);

        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            image.raycastTarget = true;

            transform.SetParent(parentAfterDrag);



        }

        

        
    }
}
