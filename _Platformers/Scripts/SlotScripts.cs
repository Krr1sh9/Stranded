using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Platformers
{
    public class SlotScripts : MonoBehaviour,IDropHandler
    {
        // Start is called before the first frame update

        public void OnDrop(PointerEventData eventData) {
            if (transform.childCount == 0) { 
                Script item = eventData.pointerDrag.GetComponent<Script>();
                item.parentAfterDrag = transform;
            }
        
        }
        
    }
}
