using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Platformers
{
    public class CraftingSlots : MonoBehaviour, IDropHandler
    {
        // Start is called before the first frame update
        public Image image;

        private CharacterStatss characterStatss;

        private void Awake()
        {
            characterStatss = GameObject.Find("Characters").GetComponent<CharacterStatss>();
        }

        // Start is called before the first frame update
        public void OnDrop(PointerEventData eventData)
        {
            characterStatss.BaseDefense = 0;
            characterStatss.BaseStrength = 0;
            characterStatss.StrengthText.text = $"Strength: {characterStatss.BaseStrength}";
            characterStatss.DefenseText.text = $"Defense: {characterStatss.BaseDefense}";
            if (transform.childCount == 0)
            {
                InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
                item.parentAfterDrag = transform;
            }
        }
    }
}
