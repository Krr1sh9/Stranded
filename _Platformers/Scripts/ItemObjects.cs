using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Platformers
{
    public class ItemObjects : MonoBehaviour
    {
        // Start is called before the first frame update
        public Item item;

        public Sprite image;

        public Vector3 position;

        public InventoryManager inventoryManager;

        




        // Optional: Reference to a Rigidbody if you want physics effects
        private Rigidbody rb;

        // Optional: Collider for interaction (e.g., player picking it up)
        private Collider col;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>(); // Or Collider2D if 2D project
            // Ensure you have a SpriteRenderer if using it, or get it dynamically
            // spriteRenderer = GetComponent<SpriteRenderer>(); // Uncomment if using SpriteRenderer
        }

        // Call this after instantiating to set up the ItemObject
        public void SetItem(Item newItem,Vector3 positions)
        {
            item = newItem;
            image = item.image;
            position = positions;
            // You could also set up a 3D model if 'item' had a reference to one
        }

        // Example: When the player (or something else) collides with it to pick it up
        //private void OnTriggerEnter(Collider other) // Or void OnTriggerEnter2D(Collider2D other) for 2D
        //{
        //    // You'd add logic here for picking up the item
        //    // For example:
        //    if (other.CompareTag("FPSController"))
        //    {




        //        Debug.Log("Picking up item");
        //        Debug.Log(other.na);

        //        inventoryManager.AddItems(item);

        //        // Add item to player's inventory
        //        Destroy(gameObject);
        //    }
        //}
    }
}
