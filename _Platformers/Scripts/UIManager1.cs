using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Add this namespace

namespace Platformers
{
    public class UIManager : MonoBehaviour
    {
        public GameObject gameOverScreen;

        // Reference to the FirstPersonController to disable player input
        [SerializeField] private FirstPersonController playerController;

        public static bool enables = false;

        private void Awake()
        {
            if (gameOverScreen == null)
            {
      
                enabled = false; // Disable script if critical UI is missing
                return;
            }
            gameOverScreen.SetActive(false);

            // Try to find the player controller if not assigned
            if (playerController == null)
            {
                playerController = GameObject.Find("FPSController").GetComponent<FirstPersonController>();
                if (playerController == null)
                {
                    ;
                }
            }
        }

        public void OnEnable()
        {
            // Subscribe only if HealthManager and StaminaManager are in the scene
            // This prevents issues if these managers are not present.
            
                HealthManager.PlayerDead += ShowGameOverScreen;
            
           
                StaminaManager.PlayerStarved += ShowGameOverScreen;
            
        }

        private void OnDisable()
        {
            // Always unsubscribe to prevent memory leaks and 'destroyed object' calls
            
                HealthManager.PlayerDead -= ShowGameOverScreen;
            
            
                StaminaManager.PlayerStarved -= ShowGameOverScreen;
            
        }

        void ShowGameOverScreen()
        {
            if (gameOverScreen != null) // Check if the UI element still exists
            {
                enables = true;
                gameOverScreen.SetActive(true);
            }
            else
            {
                ;
            }

            // Disable player controls when game over screen appears
            if (playerController != null)
            {
                playerController.SetControlsEnabled(false);
            }

         
            // Implement your game over UI logic here
        }

        public void RestartGame()
        {
        

            // Before loading the scene, ensure player controls are re-enabled (if disabled)
            // Or just let the new scene handle the initial state.
            // A small delay can sometimes help the editor clean up, but often not necessary
            // if all subscriptions are properly handled.
            enables = false;

            // It's generally safe to call LoadScene. The error is likely an editor-related side-effect.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            // Note: Any code after LoadScene will generally NOT execute because the scene is unloaded.
        }
    }
}