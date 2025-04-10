using UnityEngine;

public class PlayerController : MonoBehaviour
{
   
    [Header("Movement Settings")]
    // [SerializeField] private float walkingSpeed = 2;
    [SerializeField] private float runningSpeed = 5;
    // [SerializeField] private float sprintingSpeed = 6;
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
    
    [Header("SFX")]
    [SerializeField] private AudioClip landingAudioClip;
    [SerializeField] private AudioClip[] footstepAudioClips;
    [Range(0, 1)] [SerializeField] private float footstepAudioVolume = 0.5f;

   //Properties
    public InputManager PlayerInput { get; private set; }
    public float VerticalMovement { get; private set; }
    public float HorizontalMovement { get; private set; }
    public float MoveAmount { get; private set; }
    
    private Vector3 moveDirection;
    private bool rotateOnMove = true;
    // timeout deltatime
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;
    
    private float verticalVelocity;
    private float terminalVelocity = 53.0f;

    private CharacterController controller;
    Animator animator;

    private void Awake()
    {
        PlayerInput = GetComponent<InputManager>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
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
        HandleAllMovement();
    }

    public void HandleAllMovement()
    {
        
        HandleGrondedMovement();
        HandleMovementInput();
        HandleRotation();
    }
    private void GetMovementValues()
    {
        VerticalMovement = PlayerInput.MovePosition.y;
        HorizontalMovement = PlayerInput.MovePosition.x;
        
        if (VerticalMovement != 0 || HorizontalMovement != 0)
        {
           // GetComponent<ActionScheduler>().StartAction(this);
        }
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
                if (PlayerInput.JumpInput && jumpTimeoutDelta <= 0.0f)
                {
                   
                    PlayerInput.JumpInput = false;
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
                PlayerInput.JumpInput = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (verticalVelocity < terminalVelocity)
            {
                verticalVelocity += gravity * Time.deltaTime;
            }
        }

    private void HandleMovementInput()
    {
        GetMovementValues();
        
        MoveAmount = Mathf.Clamp01(Mathf.Abs(VerticalMovement) + Mathf.Abs(HorizontalMovement));

        if (MoveAmount <= 0.5f && MoveAmount > 0)
        {
            MoveAmount = 0.5f;
        }
        else if (MoveAmount >= 0.5f & MoveAmount <= 1f)
        {
            MoveAmount = 1f;
        }
        
        UpdateAnimator(MoveAmount);
          
    }
   
    private void HandleGrondedMovement()
    {
        GetMovementValues();
        moveDirection = Camera.main.transform.forward * VerticalMovement;
        moveDirection += Camera.main.transform.right * HorizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;
        
        // move the player
        controller.Move(moveDirection * (runningSpeed * Time.deltaTime) +
            new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
        
    }
    
    private void HandleRotation()
    {
        if (rotateOnMove)
        {
            HandleNormalRotation();
        }
    }

    private void HandleNormalRotation()
    {
            Vector3 targetDirection = Vector3.zero;
            targetDirection = Camera.main.transform.forward * VerticalMovement;
            targetDirection += Camera.main.transform.right * HorizontalMovement;
            targetDirection.Normalize();
            targetDirection.y = 0;
                
            if (targetDirection == Vector3.zero)
            {
                targetDirection = transform.forward;
            }
                
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion rotateCharacter = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = rotateCharacter; 
    }
    
     private void UpdateAnimator(float speed)
     {
         animator.SetFloat(AnimatorParams.Speed, speed, 0.1f, Time.deltaTime);
     }

    public void Cancel()
    {
        
    }

    public void SetRotateOnMove(bool value)
    {
        rotateOnMove = value;
    }
    #region Animation Events
    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (footstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, footstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(footstepAudioClips[index], transform.TransformPoint(controller.center), footstepAudioVolume);
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(landingAudioClip, transform.TransformPoint(controller.center), footstepAudioVolume);
        }
    }
    
    #endregion
}
