using UnityEngine;
using System.Collections;

public class PauseHandler : MonoBehaviour
{
    #region Public gameObject to drop to in Inspector

    public GameObject pauseMenu;

    #endregion

    bool isPaused;

    // Use this for initialization
    void Start()
    {
        if (pauseMenu == null) {
            Debug.LogError("Fail to find pause UI.");
            return;
        }

        isPaused = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                ResumeGame();
            } else {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0.0f;
        isPaused = true;

        pauseMenu.SetActive(true);
        this.gameObject.GetComponent<ThirdPersonCamera>().enabled = false;
    }

    // "Resume" button delegate
    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        isPaused = false;

        pauseMenu.SetActive(false);
        this.GetComponent<ThirdPersonCamera>().enabled = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
