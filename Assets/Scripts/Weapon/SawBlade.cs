using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawBlade : MonoBehaviour
{
    public int m_Damage = 130;

    private Rigidbody m_rigidBody;

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.GetComponentInParent<EnemyData>().Damage(m_Damage);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            return;
        }
        else
        {
            m_rigidBody.velocity = Vector3.zero;
            m_rigidBody.useGravity = false;
        }
    }
}