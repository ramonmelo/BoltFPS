using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	[Range(0.01f, 1)]
	public float AnimationDamping;

	[Range(1, 5)]
	public float Speed;

	[Range(1, 5)]
	public float RotationSpeed;

	public Transform GroundCheck;

	[Range(0.1f, 1f)]
	public float GroundCheckDistance;

	[Range(0.1f, 10f)]
	public float JumpHeight;

	public Vector3 Drag;

	private const string HorizontalAxis = "Horizontal";
	private const string VerticalAxis = "Vertical";

	private CharacterController _controller;
	private Animator _animator;
	private Vector3 _velocity;
	private float _gravity;
	private bool _isGrounded;

	private Vector3 _lookDirection;

	private readonly int _animatorHorizontalParam = Animator.StringToHash("Horizontal");
	private readonly int _animatorVerticalParam = Animator.StringToHash("Vertical");

	private void Awake()
	{
		_controller = GetComponent<CharacterController>();
		_animator = GetComponentInChildren<Animator>();
	}

	private void Start()
	{
		_gravity = Physics.gravity.y;
	}

	private void Update()
	{
		float verticalAxis = Input.GetAxisRaw(VerticalAxis);
		float horizontalAxis = Input.GetAxisRaw(HorizontalAxis);

		_animator.SetFloat(_animatorVerticalParam, verticalAxis, AnimationDamping, Time.deltaTime);
		_animator.SetFloat(_animatorHorizontalParam, horizontalAxis, AnimationDamping, Time.deltaTime);

		var forwardMove = transform.forward * verticalAxis * Speed * Time.deltaTime;
		var sidewayMove = transform.right * horizontalAxis * Speed * Time.deltaTime;

		_controller.Move(forwardMove);
		_controller.Move(sidewayMove);

		//var move = new Vector3(horizontalAxis, 0, verticalAxis).normalized;

		//// Reset Gravity
		_isGrounded = Physics.CheckSphere(GroundCheck.position, GroundCheckDistance);
		if (_isGrounded && _velocity.y < 0)
		{
			_velocity.y = 0;
		}

		//if (_isGrounded && Input.GetButtonDown("Jump"))
		//{
		//	_velocity.y += Mathf.Sqrt(JumpHeight * -2f * _gravity);
		//}

		// Apply Velocity

		_velocity.y += _gravity * Time.deltaTime;
		_controller.Move(_velocity * Time.deltaTime);

		return;

		//if (move != Vector3.zero)
		//{
		//	_lookDirection = Vector3.RotateTowards(transform.forward, move, RotationSpeed * Time.deltaTime, 0f);
		//	transform.rotation = Quaternion.LookRotation(_lookDirection);
		//}

		//_velocity.x /= 1 + Drag.x * Time.deltaTime;
		//_velocity.y /= 1 + Drag.y * Time.deltaTime;
		//_velocity.z /= 1 + Drag.z * Time.deltaTime;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.white;

		if (GroundCheck)
		{
			Gizmos.DrawWireSphere(GroundCheck.position, GroundCheckDistance);
		}

		Gizmos.DrawLine(transform.position, transform.position + _lookDirection);

		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(transform.position, transform.position + (transform.up * JumpHeight));
		Gizmos.DrawWireCube(transform.position + (transform.up * JumpHeight), new Vector3(0.5f, 0.001f, 0.5f));
	}
}
