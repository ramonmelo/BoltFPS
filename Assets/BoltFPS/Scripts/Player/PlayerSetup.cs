using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
	[SerializeField] private PlayerConfig[] _playerConfig;
	[SerializeField] private GameObject[] _maleVisuals;
	[SerializeField] private GameObject[] _femaleVisuals;

	private int _currentConfig = 0;

	private void Awake()
	{
		Setup(_currentConfig);
	}

	private void Start()
	{
		InvokeRepeating("NextSkin", 5, 5);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			NextSkin();
		}
	}

	void NextSkin()
	{
		_currentConfig++;
		if (_currentConfig == _playerConfig.Length)
		{
			_currentConfig = 0;
		}
		Setup(_currentConfig);
	}

	#region Setup

	private void Setup(int index)
	{
		Setup(_playerConfig[index].sex, _playerConfig[index].CurrentColor);
	}

	private void Setup(PlayerConfig.Sex sex, PlayerConfig.Color currentColor)
	{
		switch (sex)
		{
			case PlayerConfig.Sex.Male:
				SetupVisuals(_femaleVisuals, _maleVisuals, currentColor);
				break;
			case PlayerConfig.Sex.Female:
				SetupVisuals(_maleVisuals, _femaleVisuals, currentColor);
				break;
		}
	}

	private void SetupVisuals(GameObject[] disableVisuals, GameObject[] applyMaterialVisuals, PlayerConfig.Color currentColor)
	{
		foreach (var go in disableVisuals)
		{
			go.SetActive(false);
		}

		foreach (var go in applyMaterialVisuals)
		{
			go.SetActive(true);
		}

		if (currentColor.material != null)
		{
			foreach (var go in applyMaterialVisuals)
			{
				var skinRenderer = go.GetComponent<SkinnedMeshRenderer>();
				if (skinRenderer != null)
				{
					skinRenderer.material = currentColor.material;
				}

				var meshRenderer = go.GetComponent<MeshRenderer>();
				if (meshRenderer != null)
				{
					meshRenderer.material = currentColor.material;
				}
			}
		}
	}

	#endregion
}
