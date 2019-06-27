using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuManager : MenuManager
{
    public GameObject m_UI;
    public GameObject m_Menu;
    public GameObject m_GameOver;
    public Camera m_GameOverCam;

    private bool m_paused;

    private void Awake()
    {
        UnPause();
    }

    public void Pause()
    {
        Time.timeScale = 0;

        m_UI.SetActive(false);
        m_Menu.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        m_paused = true;
    }

    public void UnPause()
    {
        Time.timeScale = 1;

        m_UI.SetActive(true);
        m_Menu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_paused = false;
    }

    public void GameOver()
    {
        m_GameOver.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        m_GameOverCam.gameObject.SetActive(true);
    }

    public void TogglePause()
    {
        if (m_paused)
        {
            UnPause();
        }
        else
        {
            Pause();
        }
    }
}