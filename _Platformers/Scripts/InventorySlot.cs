using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Platformers
{
    public class InventorySlot : MonoBehaviour,IDropHandler
    {
        public Image image;
        public Color selectedColor, notSelectedColor;
        private CharacterStatss characterStatss;
       

        private void Awake()
        {
            Deselect();
            characterStatss = GameObject.Find("Characters").GetComponent<CharacterStatss>();
        }

        public void Select()
        {
            image.color = selectedColor;

            
        }
        public void OnClick(PointerEventData eventData)
        {
            Debug.Log("Clicked on item: ");
        }


        public void Deselect()
        {
            image.color = notSelectedColor;
        }


        // Start is called before the first frame update
        public void OnDrop(PointerEventData eventData)
        {
            characterStatss.BaseDefense = 0;
            characterStatss.BaseStrength = 0;   
            characterStatss.StrengthText.text = $"Strength: {characterStatss.BaseStrength}";
            characterStatss.DefenseText.text = $"Defense: {characterStatss.BaseDefense}";

            if (transform.childCount == 0) {

                InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
                item.parentAfterDrag = transform;
            }
        }
    }
}
