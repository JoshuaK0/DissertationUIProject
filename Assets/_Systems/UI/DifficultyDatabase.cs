using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyDatabase : MonoBehaviour
{
	private static DifficultyDatabase _instance;

	public static DifficultyDatabase Instance { get { return _instance; } }

	float difficulty = 10;


	void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
	}

	public void SetDifficulty(float newDifficulty)
	{
		difficulty = newDifficulty;
	}

	public float GetDifficulty()
	{
		return difficulty;
	}
}
