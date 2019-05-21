using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    public Transform[] Waypoints;
    //public Transform m_waypoint1;
    //public Transform m_waypoint2;
    //public Transform m_waypoint3;
    //public Transform m_waypoint4;
    public Text m_waveText;
    public Text m_remainingEnemyText;
    public GameObject m_enemyPrefab;

    public float m_gameStartTimer = 3f;
    public float m_waveTimer = 5f;

    private float m_nextWaveTimer;
    private int m_waveCounter;

    private bool m_waveInProgress = false;

    // Start is called before the first frame update
    void Start()
    {
        m_nextWaveTimer = m_gameStartTimer;
        m_waveCounter = 0;

    }

    // Update is called once per frame
    void Update()
    {
        m_nextWaveTimer -= Time.deltaTime;

        if (m_nextWaveTimer <= 0)
        {
            if (!m_waveInProgress)
            {
                SendNextWave();
            }
        }
    }

    private void SendNextWave()
    {
        m_waveInProgress = true;
        m_waveCounter++;
        m_waveText.gameObject.SetActive(true);
        m_waveText.text = "Wave: " + m_waveCounter;
        m_remainingEnemyText.gameObject.SetActive(true);

        for (int i = 0; i <= m_waveCounter; i++)
        {
            int r = Random.Range(0, 4);
            GameObject go = Instantiate(m_enemyPrefab, Waypoints[r].position, Waypoints[r].rotation);
        }
    }
}
