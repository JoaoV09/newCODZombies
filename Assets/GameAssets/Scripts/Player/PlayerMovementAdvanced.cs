using System.Collections;
using UnityEngine;

public class PlayerMovementAdvanced : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed = 2;
    public float sprintSpeed = 5;
    public float slideSpeed = 4;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    public float groundDrag = 5;

    [Header("Jumping")]
    public float jumpForce = 6;
    public float jumpCooldown = 2;
    public float airMultiplier = .4f;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed = 1.5f;
    public float crouchYScale = .5f;
    public Vector3 crouchCenter;
    private float startYScale;
    private Vector3 startCenter;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public LayerMask whatIsGround;
    public float speraRadius = .3f;

    public float distRay = 10f;
    public float distToSiling = 3f;

    [SerializeField] bool grounded;
    public bool isSiling;
    public bool sliding;
    
    [Header("Slope Handling")]
    public float maxSlopeAngle = 45f;
    private RaycastHit slopeHit;
    private bool exitingSlope;


    [Header("Reference")]
    public Transform orientation;
    public Animator animator;
    public CapsuleCollider cc;
    Rigidbody rb;


    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    public bool debug;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        sliding,
        air
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        rb.freezeRotation = true;

        readyToJump = true;

        startCenter = transform.localScale;

    }

    private void Update()
    {
        // ground check
        grounded = Physics.CheckSphere(transform.position, speraRadius, whatIsGround);

        isSiling = Physics.Raycast(transform.position , Vector3.up, distToSiling, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();
        SetAnimation();

        // handle drag
        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded && state != MovementState.sliding)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = crouchCenter;
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        } 
        else if (state != MovementState.crouching && state != MovementState.sliding)
        {
            transform.localScale = startCenter;
        }
    }

    private void StateHandler()
    {
        // Mode - Sliding
        if (sliding)
        {
            state = MovementState.sliding;

            if (OnSlope() && rb.linearVelocity.y < 0.1f)
                desiredMoveSpeed = slideSpeed;

            else
                desiredMoveSpeed = sprintSpeed;
        }

        // Mode - Crouching
        else if (Input.GetKey(crouchKey) || isSiling)
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        else if(grounded && Input.GetKey(sprintKey) && verticalInput > 0)
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }

        // check if desiredMoveSpeed has changed drastically
        if(Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.linearVelocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.linearVelocity.magnitude > moveSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, cc.height * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    private void SetAnimation()
    {
        if (animator == null) return;

        animator.SetFloat("Speed", verticalInput > 0 && Input.GetKey(sprintKey) ? 2 : verticalInput, .2f, Time.deltaTime);

        animator.SetBool("Jumping", grounded && Input.GetKey(jumpKey) && readyToJump && state == MovementState.sliding);
        animator.SetBool("Fall", state == MovementState.air ? true : false);
        animator.SetBool("IsGrounde", grounded);
        animator.SetBool("Slide", state == MovementState.sliding ? true : false);
        animator.SetBool("IsCrouch", state == MovementState.crouching ? true : false);


        if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, distRay, whatIsGround))
            animator.SetFloat("PreGround", (hit.point - transform.position).magnitude);
    }

    private void OnDrawGizmos()
    {
        if (!debug) return;
        Gizmos.color = grounded ? Color.green : Color.red;

        Gizmos.DrawSphere(transform.position, speraRadius);
        Gizmos.DrawRay(transform.position, -Vector3.up * distRay);
        
        Gizmos.color = isSiling ? Color.red : Color.green;
        Gizmos.DrawRay(transform.position , Vector3.up * distToSiling);
    }

}