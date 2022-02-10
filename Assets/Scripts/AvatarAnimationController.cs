using UnityEngine;
using UnityEngine.InputSystem;

public class AvatarAnimationController : MonoBehaviour
{
    [SerializeField] private InputActionReference move;

    [SerializeField] private Animator animator;


    private void OnEnable()
    {
        this.move.action.started += this.AnimateLegs;
        this.move.action.canceled += this.StopAnimation;
    }

    private void OnDisable()
    {
        this.move.action.started -= this.AnimateLegs;
        this.move.action.canceled -= this.StopAnimation;
    }
    

    private void AnimateLegs(InputAction.CallbackContext obj)
    {
        bool isWalkingFoward = this.move.action.ReadValue<Vector2>().y > 0;

        if (isWalkingFoward )
        {
            this.animator.SetBool("isMoving", true);
            this.animator.SetFloat("animSpeed", 1.0f);
        }
        else
        {
            this.animator.SetBool("isMoving", true);
            this.animator.SetFloat("animSpeed", -1.0f);
        }
    }

    private void StopAnimation(InputAction.CallbackContext obj)
    {
        this.animator.SetBool("isMoving", false);
        this.animator.SetFloat("animSpeed", 0.0f);
    }



}
