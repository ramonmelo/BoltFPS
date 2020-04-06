using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerConfig))]
public class PlayerConfigEditor : Editor
{
	private PlayerConfig _config;
	private int _selectedColor;

	private void OnEnable()
	{
		_config = target as PlayerConfig;
	}

	public override void OnInspectorGUI()
	{
		var colorList = serializedObject.FindProperty("colorList");
		serializedObject.Update();

		_config.sex = (PlayerConfig.Sex)EditorGUILayout.EnumPopup("Character Sex", _config.sex);


		if (_config.colorList.Length > 0)
		{
			if (_config.SelectedColor == -1) { _config.SelectedColor = 0; }

			string[] labels = Array.ConvertAll(_config.colorList, color => color.name);

			_config.SelectedColor = EditorGUILayout.Popup("Current Color", _config.SelectedColor, labels);
		}
		else
		{
			EditorGUILayout.LabelField("Populate the Color List");
			_config.SelectedColor = -1;
		}

		EditorGUILayout.PropertyField(colorList, true);

		// Save
		serializedObject.ApplyModifiedProperties();
	}
}
