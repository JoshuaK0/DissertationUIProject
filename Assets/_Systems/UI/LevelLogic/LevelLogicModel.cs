using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLogicModel : MonoBehaviour
{
	[SerializeField] string levelName;

	public string GetCurrentLevel()
	{
		return levelName;
	}
}
