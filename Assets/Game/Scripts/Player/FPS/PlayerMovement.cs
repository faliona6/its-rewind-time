﻿using System;
using UnityEngine;

public class PlayerMovement : ResetComponent
{

    //Assingables
    public Transform playerCam;
    public Transform orientation;
    public Transform cameras;
    
    // used for jumping on clones
    [SerializeField] private Transform feetPosition;

    //Other
    private Rigidbody rb;

    //Rotation and look
    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;

    //Movement
    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    public bool grounded;
    public LayerMask whatIsGround;

    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    //Crouch & Slide
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;

    //Jumping
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    public float jumpForce = 550f;

    //Input
    float x, y;
    bool jumping, sprinting, crouching;
    private bool canMove = true;

    //Sliding
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        base.Start();
        playerScale = transform.localScale;
        if (LevelManager.Instance.getIsRunning()) { 
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void KillPlayer()
    {
        // this is just to disable the player's controller after they die.
        canMove = false;
    }


    private void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        if (!LevelManager.Instance.getIsRunning())
            return;

        if (canMove)
        {
            MyInput();
            Look();
        }
        else
        {
            jumping = false;
            crouching = false;
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        // Check if the player stepped on a platform, if so, make the platform its parent
        PlatformCheck(other.collider);
    }

    private void PlatformCheck(Collider other)
    {
        if (other.CompareTag("Clone"))
        {
            transform.parent.parent = other.transform;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        // unparent the player from the platform if it leaves it
        PlatformExitCheck(other.collider);
    }

    private void PlatformExitCheck(Collider other)
    {
        if (other.CompareTag("Clone"))
        {
            transform.parent.parent = null;
        }
    }

    /// <summary>
    /// Find user input. Should put this in its own class but im lazy
    /// </summary>
    private void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
        crouching = Input.GetKey(KeyCode.LeftControl);

        //Crouching
        /*
        if (Input.GetKeyDown(KeyCode.LeftControl))
             StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopCrouch();
        */
    }

    private void StartCrouch()
    {
        var transform1 = transform;
        transform1.localScale = crouchScale;
        var position = transform1.position;
        position = new Vector3(position.x, position.y - 0.5f, position.z);
        transform1.position = position;
        if (rb.velocity.magnitude > 0.5f)
        {
            if (grounded)
            {
                rb.AddForce(orientation.transform.forward * slideForce);
            }
        }
    }

    // Hey Fiona, I hope this doesn't cause a merge conflict, this is so
    // that I can delete the camera on this player and make it kinematic
    public override void OnReset()
    {
        Destroy(cameras.gameObject);
        var tempRb = GetComponent<Rigidbody>();
        tempRb.isKinematic = true;
        tempRb.interpolation = RigidbodyInterpolation.Interpolate;
        ChangeLayersAndTagRecursively(transform.parent, "Clone", "Clone");
        // keep the layer of the hat so that the player can jump on it.
        Destroy(this);
    }

    private void ChangeLayersAndTagRecursively(Transform playerTransform, string layerName, string tagName)
    {
        foreach(Transform child in playerTransform) {
            child.gameObject.layer = LayerMask.NameToLayer(layerName);
            child.tag = tagName;
            ChangeLayersAndTagRecursively(child, layerName, tagName);
        }
    }

    private void StopCrouch()
    {
        var transform1 = transform;
        transform1.localScale = playerScale;
        transform1.position = new Vector3(transform1.position.x, transform1.position.y + 0.5f, transform1.position.z);
    }

    private void Movement()
    {
        //Extra gravity
        rb.AddForce(Vector3.down * (Time.deltaTime * 10));

        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //Counteract sliding and sloppy movement
        CounterMovement(x, y, mag);

        //If holding jump && ready to jump, then jump
        if (readyToJump && jumping) Jump();

        //Set max speed
        float maxSpeed = this.maxSpeed;

        //If sliding down a ramp, add force down so player stays grounded and also builds speed
        if (crouching && grounded && readyToJump)
        {
            rb.AddForce(Vector3.down * (Time.deltaTime * 3000));
            return;
        }

        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        //Some multipliers
        float multiplier = 1f, multiplierV = 1f;

        // Movement in air
        if (!grounded)
        {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }

        // Movement while sliding
        if (grounded && crouching) multiplierV = 0f;

        //Apply forces to move player
        rb.AddForce(orientation.transform.forward * (y * moveSpeed * Time.deltaTime * multiplier * multiplierV));
        rb.AddForce(orientation.transform.right * (x * moveSpeed * Time.deltaTime * multiplier));
    }

    private void Jump()
    {
        if (grounded && readyToJump)
        {
            readyToJump = false;

            //Add jump forces
            rb.AddForce(Vector2.up * (jumpForce * 1.5f));
            rb.AddForce(normalVector * (jumpForce * 0.5f));

            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private float desiredX;
    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!grounded || jumping) return;

        //Slow down sliding
        if (crouching)
        {
            rb.AddForce(-rb.velocity.normalized * (moveSpeed * Time.deltaTime * slideCounterMovement));
            return;
        }

        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(orientation.transform.right * (moveSpeed * Time.deltaTime * -mag.x * counterMovement));
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(orientation.transform.forward * (moveSpeed * Time.deltaTime * -mag.y * counterMovement));
        }

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            var velocity = rb.velocity;
            float fallspeed = velocity.y;
            Vector3 n = velocity.normalized * maxSpeed;
            velocity = new Vector3(n.x, fallspeed, n.z);
            rb.velocity = velocity;
        }
    }

    /// <summary>
    /// Find the velocity relative to where the player is looking
    /// Useful for vectors calculations regarding movement and limiting movement
    /// </summary>
    /// <returns></returns>
    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        var velocity = rb.velocity;
        float moveAngle = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v)
    {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;

    /// <summary>
    /// Handle ground detection
    /// </summary>
    private void OnCollisionStay(Collision other)
    {
        //Make sure we are only checking for walkable layers
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 normal = other.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal))
            {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
        float delay = 3f;
        if (!cancellingGrounded)
        {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    private void StopGrounded()
    {
        grounded = false;
    }

    /// Used for getting the y position of feet for comparing with clones.
    public float FeetYPos
    {
        get
        {
            return feetPosition.transform.position.y;
        }
    }

}