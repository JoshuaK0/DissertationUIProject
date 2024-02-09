using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeDifficulty : MonoBehaviour
{
	[SerializeField] float amount;

	[SerializeField] GameObject[] otherImages;
	[SerializeField] GameObject ownImages;

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelLoaded;
	}

	void Start()
	{
		if (DifficultyDatabase.Instance.GetDifficulty() != amount)
		{
			ownImages.SetActive(false);
		}
		else
		{
			ownImages.SetActive(true);
		}
	}

	private void OnLevelLoaded(Scene scene, LoadSceneMode loadSceneMode)
	{
		if (DifficultyDatabase.Instance.GetDifficulty() != amount)
		{
			ownImages.SetActive(false);
		}
		else
		{
			ownImages.SetActive(true);
		}
	}

	public void OnChange()
	{
		DifficultyDatabase.Instance.SetDifficulty(amount);
		foreach (GameObject image in otherImages)
		{
			image.SetActive(false);
		}
		ownImages.SetActive(true);
	}
}
