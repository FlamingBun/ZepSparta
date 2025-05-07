using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : BaseController
{
    private MessageTrigger messageTrigger;
    private bool isOnTrigger = false;

    [SerializeField] protected SpriteRenderer rideRenderer;

    [Header("Jump")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform shadowTransform;
    private bool isJumping = false;
    [SerializeField] private float gravityScale = 9.8f;
    [SerializeField] private float jumpPower = 5.0f;
    [SerializeField] private float groundOffset = 0.0f;

    private float currentJumpPower;
    private bool isGrounded = false;

    [Header("Items")]
    [SerializeField] private ItemSO lookItem;
    [SerializeField] private ItemSO rideItem;

    private bool isRiding = false;

    private Action<int> OnChangeSpeed;

    public void Initialize()
    {
        isRiding = false;

        DataManager.Instance.LoadPlayerData();
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

    protected override void Movement(Vector2 direction)
    {
        direction = direction * (statDetailsSO.Speed + rideItem.Speed + lookItem.Speed);
        _rigidbody.velocity = direction;
        if (isRiding)
        {
            animationHandler.Ride(direction);

        }
        else
        {
            animationHandler.Move(direction);
        }
    }

    public void EquipItem(ItemSO newItem)
    {

        if (newItem.itemType == ItemType.Look)
        {
            lookItem = newItem;
            characterRenderer.sprite = lookItem.sprite;
            animationHandler.SetPlayerAnimator(lookItem.animator);

        }
        else
        {
            rideItem = newItem;

            if (newItem.name == "기본")
            {
                isRiding = false;
                groundOffset = 0.0f;
                rideRenderer.sprite = null;
            }
            else
            {
                isRiding = true;
                groundOffset = 0.8f;
                rideRenderer.sprite = rideItem.sprite;
            }

            animationHandler.SetRideAnimator(rideItem.animator);
        }

        DataManager.Instance.SavePlayerData(lookItem.itemId, rideItem.itemId);
        OnChangeSpeed?.Invoke((int)(statDetailsSO.Speed + rideItem.Speed + lookItem.Speed));
    }


    public int GetSpeed()
    {
        return (int)(statDetailsSO.Speed + rideItem.Speed + lookItem.Speed);
    }

    public void AddSpeedChangeEvent(Action<int> action)
    {
        OnChangeSpeed += action;
    }

    protected override void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        bool isLeft = Mathf.Abs(rotZ) > 90f;

        characterRenderer.flipX = isLeft;

        if (isRiding)
        {
            rideRenderer.flipX = isLeft;
        }
    }
}
