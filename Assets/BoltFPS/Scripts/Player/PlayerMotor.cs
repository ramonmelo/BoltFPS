using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS.Player
{
	[RequireComponent(typeof(CharacterController))]
	public class PlayerMotor : MonoBehaviour
	{
		#region Public Members

		public Vector2 InputDirection
		{
			get { return _input; }
		}
		public Vector3 MoveDirection
		{
			get { return _moveDir; }
		}

		public float CurrentSpeed
		{
			get { return _currentSpeed; }
		}

		#endregion

		#region Private Members

		[SerializeField] private float walkSpeed = 5;
		[SerializeField] private float runSpeed = 10;
		[SerializeField] private float jumpSpeed = 10;
		[SerializeField] private float stickToGroundForce = 10;
		[SerializeField] private float gravityMultiplier = 2;
		[SerializeField] private PlayerLook playerLook;

		private float _currentSpeed;
		private bool _isWalking;
		private Camera _camera;
		private bool _jump;
		private bool _jumping;
		private Vector2 _input;
		private Vector3 _moveDir = Vector3.zero;
		private CharacterController _cc;
		private bool _previousGrounded;
		private CollisionFlags _collisionFlags;

		#endregion

		private void Start()
		{
			_cc = GetComponent<CharacterController>();
			_camera = Camera.main;

			playerLook.Init(transform, _camera.transform);
		}

		private void Update()
		{
			RotateView();
			ProcessGrounded();
		}

		private void FixedUpdate()
		{
			GetInput();

			Vector3 desiredMove = transform.forward * _input.y + transform.right * _input.x;

			// Get normal of surface
			RaycastHit hit;

			Physics.SphereCast(transform.position - Vector3.up, _cc.radius, Vector3.down, out hit, _cc.height,
				Physics.AllLayers, QueryTriggerInteraction.Ignore);

			desiredMove = Vector3.ProjectOnPlane(desiredMove, hit.normal).normalized;

			_moveDir.x = desiredMove.x * _currentSpeed;
			_moveDir.z = desiredMove.z * _currentSpeed;

			if (_cc.isGrounded)
			{
				_moveDir.y = -stickToGroundForce;

				if (_jump)
				{
					_moveDir.y = jumpSpeed;
					_jump = false;
					_jumping = true;
				}
			}
			else
			{
				_moveDir += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
			}

			_collisionFlags = _cc.Move(_moveDir * Time.fixedDeltaTime);
		}

		Vector2 _inputVelocity = new Vector2();
		readonly float _inputSmoothSpeed = 0.1f;
		Vector2 _inputDeadEnd = new Vector2(0.1f, 0.1f);

		private void GetInput()
		{
			var horizontal = Input.GetAxisRaw("Horizontal");
			var vertical = Input.GetAxisRaw("Vertical");

			_input = Vector2.SmoothDamp(_input, new Vector2(horizontal, vertical), ref _inputVelocity, _inputSmoothSpeed);

			if (Math.Abs(_input.x) < _inputDeadEnd.x) { _input.x = 0; }
			if (Math.Abs(_input.y) < _inputDeadEnd.y) { _input.y = 0; }

			_isWalking = Input.GetKey(KeyCode.LeftShift) == false;

			_currentSpeed = _isWalking ? walkSpeed : runSpeed;
		}

		private void ProcessGrounded()
		{
			// If it is not jumping, we can jump again
			if (_jump == false)
			{
				_jump = Input.GetButtonDown("Jump");
			}

			// If was not at ground, and now it's grounded
			// we can jump, and no Y move should be applied
			if (_previousGrounded == false && _cc.isGrounded)
			{
				_moveDir.y = 0f;
				_jumping = false;
			}

			// If it's falling, no Y move should be applied 
			if (_cc.isGrounded == false && _jumping == false && _previousGrounded)
			{
				_moveDir.y = 0f;
			}

			_previousGrounded = _cc.isGrounded;
		}

		private void RotateView()
		{
			playerLook.LookRotation(transform, _camera.transform);
		}
	}
}