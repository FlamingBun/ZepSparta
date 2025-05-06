using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    private Camera camera;
    private MessageTrigger messageTrigger;
    private bool isOnTrigger = false;

    [Header("Jump")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform shadowTransform;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private float gravityScale = 9.8f;
    [SerializeField] private float jumpPower = 5.0f;
    [SerializeField] private float groundOffset = 0.0f;
    [SerializeField] private UnityEvent grounded = new UnityEvent();

    private float currentJumpPower;
    private bool isGrounded = false;

    public UnityEvent Grounded => grounded;
    private bool IsGrounded => isGrounded;
    public float GravityScale => gravityScale;

    public void Init()
    {
        camera = Camera.main;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (isGrounded == true)
            return;

        playerTransform.transform.position += new Vector3(0.0f, currentJumpPower * Time.fixedDeltaTime, 0.0f);

        currentJumpPower -= gravityScale * Time.fixedDeltaTime;

        if (playerTransform.position.y <= shadowTransform.position.y + groundOffset && currentJumpPower <= 0.0f)
        {
            isGrounded = true;
            isJumping = false;
            playerTransform.position = new Vector2(playerTransform.position.x, shadowTransform.position.y + groundOffset);
            grounded?.Invoke();
        }

    }

    private void OnMove(InputValue inputValue)
    {
        movementDirection = inputValue.Get<Vector2>().normalized;
        lookDirection = movementDirection;
    }

    private void OnJump()
    {
        if (isOnTrigger && messageTrigger != null)
        {
            messageTrigger.OnClickSpaceBar();
        }
        else
        {
            if (!isJumping)
            {
                Jump(jumpPower);
            }
        }
    }

    public void SetTriggerOn(MessageTrigger _messageTrigger)
    {
        isOnTrigger = true;
        messageTrigger = _messageTrigger;
    }

    public void SetTriggerOff()
    {
        isOnTrigger = false;
        messageTrigger = null;
    }

    public void Jump(float jumpPower)
    {
        this.currentJumpPower = jumpPower;
        isJumping = true;
        isGrounded = false;
    }
}
