using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FPS.Player
{
	[Serializable]
	public class PlayerLook
	{
		public float xSensitivity = 2f;
		public float ySensitivity = 2f;
		public bool clampVerticalRotation = true;
		public float minimumX = -90F;
		public float maximumX = 90F;

		private Quaternion _characterTargetRot;
		private Quaternion _cameraTargetRot;

		public void Init(Transform character, Transform camera)
		{
			_characterTargetRot = character.localRotation;
			_cameraTargetRot = camera.localRotation;

			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		public void LookRotation(Transform character, Transform camera)
		{
			float yRot = Input.GetAxis("Mouse X") * xSensitivity;
			float xRot = Input.GetAxis("Mouse Y") * ySensitivity;

			_characterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
			_cameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

			if (clampVerticalRotation)
			{
				_cameraTargetRot = ClampRotationAroundXAxis(_cameraTargetRot);
			}

			character.localRotation = _characterTargetRot;
			camera.localRotation = _cameraTargetRot;
		}

		private Quaternion ClampRotationAroundXAxis(Quaternion q)
		{
			q.x /= q.w;
			q.y /= q.w;
			q.z /= q.w;
			q.w = 1.0f;

			float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

			angleX = Mathf.Clamp(angleX, minimumX, maximumX);

			q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

			return q;
		}
	}
}