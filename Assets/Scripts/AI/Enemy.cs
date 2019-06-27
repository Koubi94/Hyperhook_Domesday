using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float m_FirePower = 35.0f;
    public float m_Cooldown = 0.35f;
    public GameObject m_BulletPrefab;
    public Transform m_BulletPoint;
    public float m_FireRange = 75.0f;

    private float m_currentCooldown;

    private bool m_instantiated;
    private Transform m_target;
    private PlayerData m_player;
    private NavMeshAgent m_agent;

    private void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        EnemyManager.AddEnemy(this);
    }

    private void Update()
    {
        if (!m_instantiated)
        {
            return;
        }

        if (IsPlayerInRange() && IsPlayerInSight())
        {
            if (!m_agent.isStopped)
            {
                m_agent.isStopped = true;
            }

            if (m_currentCooldown == 0)
            {
                transform.LookAt(m_player.transform, Vector3.up);
                Shoot();
            }
        }
        else
        {
            if (m_agent.isStopped)
            {
                m_agent.isStopped = false;
            }

            m_agent.SetDestination(m_player.transform.position);
        }

        if (m_currentCooldown > 0)
        {
            m_currentCooldown = Mathf.Clamp(m_currentCooldown - Time.deltaTime, 0, float.MaxValue);
        }
    }

    public void Instantiate(PlayerData _player)
    {
        m_instantiated = true;
        m_player = _player;
    }

    private bool IsPlayerInSight()
    {
        Ray ray = new Ray(m_BulletPoint.position, (m_player.GetPosition() - m_BulletPoint.position).normalized);
        RaycastHit hit;

        Physics.Raycast(ray, out hit);

        return hit.transform.gameObject.layer == LayerMask.NameToLayer("Player");
    }

    private bool IsPlayerInRange()
    {
        if ((m_player.GetPosition() - m_BulletPoint.position).sqrMagnitude < m_FireRange * m_FireRange)
        {
            return true;
        }

        return false;
    }

    private void Shoot()
    {
        GameObject go = Instantiate(m_BulletPrefab, m_BulletPoint.position, Quaternion.identity);
        go.GetComponent<Rigidbody>().AddForce((m_player.GetPosition() - m_BulletPoint.position).normalized * m_FirePower, ForceMode.VelocityChange);
        m_currentCooldown = m_Cooldown;
    }
}