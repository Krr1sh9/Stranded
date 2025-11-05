using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformers
{
    public class Projectile : MonoBehaviour
    {
        public int damage = 10; // The amount of damage the projectile will do

        // OnCollisionEnter is called by Unity's physics engine when this object's collider
        // makes contact with another object's collider.
        //private void OnCollisionEnter(Collision collision)
        //{
        //    Debug.Log("Projectile collided with: " + collision.gameObject.name);

        //    // This checks if the object we physically collided with is the player.
        //    // This is still a good check to have, so we don't try to apply damage when hitting a wall.
        //    if (collision.gameObject.CompareTag("FPSController"))
        //    {
        //        Debug.Log("Collision was with the player.");

        //        // --- MODIFICATION: Find the HealthManager via GameObject.Find ---

        //        // 1. Find the GameObject named "FPSController" in the scene.
        //        GameObject healthObject = GameObject.Find("HealthManager");

        //        // 2. Check if we actually found it.
        //        if (healthObject != null)
        //        {
        //            // 3. Get the HealthManager component from the found object.
        //            HealthManager playerHealth = healthObject.GetComponent<HealthManager>();

        //            // 4. If the component exists, deal damage.
        //            if (playerHealth != null)
        //            {
        //                playerHealth.TakeDamage(damage);
        //            }
        //            else
        //            {
        //                Debug.LogError("Found the 'FPSController' object, but it's missing a HealthManager component!");
        //            }
        //        }
        //        else
        //        {
        //            Debug.LogError("Could not find any GameObject named 'FPSController' in the scene!");
        //        }
        //        // --- END OF MODIFICATION ---
        //    }

        //    // After colliding with anything, destroy the projectile.
        //    Destroy(gameObject);
        //}
    }
}
