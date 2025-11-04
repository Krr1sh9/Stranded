using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformers
{
    public class WeaponController : MonoBehaviour
    {
        public GameObject Sword; // This could be a tool like a pickaxe as well

        // --- Action Flags ---
        public bool CanAttack = true;
        public bool CanDefend = true;
        public bool CanMine = true; // NEW: Flag for mining cooldown

        // --- Cooldowns ---
        public float AttackCooldown = 1f;
        public float MineCooldown = 1.2f; // NEW: Cooldown for the mining action

        // --- Input Actions ---
        public InputActionAsset weaponActions;
        private InputAction attackAction;
        private InputAction defendAction;
        private InputAction mineAction; // NEW: Input action for mining

        // --- State Booleans ---
        public bool isAttacking = false;
        public bool isDefending = false;
        public bool isMining = false; // NEW: State for mining animation

        public void Awake()
        {
            // Find all the actions from the Input Action Asset
            var playerActionMap = weaponActions.FindActionMap("Player");
            attackAction = playerActionMap.FindAction("Attack");
            defendAction = playerActionMap.FindAction("Defend");
            mineAction = playerActionMap.FindAction("Mine"); // NEW: Find the "Mine" action
        }

        private void OnEnable()
        {
            attackAction.Enable();
            defendAction.Enable();
            mineAction.Enable(); // NEW: Enable the mine action
        }

        private void OnDisable()
        {
            attackAction.Disable();
            defendAction.Disable();
            mineAction.Disable(); // NEW: Disable the mine action
        }

        private void Update()
        {
            // Check for Attack input
            if (attackAction.triggered && CanAttack)
            {
                SwordAttack();
            }

            // Check for Defend input
            if (defendAction.triggered && CanDefend)
            {
                SwordDefend();
            }

            // NEW: Check for Mine input
            if (mineAction.triggered && CanMine)
            {
                MineAction();
            }
        }

        // --- Sword Attack Logic ---
        private void SwordAttack()
        {
            if (FirstPersonController.isInventoryOpen || ShopManager.isShopOpen) return;
            if (UIManager.enables) return; // Uncomment if you have this manager
            if (CanAttack)
            {
                isAttacking = true;
                CanAttack = false;
                Animator anim = Sword.GetComponent<Animator>();
                anim.SetTrigger("Attack");
                StartCoroutine(ResetAttack());
            }
        }

        private IEnumerator ResetAttack()
        {
            StartCoroutine(ResetAttackBool());
            yield return new WaitForSeconds(AttackCooldown);
            CanAttack = true;
        }

        IEnumerator ResetAttackBool()
        {
            yield return new WaitForSeconds(AttackCooldown);
            isAttacking = false;
        }

        // --- Sword Defend Logic ---
        private void SwordDefend()
        {
            if (FirstPersonController.isInventoryOpen || ShopManager.isShopOpen) return;
            if (UIManager.enables) return; // Uncomment if you have this manager
            if (CanDefend)
            {
                isDefending = true;
                CanDefend = false;
                Animator anim = Sword.GetComponent<Animator>();
                anim.SetTrigger("Defend");
                StartCoroutine(ResetDefend());
            }
        }

        private IEnumerator ResetDefend()
        {
            StartCoroutine(ResetDefendBool());
            yield return new WaitForSeconds(0.5f);
            CanDefend = true;
        }

        IEnumerator ResetDefendBool()
        {
            yield return new WaitForSeconds(0.5f);
            isDefending = false;
        }

        // --- NEW: MINING LOGIC ---
        private void MineAction()
        {
            // These checks prevent mining while a menu is open
            if (FirstPersonController.isInventoryOpen || ShopManager.isShopOpen) return;
            if (UIManager.enables) return; // Uncomment if you have this manager

            if (CanMine)
            {
                isMining = true;
                CanMine = false;
                Animator anim = Sword.GetComponent<Animator>(); // Assumes the pickaxe has an Animator
                anim.SetTrigger("Attack"); // You will need a "Mine" trigger in your Animator
                StartCoroutine(ResetMine());
            }
        }

        private IEnumerator ResetMine()
        {
            StartCoroutine(ResetMineBool());
            yield return new WaitForSeconds(MineCooldown);
            CanMine = true;
        }

        IEnumerator ResetMineBool()
        {
            // This coroutine resets the isMining flag after the animation is likely finished
            yield return new WaitForSeconds(MineCooldown);
            isMining = false;
        }
    }
}