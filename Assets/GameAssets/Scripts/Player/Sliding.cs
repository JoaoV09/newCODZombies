using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public CapsuleCollider cc;
    private Rigidbody rb;
    private PlayerMovementAdvanced pm;

    [Header("Sliding")]
    public float maxSlideTime = .75f;
    public float slideForce = 200f;
    private float slideTimer;

    public float slideYScale = .5f;
    public Vector3 slideCenter = new Vector3(0, .5f, 0);
    private float startYScale;
    private Vector3 startCenter;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float verticalInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementAdvanced>();
        cc = GetComponent<CapsuleCollider>();

        //startYScale = cc.height;
        startCenter = transform.localScale;
    }

    private void Update()
    {
        verticalInput = Input.GetAxisRaw("Vertical");


        if (Input.GetKeyDown(slideKey) && (verticalInput > 0) && rb.linearVelocity.magnitude > 3)
            StartSlide();

        if (Input.GetKeyUp(slideKey) && pm.sliding)
            StopSlide();
    }

    private void FixedUpdate()
    {
        if (pm.sliding)
            SlidingMovement();
    }

    private void StartSlide()
    {
        pm.sliding = true;

        transform.localScale = slideCenter;

        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput;

        // sliding normal
        if(!pm.OnSlope() || rb.linearVelocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }

        // sliding down a slope
        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }

        if (slideTimer <= 0)
            StopSlide();
    }

    private void StopSlide()
    {
        pm.sliding = false;

        transform.localScale = startCenter;
    }
}
