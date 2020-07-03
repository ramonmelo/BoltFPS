using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
	[SerializeField] private bool ActiveFullBody;

	[SerializeField] private SkinnedMeshRenderer FullBodySkin;
	[SerializeField] private SkinnedMeshRenderer OnlyArmsSkin;
	[SerializeField] private Material[] _skins;

	private int _currentSkin = 0;

	private void Awake()
	{
		Setup(_currentSkin);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			NextSkin();
		}

		if (Input.GetKeyDown(KeyCode.O))
		{
			ActiveFullBody = !ActiveFullBody;

			FullBodySkin.gameObject.SetActive(ActiveFullBody);
			OnlyArmsSkin.gameObject.SetActive(!ActiveFullBody);
		}
	}

	void NextSkin()
	{
		_currentSkin++;
		if (_currentSkin == _skins.Length)
		{
			_currentSkin = 0;
		}
		Setup(_currentSkin);
	}

	#region Setup

	private void Setup(int index)
	{
		SetupMaterial(_skins[index]);
	}

	private void SetupMaterial(Material material)
	{
		FullBodySkin.material = material;
		OnlyArmsSkin.material = material;
	}

	#endregion
}
