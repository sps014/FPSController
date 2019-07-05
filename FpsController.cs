using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sonic.FPS
{
    public class FpsController : MonoBehaviour
    {
        
        [Header("Player Move  Speed Parameters")]
        // Speed Parameters of our character Controller
        public float MoveSpeed = 3.5f;
        public float RunSpeed = 7.5f;
        public float CrouchSpeed = 1.8f;
        public float ProneSpeed = 1f;

        //current player state
        public static PlayerState FpsPlayerState;


        [Header("Jump Parameters")]
        public float jumpForce = 8.0f;

        //Funny enough emulating gravity (i know its 9.8f) with 20f
        public float Gravity = 20.0F;

        [Header("Player Height Parameter")]
        public float HeightStanding = 1;
        public float HeightCrouching = 0.55f;
        public float HeightProning = 0.25f;

        private CharacterController characterController;
        private CapsuleCollider capsuleCollider;
        private Transform lookRoot;

        private float xMove, yMove;
        private Vector3 moveDirection;
        private float verticalVelocity;

        private float currentMovementSpeed = 0;

        private void Awake()
        {
            lookRoot = transform.GetChild(0);
        }
        void Start()
        {
            //Grab current Character Controller
            characterController = GetComponent<CharacterController>();
            capsuleCollider = GetComponent<CapsuleCollider>();

            FpsPlayerState=PlayerState.Standing;
        }

        void FixedUpdate()
        {
            Crouch();
            Prone();
            RunningOrWalking();

            //Get Input Amount
            xMove = Input.GetAxis(AllAxisAndButtons.Horizontal);
            yMove = Input.GetAxis(AllAxisAndButtons.Vertical);

            //Get Move Direction from x and y Axis Movement
            moveDirection=new Vector3(xMove, 0, yMove);

            //Convert Local Direction to Global Move Direction
            moveDirection = transform.TransformDirection(moveDirection);

            //Apply Gravity to Body
            ApplyGravity();

            //Check for mouse lock settings
            MouseLockCheck();

            //Set Character Move Speed
            SetMoveSpeed();
            //currentMovementSpeed=Input.GetKey(KeyCode.LeftShift)?RunSpeed: MoveSpeed;

            //Move Character to position
            characterController.Move(moveDirection*currentMovementSpeed*Time.deltaTime);

        }
        void ApplyGravity()
        {
            if(characterController.isGrounded)
            {
                verticalVelocity -= Gravity * Time.deltaTime;
                //check if player want to jump
                JumpCheck();
            }
            else
            {
                verticalVelocity -= Gravity * Time.deltaTime;
            }
            //update MoveDirection Finally
            moveDirection.y = verticalVelocity * Time.deltaTime;
        }
        void JumpCheck()
        {
            //check if Jump button is pressed
            if(characterController.isGrounded && Input.GetButton(AllAxisAndButtons.Jump))
            {
                //Set jump force
                verticalVelocity = jumpForce;
                FpsPlayerState = PlayerState.Jumped;
            }
            
        }

        void Crouch()
        {
            if(Input.GetKeyDown(KeyCode.C))
            {
                if(FpsPlayerState!=PlayerState.Crouching && FpsPlayerState!=PlayerState.Jumped)
                {
                    FpsPlayerState = PlayerState.Crouching;
                    //set variables
                    lookRoot.localPosition = Vector3.up * HeightCrouching;

                    capsuleCollider.height = 2 * HeightCrouching;
                    characterController.height = 2 * HeightCrouching;
                }
                else
                {
                    Stand();
                }
            }
        }
        void Prone()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (FpsPlayerState != PlayerState.Proning && FpsPlayerState != PlayerState.Jumped)
                {
                    FpsPlayerState = PlayerState.Proning;
                    //set variables
                    lookRoot.localPosition = Vector3.up * HeightProning;

                    capsuleCollider.height =  HeightProning;
                    characterController.height =  HeightProning;
                }
                else
                {
                    Stand();
                }
            }
        }
        void Stand()
        {
            FpsPlayerState = PlayerState.Standing;
            lookRoot.localPosition = Vector3.up * HeightStanding;

            capsuleCollider.height = 2 * HeightStanding;
            characterController.height = 2 * HeightStanding;
        }
        private void RunningOrWalking()
        {
            if (Input.GetKey(KeyCode.LeftShift) && characterController.velocity.sqrMagnitude > 0)
            {
                if (FpsPlayerState != PlayerState.Running || FpsPlayerState != PlayerState.Standing)
                    Stand();

                FpsPlayerState = PlayerState.Running;
            }
            else if (characterController.velocity.sqrMagnitude > 0)
            {
                FpsPlayerState = PlayerState.Walking;
            }
            else if (characterController.velocity.sqrMagnitude == 0)
            {
                FpsPlayerState = PlayerState.Standing;
            }
            
        }
        void SetMoveSpeed()
        {
           

            switch(FpsPlayerState)
            {
                case PlayerState.Proning:
                    currentMovementSpeed = ProneSpeed;
                    break;
                case PlayerState.Crouching:
                    currentMovementSpeed = CrouchSpeed;
                    break;
                case PlayerState.Walking:
                    currentMovementSpeed = MoveSpeed;
                    break;
                case PlayerState.Standing:
                    currentMovementSpeed = MoveSpeed;
                    break;
                case PlayerState.Running:
                    currentMovementSpeed = RunSpeed;
                    break;
            }
            Debug.ClearDeveloperConsole();
            Debug.Log(FpsPlayerState);
        }
        void MouseLockCheck()
        {
            if(Input.GetKey(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if(Input.GetMouseButton(0)||Input.GetMouseButton(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
       
    }
    public enum PlayerState
    {
        Running,
        Standing,
        Walking,
        Jumped,
        Crouching,
        Proning
    }

}