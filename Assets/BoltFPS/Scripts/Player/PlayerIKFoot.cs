using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerIKFoot : MonoBehaviour
{
	[Range(0f, 1f), SerializeField] private float DistanceToGround;
	[SerializeField] private LayerMask GroundMask;

	[SerializeField] private bool EnableFootPositionIK;
	[SerializeField] private bool EnableFootRotationIK;
	[Range(0f, 1f), SerializeField] private float FootRotationWeight;

	private Animator _animator;
	private CharacterController _cc;

	private void Start()
	{
		_animator = GetComponent<Animator>();
		_cc = GetComponent<CharacterController>();
	}

	private void OnAnimatorIK(int layerIndex)
	{
		if (_cc.isGrounded == false) { return; }
		if (EnableFootPositionIK == false && EnableFootRotationIK == false) { return; }
		if (_animator == null) { return; }

		CalculateIKFoot(AvatarIKGoal.LeftFoot);
		CalculateIKFoot(AvatarIKGoal.RightFoot);
	}

	private void CalculateIKFoot(AvatarIKGoal goal)
	{
		_animator.SetIKPositionWeight(goal, 1f);
		_animator.SetIKRotationWeight(goal, 1f);

		var ray = new Ray(_animator.GetIKPosition(goal) + Vector3.up, Vector3.down);

		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, DistanceToGround + 5f, GroundMask))
		{
			if (EnableFootPositionIK)
			{
				var footPosition = hit.point;
				footPosition.y += DistanceToGround;

				_animator.SetIKPosition(goal, footPosition);
			}

			if (EnableFootRotationIK)
			{
				Vector3 rotAxis = Vector3.Cross(Vector3.up, hit.normal);
				float angle = Vector3.Angle(Vector3.up, hit.normal);
				var feetIKRotation = Quaternion.AngleAxis(angle * FootRotationWeight, rotAxis);

				var footRotation = Quaternion.LookRotation(transform.forward, hit.normal);

				_animator.SetIKRotation(goal, feetIKRotation * _animator.GetIKRotation(goal));
			}
		}
	}
}
