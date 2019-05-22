using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int m_Damage = 15;
    public float m_LifeTime = 3.0f;

    private void Update()
    {
        m_LifeTime -= Time.deltaTime;

        if (m_LifeTime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponentInParent<PlayerData>().Damage(m_Damage);
        }
    }
}
