using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("IsMove");
    private static readonly int IsRiding = Animator.StringToHash("IsRide");
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator rideAnimator;

    public void Move(Vector2 obj)
    {
        playerAnimator.SetBool(IsMoving, obj.magnitude > .5f);
    }

    public void Ride(Vector2 obj)
    {
        playerAnimator.SetBool(IsMoving, false);
        rideAnimator.SetBool(IsRiding, obj.magnitude > .5f);
    }

    public void SetPlayerAnimator(RuntimeAnimatorController _animator)
    {
        playerAnimator.runtimeAnimatorController = _animator;
    }
    public void SetRideAnimator(RuntimeAnimatorController _animator)
    {
        rideAnimator.runtimeAnimatorController = _animator;
    }
}
