using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject map;

    bool paused;
    private void Start()
    {
        pauseMenu.SetActive(false);
        map.SetActive(false);
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        if(paused)
        {
            map.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            map.gameObject.SetActive(!map.gameObject.activeSelf);
        }
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void PauseGame()
    {
        paused = !paused;
        pauseMenu.SetActive(paused);
        Time.timeScale = paused ? 0 : 1;
    }

    public static void GoToScene(int index)
    {
        SceneManager.LoadSceneAsync(index);
    }

    public static void GoToScene(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }

    public void ChangeScenorino(int i)
    {
        GoToScene(i);
    }

}
