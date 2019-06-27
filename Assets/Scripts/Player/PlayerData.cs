using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerData : MonoBehaviour
{
    public GameMenuManager m_menuManager;
    public TMP_Text m_HealthText;
    public RectTransform m_HealthBar;

    public float m_EyeHeight;
    public int m_MaxHealth = 200;

    public int Health { get; private set; }

    private void Start()
    {
        Health = m_MaxHealth;
        UpdateHealthUI();
    }

    public Vector3 GetPosition()
    {
        return transform.position + Vector3.up * m_EyeHeight;
    }

    public void Damage(int _damage)
    {
        Health -= _damage;

        if (Health <= 0)
        {
            Health = 0;
            UpdateHealthUI();
            Kill();
        }
        else
        {
            UpdateHealthUI();
        }
    }

    public void Heal(int _heal)
    {
        Health += _heal;

        if (Health > 200)
        {
            Health = m_MaxHealth;
        }

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        Vector3 v = Vector3.one;
        v.x = (float)Health / (float)m_MaxHealth;
        m_HealthBar.localScale = v;

        m_HealthText.text = $"{Health} / {m_MaxHealth}";
    }

    private void Kill()
    {
        m_menuManager.GameOver();

        gameObject.SetActive(false);
    }
}