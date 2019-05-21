using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static PlayerData m_Player;

    private static List<Enemy> m_activeEnemies = new List<Enemy>();

    public static void AddEnemy(Enemy _enemy)
    {
        if (m_activeEnemies.Contains(_enemy))
        {
            return;
        }

        if (m_Player == null)
        {
            m_Player = FindObjectOfType<PlayerData>();
        }

        _enemy.Instantiate(m_Player);
        m_activeEnemies.Add(_enemy);
    }

    public static void RemoveEnemy(Enemy _enemy)
    {
        if (!m_activeEnemies.Contains(_enemy))
        {
            return;
        }

        m_activeEnemies.Remove(_enemy);
    }
}