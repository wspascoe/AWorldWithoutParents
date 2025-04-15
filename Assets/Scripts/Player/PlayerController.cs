using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
   
    [Header("Movement Settings")]
    [SerializeField] private float walkingSpeed = 2;
    [SerializeField] private float runningSpeed = 5;
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;
    [SerializeField] private float rotationSpeed = 5;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -15.0f;
    [Space(10)]
    [SerializeField] private float jumpTimeout = 0.50f;
    [SerializeField] private float fallTimeout = 0.15f;

    [Header("Player Grounded")]
    [SerializeField] private bool grounded = true;
    [SerializeField] private float groundedOffset = -0.14f;
    [SerializeField] private float groundedRadius = 0.28f;
    [SerializeField] private LayerMask groundLayers;
    
    [Header("Cinemachine")]
    public GameObject CinemachineCameraTarget;
    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;
    public float CameraAngleOverride = 0.0f;
    public bool IsCameraLocked = false;
    
    // cinemachine
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;
    private bool questPos = false;
    [Header("SFX")]
    [SerializeField] private AudioClip landingAudioClip;
    [SerializeField] private AudioClip[] footstepAudioClips;
    [Range(0, 1)] [SerializeField] private float footstepAudioVolume = 0.5f;
    
   //Properties
    InputManager playerInput;
    public float SpeedChangeRate { get; private set; } = 10.0f;
    private Vector3 moveDirection;
    private bool rotateOnMove = true;
    // timeout deltatime
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;
    private float speed;
    private float animationBlend;
    private float verticalVelocity;
    private float terminalVelocity = 53.0f;
    private float targetRotation = 0.0f;
    private float rotationVelocity;
    private const float threshold = 0.01f;
    
    
    private CharacterController controller;
    Animator animator;
    Energy energy;

    private void Awake()
    {
        playerInput = GetComponent<InputManager>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        energy = GetComponent<Energy>();
    }

    private void Start()
    {
        // reset our timeouts on start
        jumpTimeoutDelta = jumpTimeout;
        fallTimeoutDelta = fallTimeout;
    }

    private void Update()
    {
        JumpAndGravity();
        GroundedCheck();
        Move();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }
    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset,
            transform.position.z);
        grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers,
            QueryTriggerInteraction.Ignore);

       
        animator.SetBool(AnimatorParams.Grounded, grounded);
        
    }
   
     private void JumpAndGravity()
        {
            if (grounded)
            {
                // reset the fall timeout timer
                fallTimeoutDelta = fallTimeout;
                animator.SetBool(AnimatorParams.Jump, false);
                animator.SetBool(AnimatorParams.FreeFall, false);
                

                // stop our velocity dropping infinitely when grounded
                if (verticalVelocity < 0.0f)
                {
                    verticalVelocity = -2f;
                }

                // Jump
                if (playerInput.JumpInput && jumpTimeoutDelta <= 0.0f)
                {
                   
                    playerInput.JumpInput = false;
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    animator.SetBool(AnimatorParams.Jump, true);
                    
                }

                // jump timeout
                if (jumpTimeoutDelta >= 0.0f)
                {
                    jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                jumpTimeoutDelta = jumpTimeout;

                // fall timeout
                if (fallTimeoutDelta >= 0.0f)
                {
                    fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    animator.SetBool(AnimatorParams.FreeFall, true);
                    
                }

                // if we are not grounded, do not jump
                playerInput.JumpInput = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (verticalVelocity < terminalVelocity)
            {
                verticalVelocity += gravity * Time.deltaTime;
            }
        }

      private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (playerInput.LookPosition.sqrMagnitude >= threshold && !IsCameraLocked)
            {
                cinemachineTargetYaw += playerInput.LookPosition.x;
                cinemachineTargetPitch += playerInput.LookPosition.y;
            }
            
            // clamp our rotations so our values are limited 360 degrees
            cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
            cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);
            
            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + CameraAngleOverride,
                cinemachineTargetYaw, 0.0f);
            
        }
        
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
        private void Move()
        {
            bool canRun = playerInput.RunInput && energy.Amount >= 30;
            float targetSpeed = canRun ? runningSpeed : walkingSpeed;
            
            if (playerInput.MovePosition == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

            float speedOffset = 0.1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                speed = Mathf.Round(speed * 1000f) / 1000f;
            }
            else
            {
                speed = targetSpeed;
            }

            animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (animationBlend < 0.01f) animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(playerInput.MovePosition.x, 0.0f, playerInput.MovePosition.y).normalized;

            //note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
           // if there is a move input rotate player when the player is moving
            if (playerInput.MovePosition != Vector2.zero)
            {
                targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  Camera.main.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                    RotationSmoothTime);
            
                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
            

            Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

            // move the player
            controller.Move(targetDirection.normalized * (speed * Time.deltaTime) +
                            new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
            if (canRun)
            {
                energy.UseEnergy(Time.deltaTime);
            }
            animator.SetFloat(AnimatorParams.Speed, animationBlend);
           
        }
    
    // #region Animation Events
    // private void OnFootstep(AnimationEvent animationEvent)
    // {
    //     if (animationEvent.animatorClipInfo.weight > 0.5f)
    //     {
    //         if (footstepAudioClips.Length > 0)
    //         {
    //             var index = Random.Range(0, footstepAudioClips.Length);
    //             AudioSource.PlayClipAtPoint(footstepAudioClips[index], transform.TransformPoint(controller.center), footstepAudioVolume);
    //         }
    //     }
    // }
    //
    // private void OnLand(AnimationEvent animationEvent)
    // {
    //     if (animationEvent.animatorClipInfo.weight > 0.5f)
    //     {
    //         AudioSource.PlayClipAtPoint(landingAudioClip, transform.TransformPoint(controller.center), footstepAudioVolume);
    //     }
    // }
    //
    // #endregion
}
