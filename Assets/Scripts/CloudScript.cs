using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CloudScript : MonoBehaviour
{
    [Header("Rb")]
    public Rigidbody2D rb;
    public float power;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.5f);
    public LayerMask groundLayer;
    public bool isGrounded;

    float time = 0.3f;
    float timer;

    bool isJumping;

    void Update()
    {
        GroundCheck();
        if (timer <= 0)
        {
            isJumping = false;
            timer = time;
        }
        if (timer> 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        if (isGrounded && isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, power);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        isJumping = true;
    }
}
