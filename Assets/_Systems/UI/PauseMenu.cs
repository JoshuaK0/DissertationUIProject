using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading and quitting

public class PauseMenu : MonoBehaviour
{
	public GameObject pauseMenuUI; // Assign this in the Inspector with your pause menu panel
	public GameObject disableUI;

	public UIEnableDisable settingsResume;

	private bool isPaused = false;

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (isPaused)
			{
				ResumeGame();
			}
			else
			{
				PauseGame();
			}
		}
	}

	public void ResumeGame()
	{
		settingsResume.OnClick();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		pauseMenuUI.SetActive(false);
		Time.timeScale = 1f; // Resume normal time
		isPaused = false;
		disableUI.SetActive(true);
	}

	void PauseGame()
	{
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
		pauseMenuUI.SetActive(true);
		Time.timeScale = 0f; // Freeze time
		isPaused = true;
		disableUI.SetActive(false);

	}

	public void QuitGame()
	{
		Debug.Log("Quitting game...");
		// Add your quit logic here. For editor, use UnityEditor.EditorApplication.isPlaying = false;
		// For a build, use Application.Quit();
/*		if (Application.isEditor)
		{
			UnityEditor.EditorApplication.isPlaying = false;
		}
*/		/*else
		{
			Application.Quit();
		}*/
		
		Application.Quit();
	}
}
