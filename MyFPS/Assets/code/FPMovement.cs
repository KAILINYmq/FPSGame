using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPMovement : MonoBehaviour
{
    public float Speed;
    public float gravity;
    public float JumpHeight;
    private Rigidbody characterRigidbody;
    private bool isGrounded;
    
    private void Start()
    {
        characterRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {   
        // Move
        if (isGrounded)
        {
            var tmp_Horizontal =  Input.GetAxis("Horizontal");
            var tmp_Vertical =  Input.GetAxis("Vertical");

            var tmp_CurrentDirection = new Vector3(tmp_Horizontal, 0, tmp_Vertical);
            tmp_CurrentDirection = transform.TransformDirection(tmp_CurrentDirection);
            tmp_CurrentDirection *= Speed;

            var tmp_CurrentVelocity = characterRigidbody.velocity;
            var tmp_VelocityChange = tmp_CurrentDirection - tmp_CurrentVelocity;
            tmp_VelocityChange.y = 0;
            
            characterRigidbody.AddForce(tmp_VelocityChange, ForceMode.VelocityChange);

            // Jump
            if (Input.GetButtonDown("Jump"))
            {
                characterRigidbody.velocity = new Vector3(
                    tmp_CurrentVelocity.x, CalculateJumpHeightSpeed(),
                    tmp_VelocityChange.z);
            }
        }
        
        // gravity
        characterRigidbody.AddForce(new Vector3(0,-gravity*characterRigidbody.mass,0));
    }

    private float CalculateJumpHeightSpeed()
    {
        return Mathf.Sqrt(2 * gravity * JumpHeight);
    }

    private void OnCollisionStay(Collision other)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision other)
    {
        isGrounded = false;
    }
}
