using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace Platformers
{
    public class Script : MonoBehaviour,IBeginDragHandler,IDragHandler, IEndDragHandler
    {
        // Start is called before the first frame update
        [Header("UI")]
        public Image image;
        [HideInInspector] public Transform parentAfterDrag;
        public void OnBeginDrag(PointerEventData eventData)
        {
            image.raycastTarget = false;
            parentAfterDrag = transform.parent;
            Debug.Log(parentAfterDrag.position);
            transform.SetParent(transform.root);
        }
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
          
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            image.raycastTarget = true;
            Debug.Log(parentAfterDrag.position);
            if ((image.transform.position.x >= 31 && image.transform.position.x <= 51) && (image.transform.position.y >= -31 && image.transform.position.x <= -47)) {
                const float scale = 39.99995f;
                image.transform.position = new Vector3(scale, -39, 0);
            }
            
            transform.SetParent(parentAfterDrag);

        }
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
