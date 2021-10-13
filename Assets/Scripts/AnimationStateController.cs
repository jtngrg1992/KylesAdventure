using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationStateController : MonoBehaviour
{
    private int velocityXHash;
    private int velocityYHash;
    private float velocityX = 0.0f;
    private float velocityY = 0.0f;
    private float acceleration = 1f;

    private Animator animator;

    void Awake()
    {
        velocityXHash = Animator.StringToHash("VelocityX");
        velocityYHash = Animator.StringToHash("VelocityY");
        animator = GetComponent<Animator>();
    }

    public void ProcessDirectionalVector(Vector2 direction, bool isSprinting)
    {
        float xDirection = direction.x;
        float yDirection = direction.y;

        float adjustedXDirection = (isSprinting ? xDirection * 2 : xDirection);
        float adjustedYDirection = (isSprinting ? yDirection * 2 : yDirection);


        animator.SetFloat(velocityXHash, adjustedXDirection, 0.1f, 0.1f);
        animator.SetFloat(velocityYHash, adjustedYDirection, 0.1f, 0.1f);
    }

}
