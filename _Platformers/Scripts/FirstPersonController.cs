using Controller;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting; // Not explicitly used, can remove if not needed
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using static UnityEditor.Progress;
namespace Platformers
{
    public class FirstPersonController : MonoBehaviour
    {
        private CharacterController characterController;
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float mouseSensitivity = 2f;
        [SerializeField] private float verticalRotationLimit = 80f;
        [SerializeField] private float sprintMultiplier = 2f;
        [SerializeField] private float jumpHeight = 5.0f;
        [SerializeField] private float gravity = 9.81f;
        [SerializeField] private InputActionAsset playerActions; // Assign your Input Action Asset here
        private InputAction moveAction;
        private InputAction jumpAction;
        private InputAction sprintAction;
        private InputAction lookAction;
        private InputAction mineAction;
        private Vector2 moveInput;
        private Vector2 lookInput;
        private GameObject inventoryMenu;
        private InputAction inventoryAction;
        private InputAction attackAction;
        private InputAction defendAction;
        private InputAction interaction;
        private InventoryManager inventoryManager;
        private HealthManager healthManager;
        private readonly Animator animator;
        [SerializeField] private ShopManager shopManager;
        [HideInInspector] public static bool isInventoryOpen = false;
        public float mineDistance = 3f; // How far the player can mine
        public LayerMask mineableLayer; // Assign this in the Inspector to only hit mineable objects
        public GameObject mineEffect; // Optional: Particle effect when mining

        private bool canMine = true; // To prevent spamming mining
        public float mineRate = 1f; // How often the player can mine (seconds between mines)
        public Button closeButton;



        private float verticalRotation;
        private Camera mainCamera;
        private Vector3 currentMovement = Vector3.zero;

        public float attackDamage = 1f;
        public float attackDistance = 2f;
        public float attackRate = 1f;
        public float attackDelay = 1f;
        public WeaponController weaponController;

        public LayerMask npcLayer;

        public LayerMask attackLayer;
        public GameObject hitEffect;    

        bool attacking = false;
        bool readyToAttack = true;  
        bool defending = false;
        int attackCount;

        public float interactionDistance = 4f;


        public const string ANIMATION_ATTACK_01 = "Attack_01";
        public const string ANIMATION_ATTACK_02 = "Attack_02";

        string currentAnimationState;


        // >>>>>>>>>>>>>>>>> NEW: Toggle for player and camera control <<<<<<<<<<<<<<<<<
        public bool controlsEnabled = true; // Public boolean to enable/disable controls

        void Awake()
        {
            inventoryMenu = GameObject.Find("MainInvenGroup");
            inventoryMenu.SetActive(false);

            inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
            

            
            shopManager.HideCoinsText();
            shopManager.gameObject.SetActive(false);
            shopManager.CloseShop();

            closeButton.gameObject.SetActive(false);


            characterController = GetComponent<CharacterController>();
            healthManager = GameObject.Find("HealthManager").GetComponent<HealthManager>();

            //if (characterController == null)
            //{
            //    Debug.LogError("The FirstPersonController script on the object '" + gameObject.name + "' is missing a CharacterController component.", this.gameObject);
            //}
            mainCamera = Camera.main; // Make sure your main camera has the "MainCamera" tag

            // Initial cursor state for gameplay
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Ensure playerActions is assigned in the Inspector
            if (playerActions == null)
            {
                Debug.LogError("Player Actions Asset not assigned in FirstPersonController!", this);
                return;
            }
            //shopManager = GameObject.Find("ShopManager").GetComponent<ShopManager>();

            moveAction = playerActions.FindActionMap("Player").FindAction("Move");
            jumpAction = playerActions.FindActionMap("Player").FindAction("Jump");
            sprintAction = playerActions.FindActionMap("Player").FindAction("Sprint");
            lookAction = playerActions.FindActionMap("Player").FindAction("Look");
            attackAction = playerActions.FindActionMap("Player").FindAction("Attack");
            defendAction = playerActions.FindActionMap("Player").FindAction("Defend");
            mineAction = playerActions.FindActionMap("Player").FindAction("Mine");
            interaction = playerActions.FindActionMap("Player").FindAction("Interact");

            // Input action callbacks
            moveAction.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            moveAction.canceled += ctx => moveInput = Vector2.zero;
            AssignInputs();

            lookAction.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
            lookAction.canceled += ctx => lookInput = Vector2.zero;

            
        }

