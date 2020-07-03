using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.Player
{
	[RequireComponent(typeof(PlayerMotor))]
	public class PlayerAnimatorController : MonoBehaviour
	{
		[SerializeField, Range(0.01f, 1)] private float animationDamping;
		[SerializeField] private Animator animator;

		private PlayerMotor _motor;
		private CharacterController _controller;
		private readonly int _animatorHorizontalParam = Animator.StringToHash("Horizontal");
		private readonly int _animatorVerticalParam = Animator.StringToHash("Vertical");
		private readonly int _animatorFireParam = Animator.StringToHash("Fire");
		private readonly int _animatorSpeedMultiParam = Animator.StringToHash("SpeedMulti");

		private void Start()
		{
			_motor = GetComponent<PlayerMotor>();
			_controller = GetComponent<CharacterController>();
		}

		private void Update()
		{
			var inputDirection = _motor.InputDirection;

			animator.SetFloat(_animatorSpeedMultiParam, _motor.CurrentSpeed);
			animator.SetFloat(_animatorVerticalParam, inputDirection.y);
			animator.SetFloat(_animatorHorizontalParam, inputDirection.x);
			animator.SetBool(_animatorFireParam, Input.GetMouseButton(0));
		}
	}
}