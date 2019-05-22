using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Get { get; private set; }

    public PlayerData m_player;

    public Transform[] Waypoints;
    public TMP_Text m_waveText;
    public TMP_Text m_remainingEnemyText;
    public GameObject m_enemyPrefab;

    public float m_TimeBeforeStart = 3f;
    public float m_TimeBetweenWaves = 5f;

    private float m_nextWaveTimer;
    private int m_waveCounter;
    private int m_remainingEnemies;

    private bool m_waveInProgress = false;

    private void Awake()
    {
        Get = this;
    }

    void Start()
    {
        m_nextWaveTimer = m_TimeBeforeStart;
    }

    void Update()
    {
        if (!m_waveInProgress)
        {
            m_nextWaveTimer -= Time.deltaTime;

            Debug.Log(m_nextWaveTimer);
        }

        if (m_nextWaveTimer <= 0 && !m_waveInProgress)
        {
            SendNextWave();
        }
    }

    private void SendNextWave()
    {
        m_waveInProgress = true;
        m_waveCounter++;
        m_waveText.text = $"Wave: {m_waveCounter.ToString()}";

        for (int i = 0; i <= m_waveCounter - 1; i++)
        {
            int r = Random.Range(0, Waypoints.Length);
            GameObject go = Instantiate(m_enemyPrefab, Waypoints[r].position, Waypoints[r].rotation);
        }

        m_remainingEnemies = m_waveCounter;

        m_remainingEnemyText.text = $"Enemies: {m_remainingEnemies.ToString()}";
    }

    public void EnemyDead()
    {
        m_remainingEnemies--;

        m_remainingEnemyText.text = $"Enemies: {m_remainingEnemies.ToString()}";

        if (m_remainingEnemies <= 0)
        {
            StopWave();
        }
    }

    private void StopWave()
    {
        m_nextWaveTimer = m_TimeBetweenWaves;
        m_player.Heal(50);

        m_waveInProgress = false;
    }
}
