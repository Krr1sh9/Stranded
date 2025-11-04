using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Platformers
{
    public class SwordCollisionDetector : MonoBehaviour
    {
        // --- REMOVED --- The sword itself doesn't have health. The things it hits do.
        // public int health = 100; 

        [Header("Combat Stats")]
        public int attackDamage = 10;

        [Header("Mining Stats")]
        public int miningDamage = 5; // NEW: Damage dealt to resources

        // References
        public WeaponController weaponController;
        private CharacterStatss characterStats;
        [SerializeField] private MineableResource mineable;

        public void Start()
        {
            
            // Ensure you have a "Characters" GameObject with the CharacterStatss script
            GameObject charactersGO = GameObject.Find("Characters");
            if (charactersGO != null)
            {
                characterStats = charactersGO.GetComponent<CharacterStatss>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // --- 1. CHECK FOR MINING ---
            // We check for mining first. If the pickaxe hits a rock, it shouldn't also try to damage it as an enemy.
            if (other.CompareTag("Mineable"))
            {
                Debug.Log("SwordCollisionDetector initialized.");
                Debug.Log("Mining tool hit a mineable resource: " + other.name);

                // Get the MineableResource component from the object we hit.
                if (other.TryGetComponent<MineableResource>(out var resource))
                {
                    // Tell the RESOURCE to take damage.
                    resource.TakeDamage(miningDamage);
                }
            }

            // --- 2. CHECK FOR ENEMY ATTACK ---
            else if (other.CompareTag("Enemy") && weaponController.isAttacking)
            {
                Debug.Log("Sword hit an enemy: " + other.name);

                // CORRECTED LOGIC: Get the health component FROM THE ENEMY.
                if (other.TryGetComponent<EnemyAI>(out var enemy))
                {
                    // Tell the ENEMY to take damage.
                    int totalDamage = attackDamage + (characterStats != null ? characterStats.BaseStrength : 0);
                    enemy.TakeDamage(totalDamage);
                }
            }

            // --- 3. CHECK FOR DEFENDING ---
            // Note: You might want a different tag for projectiles, like "EnemyProjectile"
            //else if (other.CompareTag("EnemyProjectile") && weaponController.isDefending)
            //{
            //    // Handle projectile deflection logic here
            //    Debug.Log("Projectile deflected by sword!");
            //    Destroy(other.gameObject); // Destroy the projectile
            //}
        }
    }
}

// NOTE: You will need an EnemyHealth script on your enemies for the corrected logic to work.
// Example EnemyHealth.cs:
/*
using UnityEngine;
public class EnemyHealth : MonoBehaviour {
    public int health = 100;
    public void TakeDamage(int damage) {
        health -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage. Health: " + health);
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}
*/