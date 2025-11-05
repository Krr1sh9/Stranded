using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformers
{
    public class HealthSystem : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private int health = 100;

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
        private void Die()
        {
            // Handle death logic here
            Debug.Log($"{gameObject.name} has died.");
            Destroy(gameObject);
        }


    }
}
