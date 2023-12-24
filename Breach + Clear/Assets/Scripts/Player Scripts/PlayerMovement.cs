using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    private float startWalkSpeed;
    private float startSprintSpeed;
    private float startCrouchSpeed;
    public float speedReductionAmount;
    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = transform.localScale.y;

        startCrouchSpeed = crouchSpeed;
        startSprintSpeed = sprintSpeed;
        startWalkSpeed = walkSpeed;
    }
    
    private void Update()
    {

        Debug.Log(grounded);

        //ground check
        grounded = Physics.Raycast(transform.position + new Vector3(0,0.05f,0), Vector3.down, playerHeight * 0.5f + 0.35f, whatIsGround);

        MyInput();
        StateHandler();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 5;

        //reduce speed when ADS
        if (GameManager.isScoped)
        {
            crouchSpeed = startCrouchSpeed / speedReductionAmount;
            walkSpeed = startWalkSpeed / speedReductionAmount;
        }

        else
        {
            crouchSpeed = startCrouchSpeed;
            walkSpeed = startWalkSpeed;
        }
    }
  
    private void FixedUpdate()
    {
        MovePlayer();
    }
    
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(GameManager.crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(GameManager.crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void MovePlayer()
    {
        // calculate move direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (!OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 7.25f, ForceMode.Force);
        }

        //airborn

        if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 3f, ForceMode.Force);
            rb.AddForce(Vector3.down * 30f, ForceMode.Force);
        }

        //on slope
        if (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 7.25f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 10f, ForceMode.Force);
            }
                
        }

        //no grav on slopes
        rb.useGravity = !OnSlope();
    }

    private void StateHandler()
    {
        // Mode - Sprinting
        if(grounded && Input.GetKey(GameManager.sprintKey))
        {
            GameManager.isSprinting = true;
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        //walking
        else if (grounded)
        {
            GameManager.isSprinting = false;
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        //air
        else
        {
            GameManager.isSprinting = false;
            state = MovementState.air;
        }

        //crouching
        if (Input.GetKey(GameManager.crouchKey))
        {
            GameManager.isSprinting = false;
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
    }

    //teleport junk
    public void Teleport(Vector3 position)
    {
        transform.position = position;
        Physics.SyncTransforms();
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle <= maxSlopeAngle && angle != 0;
        }

        return false;
    }
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
    private void SpeedControl()
    {
        if (OnSlope())
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

        if (rb.velocity.y < -30f)
        {
            rb.velocity = new Vector3(rb.velocity.x, -30f, rb.velocity.z);
        }
    }
}
