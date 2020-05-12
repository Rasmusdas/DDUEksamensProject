using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public GameObject controls;
    public GameObject startScreen;
    public GameObject loading;

    private void Start()
    {
        controls.SetActive(false);
        loading.SetActive(false);
    }

    public void ChangeScreens()
    {
        controls.SetActive(!controls.activeInHierarchy);
        startScreen.SetActive(!startScreen.activeInHierarchy);
    }

    public void StartGame(int i)
    {
        loading.SetActive(true);
        startScreen.SetActive(false);
        PauseMenu.GoToScene(i);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