        public void Mine()
        {
            if (!canMine || isInventoryOpen) return; // Don't mine if not ready or inventory is open

            // Optional: Play a mining animation
            // animator.SetTrigger("Mine"); // You'd need a "Mine" trigger in your Animator

            canMine = false;
            Invoke(nameof(ResetMine), mineRate); // Reset mining cooldown

            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, mineDistance, mineableLayer))
            {
                Debug.Log("Hit something mineable: " + hit.collider.name);

                // Optional: Instantiate a hit effect at the impact point
                if (mineEffect != null)
                {
                    Instantiate(mineEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }

                // --- Handle the mining logic here ---
                if (hit.collider.TryGetComponent<MineableResource>(out var mineable))
                {
                    mineable.TakeDamage(attackDamage); // Assuming mining does "damage" to the resource
                                                       // Or mineable.MineResource(inventoryManager);
                                                       // Or hit.collider.gameObject.GetComponent<SomeResourceScript>().Gather();
                }
                else
                {
                    Debug.LogWarning("Hit object with mineableLayer, but no MineableObject component found.");
                }
            }
            else
            {
                Debug.Log("Did not hit anything mineable within range.");
            }
        }

        private void ResetMine()
        {
            canMine = true;
        }


        public void Attack() {
            
            Debug.Log("Attack Invoked " + isInventoryOpen);



            if (!readyToAttack || attacking) return;

            readyToAttack = false;
            attacking = true;

            Invoke(nameof(ResetAttack), attackRate);
            Invoke(nameof(PerformAttack), attackDelay);


            //if (attackCount == 0)
            //{
            //    ChangeAnimationState(ANIMATION_ATTACK_01);
            //    attackCount += 1;
            //}
            //else {
            //    ChangeAnimationState(ANIMATION_ATTACK_02);
            //    attackCount = 0;

            //}

        }
        private void HandleRightClickInteract()
        {
            if (isInventoryOpen || !controlsEnabled)
            {
                // Don't interact if inventory is open, game is paused, or controls are disabled
                return;
            }

            // Perform a raycast from the center of the screen
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            // Check if the ray hits an enemy object within a certain distance
            if (Physics.Raycast(ray, out hit, attackDistance, npcLayer)) // Reuse attackDistance for interaction range
            {
                Debug.Log($"Right-clicked on: {hit.collider.name}");

                // Example: Get a component from the enemy and call a method
                NormalAI enemyHealth = hit.collider.GetComponent<NormalAI>();
                if (enemyHealth != null)
                {
                    // You could:
                    // 1. Target the enemy (e.g., set it as current target)
                    // playerCombat.SetTarget(enemyHealth); // If you have a combat manager
                    Debug.Log($"Targeted enemy: {hit.collider.name}");

                    shopManager.gameObject.SetActive(true);
                    shopManager.ShowCoinsText();
                    shopManager.OpenShop();
                    closeButton.gameObject.SetActive(true);


                    // 2. Display enemy info (e.g., show a health bar above it)
                    // enemyHealth.ShowInfo();

                    // 3. Initiate a dialogue (if it's an NPC)
                    // NPCDialogue dialogue = hit.collider.GetComponent<NPCDialogue>();
                    // if (dialogue != null) dialogue.StartDialogue();
                }
                else
                {
                    Debug.LogWarning($"Right-clicked on an object in EnemyLayer but it has no EnemyHealth component: {hit.collider.name}");
                }
            }
            else
            {
                Debug.Log("Right-clicked nothing or not an enemy.");
            }
        }

        void Update()
        {
            //// >>>>>>>>>>>>>>>>> NEW: Only run if controls are enabled <<<<<<<<<<<<<<<<<
            //if (!controlsEnabled)
            //{
            //    // Optionally clear input here if you want to ensure no lingering movement
            //    // when controls are re-enabled (e.g., if a key was held down while menu opened)
            //    moveInput = Vector2.zero;
            //    lookInput = Vector2.zero;
            //    return;
            //}

            //if (Keyboard.current.qKey.isPressed)
            //{
            //    if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, 5f))
            //    {
            //        NPCTrader trader = hit.collider.GetComponent<NPCTrader>();
            //        if (trader != null)
            //        {
            //            trader.BeginTrading();
            //        }
            //    }
            //}

            if (characterController == null)
            {
                Debug.LogError("The FirstPersonController script on the object '" + gameObject.name + "' is missing a CharacterController component.", this.gameObject);
            }

            //if (attackAction.IsPressed()) {

            //    Attack();
            //}
            //if (defendAction.WasPressedThisFrame()) {
            //    Defend();
            //}

            //if (shopManager != null)
            //{
            //    shopManager.gameObject.SetActive(false);
            //}


            HandleMovement();
            HandleRotation();
            InventoryState();
        }

        

