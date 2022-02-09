using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AvatarAnimationController : MonoBehaviour
{
    [SerializeField] private InputActionReference move;

    private Animator animator;

    private void Awake()
    {
        this.move.action.started += AnimateLegs;
        this.move.action.canceled += StopAnimation;
    }

    private void StopAnimation(InputAction.CallbackContext obj)
    {
        animator.SetBool("IsMoving", false);

    }

    private void Start()
    {
        this.animator = GetComponent<Animator>();
    }

    private void AnimateLegs(InputAction.CallbackContext obj)
    {
        animator.SetBool("IsMoving", true);
    }

    private void OnDisable()
    {
        move.action.performed -= AnimateLegs;
        move.action.canceled -= StopAnimation;
    }

}
