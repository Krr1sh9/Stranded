using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // Required for Image and Slider components
using System;
namespace Platformers
{
    public class StaminaManager : MonoBehaviour
    {
        [Header("UI")]
        public Image hungerBar; // Assign your UI Image here

        [Header("Hunger Settings")]
        [Range(0, 100)] // Clamp hunger value in inspector
        public float hunger = 100f; // Start with full hunger
        public float maxHunger = 100f; // Max hunger value
        public static event Action PlayerStarved;

        [Header("Hunger Drain")]
        public int hungerDrainPerTick = 1; // How much hunger to lose per tick
        public float hungerTickInterval = 2.0f; // How often (in seconds) to lose hunger

        [Header("Health Link (Optional)")]
        // Reference to the player's actual HealthManager
        public HealthManager playerHealthManager;
        public int healthDamagePerTick = 5; // How much actual health to lose when hunger is zero
        public float healthDamageInterval = 1.0f; // How often health is lost when hunger is zero

        [Header("Input Actions")]
        public InputAction rejuvenation; // Assign in Inspector
        public InputAction starvation;   // Assign in Inspector

        private bool isHungerDraining = false; // To control the InvokeRepeating state
        private bool isHealthDraining = false; // To control health drain when hunger is low

        void Awake()
        {
            // Initial setup for input actions if not assigned in Inspector
            // You can uncomment and modify these if you want to hardcode bindings
            // if (rejuvenation == null) rejuvenation = new InputAction("Rejuvenation", type: InputActionType.Button, binding: "<Keyboard>/p");
            // if (starvation == null) starvation = new InputAction("Starvation", type: InputActionType.Button, binding: "<Keyboard>/o");
        }

        void Start()
        {
            hunger = maxHunger; // Ensure hunger starts at max
            UpdateHungerUI(); // Set initial UI state

            // Start the continuous hunger drain
            StartContinuousHungerDrain();

            // Find HealthManager if not assigned (less ideal but works)
            if (playerHealthManager == null)
            {
                playerHealthManager = FindFirstObjectByType<HealthManager>();
                if (playerHealthManager == null)
                {
                    Debug.LogWarning("StaminaManager: No HealthManager found in scene. Health will not be affected when hunger is zero.");
                }
            }
        }

        public void OnEnable()
        {
            rejuvenation.Enable();
            starvation.Enable();
            rejuvenation.performed += UpdateHungerInput; // Renamed to avoid confusion with internal updates
            starvation.performed += UpdateHungerInput;
        }

        public void OnDisable()
        {
            rejuvenation.Disable();
            starvation.Disable();
            rejuvenation.performed -= UpdateHungerInput;
            starvation.performed -= UpdateHungerInput;

            // Stop all repeating invokes when disabled
            CancelInvoke();
            isHungerDraining = false;
            isHealthDraining = false;
        }

        // Renamed from UpdateHunger to avoid confusion with UI/internal updates
        public void UpdateHungerInput(InputAction.CallbackContext context)
        {
            if (context.performed) // Ensure it only triggers once per press
            {
                if (context.action == rejuvenation)
                {
                    ChangeHunger(10); // Use a generic ChangeHunger method
                }
                else if (context.action == starvation)
                {
                    ChangeHunger(-10); // Use a generic ChangeHunger method
                }
            }
        }

        // --- Hunger Drain Logic ---

        public void StartContinuousHungerDrain()
        {
            if (!isHungerDraining)
            {
                InvokeRepeating("ApplyHungerDrainTick", hungerTickInterval, hungerTickInterval);
                isHungerDraining = true;
                Debug.Log("Hunger drain started.");
            }
        }

        void ApplyHungerDrainTick()
        {
            ChangeHunger(-hungerDrainPerTick); // Decrement hunger

            // Check if hunger has dropped to 0 or below
            if (hunger <= 0)
            {
                
                    hunger = 0f;
                    Debug.Log("Player is dead!");
                    PlayerStarved?.Invoke();
                    hungerBar.fillAmount = 0f / 100f;
                
                // If hunger is empty, start damaging health if not already doing so
                if (playerHealthManager != null && !isHealthDraining)
                {
                    InvokeRepeating("ApplyHealthDamageTick", healthDamageInterval, healthDamageInterval);
                    isHealthDraining = true;
                    Debug.Log("Hunger is empty! Starting to lose health.");
                }
            }
            else // If hunger is above 0, ensure health drain is stopped
            {
                if (isHealthDraining)
                {
                    CancelInvoke("ApplyHealthDamageTick");
                    isHealthDraining = false;
                    Debug.Log("Hunger restored, stopped losing health.");
                }
            }
        }

        void ApplyHealthDamageTick()
        {
            if (playerHealthManager != null)
            {
                playerHealthManager.TakeDamage(healthDamagePerTick);
            }
            else
            {
                // If HealthManager is null, stop trying to damage health
                CancelInvoke("ApplyHealthDamageTick");
                isHealthDraining = false;
                Debug.LogWarning("HealthManager is null, cannot apply health damage from starvation.");
            }
        }


        // --- Public Methods for External Use ---

        public void ChangeHunger(float amount)
        {
            hunger += amount;
            hunger = Mathf.Clamp(hunger, 0, maxHunger); // Clamp between 0 and maxHunger
            UpdateHungerUI();
            Debug.Log("Current Hunger: " + hunger);
        }

        // --- UI Update ---

        void UpdateHungerUI()
        {
            if (hungerBar != null)
            {
                hungerBar.fillAmount = hunger / maxHunger;
            }
            else
            {
                Debug.LogWarning("Hunger Bar Image is not assigned in StaminaManager!", this);
            }
        }
    }
}