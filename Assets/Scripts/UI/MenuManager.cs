using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void SwitchScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}