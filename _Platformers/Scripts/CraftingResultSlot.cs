using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformers
{
    public class CraftingResultSlot : MonoBehaviour
    {

        public GameObject itemPrefab;
        // Start is called before the first frame update
        public void DisplayItem(Item item)
        {
            GameObject itemObj = Instantiate(itemPrefab, transform);

            itemObj.transform.SetParent(transform);

            InventoryItem item1 = itemObj.GetComponent<InventoryItem>();
            item1.InitialiseItem(item);

        }

        // Update is called once per frame
        public void ClearSlot()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
