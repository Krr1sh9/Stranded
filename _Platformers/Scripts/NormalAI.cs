using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
namespace Platformers
{

    public class NormalAI : MonoBehaviour
    {
        public NavMeshAgent agent;
        public InputAction interact;
        public Transform player;

        public LayerMask whatIsGround, whatIsPlayer;

        public float health;

        //public Camera mainCamera;
        public LayerMask npcLayer;
        public float attackDistance = 3f;

        //Patroling
        public Vector3 walkPoint;
        bool walkPointSet;
        public float walkPointRange;

        //Attacking
        public float timeBetweenAttacks;
        bool alreadyAttacked;
        public GameObject projectile;

        //States
        public float sightRange, attackRange;
        public bool playerInSightRange, playerInAttackRange;
        [SerializeField] private InputActionAsset playerActions; // Assign your Input Action Asset here

        private void Awake()
        {
            player = GameObject.Find("FPSController").transform;
            agent = GetComponent<NavMeshAgent>();
            //mainCamera = Camera.main;
            Debug.Log("NormalAI: Main Camera assigned as " + playerActions);
            
        }

        private void Start()
        {
            interact = playerActions.FindActionMap("Player").FindAction("Interaction");
        }

        private void Update()
        {
            //Check for sight and attack range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (UIManager.enables) return;
            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            //if (playerInAttackRange && playerInSightRange) AttackPlayer();
        }

        //public void OnEnable()
        //{
        //    interact.Enable();
        //    interact.performed += ctx => HandleRightClickInteract();
        //}
        //public void OnDisable()
        //{
        //    interact.performed -= ctx => HandleRightClickInteract();
        //    interact.Disable();
        //}
        public void OnInteractedByPlayer()
        {
            Debug.Log($"Player has interacted with {gameObject.name}");
            // Here you can add logic for what happens when the player interacts.
            // For example, an NPC might start a dialogue, or an enemy might become alerted.
        }
        private void Patroling()
        {


            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet)
                agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            //Walkpoint reached
            if (distanceToWalkPoint.magnitude < 1f)
                walkPointSet = false;
        }
        private void SearchWalkPoint()
        {
            //Calculate random point in range
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
                walkPointSet = true;
        }

        //private void HandleRightClickInteract()
        //{
        //    //if (isInventoryOpen || !controlsEnabled)
        //    //{
        //    //    // Don't interact if inventory is open, game is paused, or controls are disabled
        //    //    return;
        //    //}

        //    // Perform a raycast from the center of the screen
        //    Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        //    RaycastHit hit;

        //    // Check if the ray hits an enemy object within a certain distance
        //    if (Physics.Raycast(ray, out hit, attackDistance, npcLayer)) // Reuse attackDistance for interaction range
        //    {
        //        Debug.Log($"Right-clicked on: {hit.collider.name}");

        //        // Example: Get a component from the enemy and call a method
        //        NormalAI enemyHealth = hit.collider.GetComponent<NormalAI>();
        //        if (enemyHealth != null)
        //        {
        //            // You could:
        //            // 1. Target the enemy (e.g., set it as current target)
        //            // playerCombat.SetTarget(enemyHealth); // If you have a combat manager
        //            Debug.Log($"Targeted enemy: {hit.collider.name}");



        //            // 2. Display enemy info (e.g., show a health bar above it)
        //            // enemyHealth.ShowInfo();

        //            // 3. Initiate a dialogue (if it's an NPC)
        //            // NPCDialogue dialogue = hit.collider.GetComponent<NPCDialogue>();
        //            // if (dialogue != null) dialogue.StartDialogue();
        //        }
        //        else
        //        {
        //            Debug.LogWarning($"Right-clicked on an object in EnemyLayer but it has no EnemyHealth component: {hit.collider.name}");
        //        }
        //    }
        //    else
        //    {
        //        Debug.Log("Right-clicked nothing or not an enemy.");
        //    }
        //}


        private void ChasePlayer()
        {
            agent.SetDestination(player.position);
        }

        private void AttackPlayer()
        {
            //Make sure enemy doesn't move
            agent.SetDestination(transform.position);

            transform.LookAt(player);

            if (!alreadyAttacked)
            {
                ///Attack code here
                Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
                rb.AddForce(transform.up * 8f, ForceMode.Impulse);

                ///End of attack code

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
        private void ResetAttack()
        {
            alreadyAttacked = false;
        }

        public void TakeDamage(int damage)
        {
            health -= damage;

            if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
        }
        private void DestroyEnemy()
        {
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sightRange);
        }
    }



}
