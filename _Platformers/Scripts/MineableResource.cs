// MineableResource.cs
using UnityEngine;
using static UnityEditor.Progress;

namespace Platformers 
{

    public class MineableResource : MonoBehaviour
    {
        [Header("Resource Stats")]
        public float health = 50f; // How many hits it can take

        [Header("Loot Drops")]
        public GameObject lootPrefab; // The item that will spawn when destroyed
        public int lootAmount = 3;   // How many items to spawn

        [SerializeField] private FloatingHealthbar healthbar;

        private FirstPersonController player;
        public GameObject droppedItemPrefab;
        public Item item; // The item data to assign to the dropped loot

        // This is a public method that other scripts (like the player) can call.

        private void Awake()
        {
            healthbar = GetComponentInChildren<FloatingHealthbar>();
            player = GameObject.Find("FPSController").GetComponent<FirstPersonController>();
        }
        public void TakeDamage(float damageAmount)
        {
            // Subtract the damage from our health
            health -= damageAmount;
            healthbar.UpdateHealthbar(health, 50f);

            Debug.Log(gameObject.name + " took " + damageAmount + " damage. Health is now " + health);

            // Check if the resource should be destroyed
            if (health <= 0)
            {
                DestroyResource();
            }
        }

        private void DestroyResource()
        {
            Debug.Log(gameObject.name + " destroyed!");

            // Spawn the loot
            if (droppedItemPrefab != null)
            {
                for (int i = 0; i < lootAmount; i++)
                {
                    // Instantiate the loot prefab at the resource's position with no rotation.
                    // We can add a small random offset to make the drops scatter.
                    Debug.Log("Dropping Item: ");
                    // Define the offset (e.g., slightly in front of the player, adjusted for 3D)
                    // You might want this to be a public variable for easy tweaking in the Inspector
                    Vector3 dropOffset = player.transform.forward * 4.5f + player.transform.up * 2f; // 1.5 units forward, 0.5 units up


                    // Calculate the final drop position
                    Vector3 targetDropPosition = player.transform.position + dropOffset;
                    Debug.Log("Drop Position: " + targetDropPosition);
                    // Instantiate the dropped item (which should have the ItemObject script attached)
                    GameObject droppedGameObject = Instantiate(droppedItemPrefab, targetDropPosition, Quaternion.identity);
                    Debug.Log("Dropped GameObject: " + droppedGameObject.transform.position);

                    // Get the ItemObject component from the instantiated GameObject
                    ItemObjects itemObjectComponent = droppedGameObject.GetComponent<ItemObjects>();

                    // Check if ItemObject component exists, which it should if the prefab is set up correctly
                    if (droppedGameObject.TryGetComponent<ItemObjects>(out _))
                    {
                        // Pass the item data from this InventoryItem to the new ItemObject
                        itemObjectComponent.SetItem(item, targetDropPosition);
                        itemObjectComponent.position = targetDropPosition;
                        // Use the new SetItem method

                        if (droppedGameObject.TryGetComponent<Rigidbody>(out var rb))
                        {
                            // Add force relative to player's forward direction
                            rb.AddForce(player.transform.forward * 5f, ForceMode.VelocityChange); ; // Increased force for more noticeable effect
                        }
                    }
                    else
                    {
                        Debug.LogError("Dropped Item Prefab is missing an ItemObject component!", droppedGameObject);
                    }

                }
            }

            // Destroy this resource GameObject
            Destroy(gameObject);
        }
    }


}