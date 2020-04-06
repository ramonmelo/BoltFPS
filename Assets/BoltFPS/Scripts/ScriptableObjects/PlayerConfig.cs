using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "PlayerConfig", menuName = "BoltFPS/PlayerConfig", order = 0)]
public class PlayerConfig : ScriptableObject
{
	public enum Sex
	{
		Male, Female
	}

	[Serializable]
	public struct Color
	{
		public string name;
		public Material material;
	}

	public Color CurrentColor
	{
		get
		{
			if (colorList == null || colorList.Length == 0 || SelectedColor < 0)
			{
				return new Color { name = "None", material = null };
			}

			return colorList[SelectedColor];
		}
	}

	public Sex sex;
	public Color[] colorList;
	public int SelectedColor = -1;
}
