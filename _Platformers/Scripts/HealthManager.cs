using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
namespace Platformers
{
    public class HealthManager : MonoBehaviour
    {
        
        public Image healthBar;
        public float health = 100f;
        public InputAction Heals;
        public InputAction TakeDamages;
        public static event Action PlayerDead;
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Health Manager started with health: " + Heals);
            Debug.Log("TakeDamages Manager started with health: " + TakeDamages);
        }

        // Update is called once per frame

        public void OnEnable()
        {
            Heals.Enable();
            TakeDamages.Enable();
            Heals.performed += UpdateHealth;
            TakeDamages.performed += UpdateHealth;
        }

        public void OnDisable()
        {
            Heals.Disable();
            TakeDamages.Disable();
            Heals.performed -= UpdateHealth;
            TakeDamages.performed -= UpdateHealth;
        }


        public void UpdateHealth(InputAction.CallbackContext context)
        {
            if (context.action == Heals)
            {
                Debug.Log("Heal action performed (from Inspector config)");
                Heal(10);
            }
            else if (context.action == TakeDamages)
            {
                Debug.Log("Take Damage action performed (from Inspector config)");
                TakeDamage(10);
            }
        }


        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0f) {
                health = 0f;
                Debug.Log("Player is dead!");
                PlayerDead?.Invoke();
                healthBar.fillAmount = 0f/ 100f;
            }
            Debug.Log("Player health: " + health);
            healthBar.fillAmount = health / 100f;


        }
        public void Heal(int amount)
        {
            health += amount;
            Debug.Log("Player health: " + health);
            if (health > 100f) health = 100f;
            healthBar.fillAmount = health / 100f;
        }
    }
}
