using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerIKHands : MonoBehaviour
{
	[SerializeField] private Transform GunRoot;

	private Animator _animator;
	private WeaponIKHandler[] _weaponIKHandlers;
	private int currentWeapon = -1;

	// Start is called before the first frame update
	void Start()
	{
		_animator = GetComponent<Animator>();

		var weaponCount = GunRoot.childCount;
		_weaponIKHandlers = new WeaponIKHandler[weaponCount];

		for (int i = 0; i < weaponCount; i++)
		{
			Transform child = GunRoot.GetChild(i);

			_weaponIKHandlers[i] = child.GetComponent<WeaponIKHandler>();

			if (child.gameObject.activeInHierarchy)
			{
				currentWeapon = i;
			}

			if (_weaponIKHandlers[i] == null)
			{
				Debug.LogWarningFormat("Weapon of index {0} has not IK Handler", i);
			}
		}

	}

	private void OnAnimatorIK(int layerIndex)
	{
		if (_animator == null) { return; }

		var handler = _weaponIKHandlers[currentWeapon];

		if (handler != null)
		{
			CalculateIKHand(AvatarIKGoal.LeftHand, handler.LeftHandIK);
			CalculateIKHand(AvatarIKGoal.RightHand, handler.RightHandIK);
		}
	}

	private void CalculateIKHand(AvatarIKGoal goal, Transform handIK)
	{
		_animator.SetIKPositionWeight(goal, 1f);
		_animator.SetIKRotationWeight(goal, 1f);

		_animator.SetIKPosition(goal, handIK.position);
		_animator.SetIKRotation(goal, handIK.rotation);
	}
}
