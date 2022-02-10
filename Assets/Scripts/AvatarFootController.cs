using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AvatarFootController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] [Range(0, 1)] private float leftFootPosWeight;
    [SerializeField] [Range(0, 1)] private float rightFootPosWeight;

    [SerializeField] [Range(0, 1)] private float leftFootRotWeight;
    [SerializeField] [Range(0, 1)] private float rightFootRotWeight;

    [SerializeField] private Vector3 footOffset;
    [SerializeField] private Vector3 raycastOffsetLeft;
    [SerializeField] private Vector3 raycastOffsRight;


    private void OnAnimatorIK(int layerIndex)
    {
        Vector3 leftFootPos = this.animator.GetIKPosition(AvatarIKGoal.LeftFoot);
        Vector3 rightFootPos = this.animator.GetIKPosition(AvatarIKGoal.RightFoot);

        RaycastHit hitLeftFoot;
        RaycastHit hitRightFoot;


        bool isLeftFootDown = Physics.Raycast(leftFootPos + this.raycastOffsetLeft, Vector3.down, out hitLeftFoot);
        bool isRightFootDown = Physics.Raycast(rightFootPos + this.raycastOffsRight, Vector3.down, out hitRightFoot);

        if (isLeftFootDown)
        {
            this.animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, this.leftFootPosWeight);
            this.animator.SetIKPosition(AvatarIKGoal.LeftFoot, hitLeftFoot.point + this.footOffset);

            Debug.DrawRay(transform.position, transform.forward,Color.blue);
            Debug.DrawRay(hitLeftFoot.normal, hitLeftFoot.normal, Color.red);
            Debug.DrawRay(transform.forward, hitLeftFoot.normal, Color.green);


            Quaternion leftFootRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hitLeftFoot.normal), hitLeftFoot.normal);
            this.animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, this.leftFootRotWeight);
            this.animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRotation);
        }
        else
        {
            this.animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
        }

        if (isRightFootDown)
        {
            this.animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, this.rightFootPosWeight);
            this.animator.SetIKPosition(AvatarIKGoal.RightFoot, hitRightFoot.point + this.footOffset);

            Quaternion rightFootRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hitRightFoot.normal), hitRightFoot.normal);
            this.animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, this.rightFootRotWeight);
            this.animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRotation);
        }
        else
        {
            this.animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
        }
    }
}
