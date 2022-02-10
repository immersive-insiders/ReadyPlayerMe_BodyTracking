using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class AvatarAnimationController : MonoBehaviour
{
    [SerializeField] private InputActionReference move;

    private Animator animator;

    private void Awake()
    {
        this.move.action.started += AnimateLegs;
        this.move.action.canceled += StopAnimation;
    }

    private void Start()
    {
        this.animator = GetComponent<Animator>();
    }
    private void OnDisable()
    {
        move.action.started -= AnimateLegs;
        move.action.canceled -= StopAnimation;
    }

    private void AnimateLegs(InputAction.CallbackContext obj)
    {
        if(move.action.ReadValue<Vector2>() == new Vector2(0.0f,1.0f) )
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
