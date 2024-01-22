using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLogicUIController : MonoBehaviour
{
	[Header ("Logic")]
	[SerializeField] string levelLogicSceneName;
	[SerializeField] LevelLogicModel model;
	[SerializeField] string exitSceneName;

	[Header("UI Components")]
	[SerializeField] GameObject winPanel;
	[SerializeField] GameObject losePanel;
	[SerializeField] GameObject losePanelButtons;
	[SerializeField] Slider loseSlider;
	[SerializeField] Image loseBackground;

	[Header("Lose Screen Params")]
	[SerializeField] float transitionSpeed;

	bool isLose = false;

	void Start()
	{
		LockCursor();
		winPanel.SetActive(false);
		losePanel.SetActive(false);
		loseSlider.value = 0;
	}

	public void OnLose()
	{
		UnlockCursor();
		winPanel.SetActive(false);
		losePanel.SetActive(true);
		isLose = true;
		
	}

	public void OnWin()
	{
		UnlockCursor();
		winPanel.SetActive(true);
		losePanel.SetActive(false);
	}
	public void OnClickRestartLevel()
	{
		LockCursor();
		string levelName = model.GetCurrentLevel();
		SceneManager.LoadScene(levelName);
	}

	public void OnClickExitLevel()
	{
		UnlockCursor();
		SceneManager.UnloadSceneAsync(levelLogicSceneName);
		SceneManager.LoadScene(exitSceneName);
	}

	void LockCursor()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void UnlockCursor()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	void Update()
	{
		if(isLose)
		{
			loseSlider.value += transitionSpeed * Time.deltaTime;
			loseBackground.color = Color.Lerp(Color.clear, Color.black, loseSlider.value);
			

			if (loseSlider.value > 0.9)
			{
				losePanelButtons.SetActive(true);
			}
		}
	}
}
