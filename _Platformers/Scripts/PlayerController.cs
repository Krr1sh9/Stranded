using Cinemachine; // Correct namespace for CinemachineFreeLook and related types
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformers
{
    public class PlayerController : MonoBehaviour
    {
        public Vector2 moveValue;
        public float moveSpeed;

        public void OnMove(InputValue value) {
            moveValue = value.Get<Vector2>();
        }

        public void FixedUpdate()
        {
            Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);
            GetComponent<Rigidbody>().AddForce(movement * moveSpeed * Time.fixedDeltaTime);
        }

        //        [Header("References")]
        //        [SerializeField] CharacterController controller;
        //        [SerializeField] Animator animator;
        //        [SerializeField] InputReader inputReader;
        //        [SerializeField] CinemachineFreeLook freeLook;
        //        [SerializeField] Transform camera;

        //        [Header("Settings")]
        //        [SerializeField] float moveSpeed = 5f;
        //        [SerializeField] float rotationSpeed = 10f;
        //        [SerializeField] float smoothSpeed = 0.2f;
        //        [SerializeField] float smoothTime = 0.2f;
        //        [SerializeField] float gravity = 9.81f;
        //        [SerializeField] float turningSpeed = 2f;
        //        [SerializeField] float jumpHeight = 2f;

        //        private float verticalVelocity;

        //        [Header("Movement Settings")]
        //        public float walkSpeed = 2f;

        //        [Header("Input")]
        //        private float moveInput;
        //        private float turnInput;


        //        Transform mainCam;
        //        private float currentSpeed;
        //        private float speedVelocity;
        //        const float ZeroF = 0f;
        //        float velocity;


        //        public void Start()
        //        {
        //            controller = GetComponent<CharacterController>();
        //        }




        //        public void Awake()
        //        {
        //            mainCam = Camera.main.transform;
        //            freeLook.Follow = transform;
        //            freeLook.LookAt = transform;
        //            freeLook.OnTargetObjectWarped(transform, transform.position - freeLook.transform.position - Vector3.forward);
        //        }

        //        public void Update()
        //        {
        //            InputManagement();
        //            HandleMovement();

        //        }

        //        public void Movement() {
        //            Turn();
        //            GroundMovement();

        //        }

        //        public void GroundMovement() {
        //            Vector3 move = new Vector3(inputReader.Direction.x, 0, inputReader.Direction.y);
        //            move = camera.transform.TransformDirection(move);



        //            move *= walkSpeed;
        //            move.y = VerticalForceCalculation();

        //            controller.Move(move * moveSpeed * Time.deltaTime);
        //        }

        //        private void Turn() {
        //            if (Mathf.Abs(turnInput) > 0 | Mathf.Abs(moveInput) > 0) {

        //                Vector3 currentLookDirection = controller.velocity.normalized;
        //                currentLookDirection.y = 0;

        //                currentLookDirection.Normalize();

        //                Quaternion targetRotation = Quaternion.LookRotation(currentLookDirection);
        //                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turningSpeed);

        //            }
        //        }

        //        private float VerticalForceCalculation() {
        //            if (controller.isGrounded) {
        //                verticalVelocity = 0;
        //                if (Input.GetButtonDown("Jump")) {
        //                    verticalVelocity = Mathf.Sqrt(jumpHeight * 2f * gravity);
        //                }
        //            }
        //            else {
        //                verticalVelocity -= gravity * Time.deltaTime;
        //            }
        //            return verticalVelocity;
        //        }

        //        private void InputManagement() { 
        //            moveInput = Input.GetAxis("Vertical");
        //            turnInput = Input.GetAxis("Horizontal");

        //        }
        //        public void HandleMovement() {
        //            var movementDirection = new Vector3(inputReader.Direction.x, 0, inputReader.Direction.y).normalized;
        //            var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movementDirection;

        //            if (adjustedDirection.magnitude > ZeroF)
        //            {
        //                HandleRotation(adjustedDirection);
        //                HandleCharacterController(adjustedDirection);
        //                SmoothSpeed(adjustedDirection.magnitude);
        //            }
        //            else
        //            {
        //                SmoothSpeed(ZeroF);

        //                // Reset horizontal velocity for a snappy stop

        //            }
        //        }

        //        void HandleCharacterController(Vector3 adjustedDirection) {
        //            var adjustedMovement = adjustedDirection * (moveSpeed * Time.deltaTime);
        //            controller.Move(adjustedMovement);

        //        }

        //        void HandleRotation(Vector3 adjustedDirection)
        //        {
        //            // Adjust rotation to match movement direction
        //            var targetRotation = Quaternion.LookRotation(adjustedDirection);
        //            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        //        }

        //        void SmoothSpeed(float value)
        //        {
        //            currentSpeed = Mathf.SmoothDamp(currentSpeed, value, ref velocity, smoothTime);
        //        }
    }


}
