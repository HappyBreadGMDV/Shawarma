using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Rigidbody rigidbody;
    public Animator animator;
    public float MinSpeed;
    public string FloatAnimation;
    public float WalkAnim;
    public float IdleAnim;

    private void FixedUpdate()
    {
        if (rigidbody.velocity.magnitude <= MinSpeed)
        {
            animator.SetFloat(FloatAnimation, WalkAnim);
        }

        else
        {
            animator.SetFloat(FloatAnimation, IdleAnim);
        }
    }
}
