using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : MonoBehaviour
{
	[SerializeField] PauseMenu pauseMenu;
	public void OnRestart()
	{
		pauseMenu.ResumeGame();
		FindObjectOfType<LevelLogicUIController>().OnClickRestartLevel();
	}
}
