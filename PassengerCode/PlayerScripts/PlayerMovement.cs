using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  
    [Header("Player Objects")]
    public Transform playerBody;
    public CharacterController cc;
    public Animator anim;
    public Transform groundCheck;
    public Transform ceilingCheck;
    public LayerMask groundMask;



    [Header("Current Player Movement Speed")]
    [SerializeField]
    private float currentSpeed;
    [Header("Movement Speed Variables")]
    [SerializeField]
    private float walkSpeed = 12f;
    [SerializeField]
    private float sprintSpeed = 20f;
    [SerializeField]
    private float sprintMultiplier;
    [SerializeField]
    private float crouchSpeed = 2f;
    static float crouchPlacementTime = 0f;
    static float standPlacementTime = 0f;

    //Movement bools
    public bool IsWalking { get; set; }
    public bool IsSprinting { get; set; }
    public bool IsCrouching { get; set; }

    //Body Crouch Placement
    private float standPlacement = 0f;
    private float crouchPlacement = -0.13f;

    //Ground and Ceiling check radius
    [Header("Ground and Ceiling check Radius")]
    [SerializeField]
    private float groundDistance = 0.4f;
    [SerializeField]
    private float ceilingDistance = 0.5f;

    //Gravity
    [Header("Gravity Variables")]
    [SerializeField]
    private float gravity = 9.81f;
    [SerializeField]
    private float gravityMultiplier = 2;
    [SerializeField]
    private float groundedGravity = 0;
    [SerializeField]
    private float maxGravity = -30f;
    [SerializeField]
    public Vector3 velocity;

    public bool canCrouch = false;
    public bool canRun = true;

   

    void Update()
    {

        if (GameManager.GM.State == GameState.Gameplay)
        {
            Movement();
            Gravity();
            if (canCrouch)
            {
                HandleCrouching();
            }        
        }
        
    }

    void Movement()
    {
        
        Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 move = transform.right * inputVector.x + transform.forward * inputVector.z;
        cc.Move(move.normalized * (currentSpeed * Time.deltaTime));

        if (IsGrounded())
        {
            var ccVelocity = cc.velocity;
            IsWalking = ccVelocity.magnitude > 0.2f;
            IsSprinting = ccVelocity.magnitude > 3f;
        }

        if (IsCrouching || CrouchCheck()) return;
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && canRun)
        {

            if (currentSpeed >= sprintSpeed)
            {
                currentSpeed = sprintSpeed;
            }
            else if (IsSprinting)
            {
                currentSpeed += Time.deltaTime * sprintMultiplier;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.S))
            {
                currentSpeed = crouchSpeed;
            }
            else
            {
                currentSpeed = walkSpeed;
            }
            
        }
    }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
    }

    void Gravity()
    {
        //velocity.y += isGrounded() ? groundedGravity : -(gravity * gravityMultiplier * Time.deltaTime);
        
        if (!IsGrounded())
        {
            
            if (velocity.y <= maxGravity)
            {
                print("MaxGravity");
                velocity.y = maxGravity;
            }
            else
            {
                velocity.y += -(gravity * gravityMultiplier) * Time.deltaTime;
            }
            
        }
        else
        {
            velocity.y = groundedGravity;
        }
        cc.Move(velocity * Time.deltaTime);
    }

    
    //CC center
    private readonly Vector3 standCenter = new Vector3(0f,-0.33f,0f);
    private readonly Vector3 crouchCenter = new Vector3(0f,-0.36f, 0f); //-0.5775f
    
    //CC height
    private readonly float standingHeight = 2f;
    private readonly float crouchHeight = 1.5f;
    private float currentHeight;
    

    void HandleCrouching()
    {
        cc.height = currentHeight;
        IsCrouching = Input.GetButton("Crouch");

        if (!IsCrouching && !CrouchCheck())
        {
            if (CrouchCheck()) return;
            crouchPlacementTime = 0;
            standPlacementTime += 3f * Time.deltaTime;

            cc.center = standCenter;
            currentHeight = standingHeight;
            playerBody.localPosition = new Vector3(0f, -1.317f, Mathf.Lerp(crouchPlacement, standPlacement, standPlacementTime));
        }
        else
        {
            standPlacementTime = 0;
            crouchPlacementTime += 3f * Time.deltaTime;

            currentSpeed = crouchSpeed;
            currentHeight = crouchHeight;
            cc.center = new Vector3(0, Mathf.Lerp(standCenter.y, crouchCenter.y, crouchPlacementTime*1f),0);
            playerBody.localPosition = new Vector3(0f, -1.317f, Mathf.Lerp(0, crouchPlacement, crouchPlacementTime));
        }

    }

    public bool CrouchCheck()
    {      
        return Physics.CheckSphere(ceilingCheck.position, ceilingDistance, groundMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawSphere(groundCheck.position, groundDistance);
        Gizmos.color = CrouchCheck() ? Color.red : Color.blue;
        Gizmos.DrawSphere(ceilingCheck.position, ceilingDistance);
    }

}