        public void ResetAttack() {

            Debug.Log("Reset Attack " + isInventoryOpen);
            attacking = false;
            readyToAttack = true;
        }
        public void ResetDefend() {
            Debug.Log("Reset Defend " + isInventoryOpen);
            defending = false;

        }
        public void PerformAttack() {
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, attackDistance, attackLayer))
            {
                //HitTarget(hit.point);
            }
        }

        void DisableMovement() {
        
            
        }



        public void ChangeAnimationState(string newState) {
            if (currentAnimationState == newState) return;

            currentAnimationState = newState;
            animator.CrossFade(currentAnimationState, 0.1f);
        }

   

        public void Defend()
        {

            Debug.Log(isInventoryOpen);
            
            defending = !defending;
            animator.SetBool("isDefending", defending);
            
            

        }
        public void AssignInputs() {

            //defendAction.performed += ctx => Defend();
            //attackAction.performed += ctx => Attack();
            //mineAction.performed += ctx => Mine()
            ;

        }

        private void OnEnable()
        {
            // Only enable input actions if controlsEnabled is true
            // This is important because MenuManager might set controlsEnabled to false
            // before this OnEnable runs if the menu is open from the start.
            if (controlsEnabled)
            {
                EnableInputActions();
            }
        }
        private void OnDisable()
        {
            DisableInputActions();
        }

        // >>>>>>>>>>>>>>>>> NEW: Helper methods to enable/disable all actions <<<<<<<<<<<<<<<<<
        private void EnableInputActions()
        {
            moveAction.Enable();
            jumpAction.Enable();
            sprintAction.Enable();
            lookAction.Enable();
            attackAction.Enable();
            defendAction.Enable();
            mineAction.Enable();
            interaction.Enable();
            interaction.performed += ctx => HandleRightClickInteract();
        }

        private void DisableInputActions()
        {
            moveAction.Disable();
            jumpAction.Disable();
            sprintAction.Disable();
            lookAction.Disable();
            attackAction.Disable();
            defendAction.Disable();
            mineAction.Disable();
            interaction.Disable();
            interaction.performed -= ctx => HandleRightClickInteract();
        }
        //private void OnTriggerEnter(Collider other) // Or void OnTriggerEnter2D(Collider2D other) for 2D
        //{
        //    // You'd add logic here for picking up the item
        //    // For example:
        //    if (other.CompareTag("FPSController"))
        //    {

        //        Debug.Log("Picking up item");


        //        // Add item to player's inventory
        //        Destroy(gameObject);
        //    }
        //}

        private void OnCollisionEnter(Collision collision)
        {
            // We only care about objects tagged "Projectile".
            if (collision.gameObject.CompareTag("Projectile"))
            {
                // --- THIS IS THE NEW LOGIC ---
                // Check the flag from your WeaponController.
                if (weaponController != null && weaponController.isDefending)
                {
                    // --- DEFENDING LOGIC ---
                    Debug.Log("BLOCKED! Projectile was deflected.");

                
                }
                else
                {
                    // --- NOT DEFENDING LOGIC (Take Damage) ---
                    Debug.Log("HIT! Player took damage from a projectile.");

                    // Try to get the Projectile component from the object we hit.
                    if (collision.gameObject.TryGetComponent<Projectile>(out var projectile))
                    {
                        // Apply damage from the projectile's damage value.
                        if (healthManager != null)
                        {
                            healthManager.TakeDamage(projectile.damage);
                        }
                    }
                }

                // --- COMMON LOGIC ---
                // In BOTH cases (hit or block), we want to destroy the projectile.
                Destroy(collision.gameObject);
            }
        }


        private void HandleGravityAndJumping()
        {
            if (characterController.isGrounded)
            {
                currentMovement.y = -0.5f; // Small downward force to ensure isGrounded stays true
                if (jumpAction.triggered)
                { // Use 'triggered' for one-shot actions
                    currentMovement.y = jumpHeight;
                }
            }
            else
            {
                currentMovement.y -= gravity * Time.deltaTime;
            }
        }

        private void HandleInteraction()
        {
            // Make sure the mainCamera still exists before using it!
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera is missing!");
                return;
            }

            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionDistance, npcLayer))
            {
                // We hit something on the enemy layer, now get its AI script
                NormalAI ai = hit.collider.GetComponent<NormalAI>();
                if (ai != null)
                {
                    // SUCCESS! Call the public method on the AI we hit.
                    ai.OnInteractedByPlayer();

                    // And now you can open your UI menu
                    shopManager.gameObject.SetActive(true);
                    
                }
            }
        }

        private void HandleMovement()
        {
            // Read sprint input
            float speedModifier = sprintAction.ReadValue<float>() > 0 ? sprintMultiplier : 1f;

            // Calculate movement direction relative to player's forward
            Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
            moveDirection.Normalize(); // Normalize to prevent faster diagonal movement

            // Apply speed and modifier
            currentMovement.x = moveDirection.x * movementSpeed * speedModifier;
            currentMovement.z = moveDirection.z * movementSpeed * speedModifier;

            HandleGravityAndJumping(); // Apply gravity and jump to currentMovement.y

            characterController.Move(currentMovement * Time.deltaTime);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit) // 
        {
            
            if (hit.gameObject.CompareTag("DroppedItem"))
            {
                Debug.Log("Collided with DroppedItem"); 
                if (hit.gameObject.TryGetComponent<ItemObjects>(out var itemObject))
                {
                    Debug.Log("Picking up item");
                

                    inventoryManager.AddItems(itemObject.item);

                    // Add item to player's inventory
                    Destroy(hit.gameObject);
                }
            }
            //if (hit.gameObject.CompareTag("Mineable"))
            //{
            //    Debug.Log("Collided with Mineable");
            //    if (hit.gameObject.TryGetComponent<ItemObjects>(out var itemObject))
            //    {
            //        Debug.Log("Picking up item");


            //        inventoryManager.AddItems(itemObject.item);

            //        // Add item to player's inventory
            //        Destroy(hit.gameObject);
            //    }
            //}
            //if (hit.gameObject.CompareTag("Projectile"))
            //{

            //    Debug.Log("fwfefw");
            //    // Try to get the Projectile component from the object we hit
            //    Projectile projectile = hit.gameObject.GetComponent<Projectile>();


            //    if (projectile != null)
            //    {
            //        // Apply damage from the projectile's damage value
            //        healthManager.TakeDamage(projectile.damage);
            //    }

            //    // Destroy the projectile after it hits the player
            //    Destroy(hit.gameObject, 2.0f);
            //}
        }
        



        public void InventoryState()
        {

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (ShopManager.isShopOpen) return;
                Debug.Log(isInventoryOpen);
                if (inventoryMenu.activeSelf)
                {
                    inventoryMenu.SetActive(false);
                    SetControlsEnabled(true); // Re-enable controls when closing inventory
                    isInventoryOpen = false;
                }
                else
                {
                    inventoryMenu.SetActive(true);
                    SetControlsEnabled(false); // Disable controls when opening inventory
                    isInventoryOpen = true;
                }
            }

            // Handle the pointer click event

        }

        private void HandleRotation()
        {
            // Apply horizontal rotation to the player body (around Y-axis)
            float mouseX = lookInput.x * mouseSensitivity;
            transform.Rotate(Vector3.up * mouseX);

            // Apply vertical rotation to the camera (around X-axis)
            float mouseY = lookInput.y * mouseSensitivity;
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);
            mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        }

        // >>>>>>>>>>>>>>>>> NEW: Public method for other scripts to call <<<<<<<<<<<<<<<<<
        public void SetControlsEnabled(bool enabled)
        {
            // This flag is the main gatekeeper in the Update() method.
            controlsEnabled = enabled;

            if (enabled)
            {
                // --- RE-ENABLE GAMEPLAY ---

                // 1. Re-enable the input action map to start receiving input again.
                playerActions.Enable();

                // 2. Lock and hide the cursor for first-person gameplay.
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                
            }
            else
            {
                // --- DISABLE GAMEPLAY FOR MENU ---

                // 1. Disable the input action map to stop receiving all player input.
                playerActions.Disable();

                // 2. Unlock and show the cursor for UI interaction.
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                // 3. CRITICAL: Reset all input values to zero. This stops all movement and looking instantly.
                moveInput = Vector2.zero;
                lookInput = Vector2.zero;
                

            }
        }
    }
}