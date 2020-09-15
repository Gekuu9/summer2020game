using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimator : MonoBehaviour
{
    const float locomotionAnimationSmoothTime = 0.1f;

    Animator animator;
    Player player;

    private void Start() {
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update() {
        if (player.moveVelocity != Vector3.zero) {
            animator.SetFloat("Blend", 1f, locomotionAnimationSmoothTime, Time.deltaTime);
        } else {
            animator.SetFloat("Blend", 0f, locomotionAnimationSmoothTime, Time.deltaTime);
        }
    }
}
