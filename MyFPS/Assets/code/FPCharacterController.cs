using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCharacterController : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 tmp_MovementDirection;
    private Animator characterAnimator;
    private bool isCrouch;
    private float originHeight;
    private float Velocity;
    
    public float WalkSpeed;
    public float RunSpeed;
    public float CrouchSpeed;
    public float CrouchShiftSpeed;
    public float Jump;
    public float gravity = 9.8f;
    public float CrouchHeight = 1f;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        // characterAnimator = GetComponentInChildren<Animator>();
        originHeight = characterController.height;
    }

    void Update()
    {
        float speed = WalkSpeed;
        if (characterController.isGrounded)
        {
            var tmp_Horizontal = Input.GetAxis("Horizontal");
            var tmp_Vertical = Input.GetAxis("Vertical");
            tmp_MovementDirection = 
                transform.TransformDirection(new Vector3(tmp_Horizontal, 0, tmp_Vertical));
            
            if (Input.GetButtonDown("Jump")) tmp_MovementDirection.y = Jump;
            if (Input.GetKey(KeyCode.LeftShift)) speed = RunSpeed;
            if (Input.GetKeyDown(KeyCode.C))
            {   
                var tmp_CurrentHight = isCrouch ? originHeight : CrouchHeight;
                StartCoroutine(DoCrouch(tmp_CurrentHight));
                isCrouch = !isCrouch;
            }

            if (isCrouch)
            {
                speed = Input.GetKey(KeyCode.LeftShift) ? CrouchShiftSpeed : CrouchSpeed;
            }
            else
            {
                speed = Input.GetKey(KeyCode.LeftShift) ? RunSpeed : WalkSpeed;
            }
            
            // 调整动画状态
            var tmp_velocity = characterController.velocity;
            tmp_velocity.y = 0;
            if (characterAnimator != null) characterAnimator.SetFloat("Velocity",
                tmp_velocity.magnitude, 0.25f, Time.deltaTime);
            
            // Debug.Log(characterAnimator.velocity);
        }

        tmp_MovementDirection.y -= gravity * Time.deltaTime;
        characterController.Move(speed*Time.deltaTime*tmp_MovementDirection);
    }

    private IEnumerator DoCrouch(float _target)
    {
        float tmp_CurrentHeight = 0;
        while (Mathf.Abs(characterController.height - _target) > 0.1f)
        {
            yield return null;
            characterController.height = Mathf.SmoothDamp(characterController.height, _target,
                ref tmp_CurrentHeight, Time.deltaTime*5);
        }
    }

    internal void SetupAnimator(Animator _animator)
    {
        characterAnimator = _animator;
    }
}
