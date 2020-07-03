using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIKHandler : MonoBehaviour
{
	public Transform RightHandIK
	{
		get { return rightHandIK; }
	}

	public Transform LeftHandIK
	{
		get { return leftHandIK; }
	}

	[SerializeField] private Transform rightHandIK;
	[SerializeField] private Transform leftHandIK;
}
