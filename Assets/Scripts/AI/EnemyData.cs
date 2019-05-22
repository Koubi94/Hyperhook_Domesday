using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public int Health { get; private set; }

    private void Start()
    {
        Health = 100;
    }

    public void Damage(int _damage)
    {
        Health -= _damage;

        if (Health <= 0)
        {
            Kill();
        }
    }

    private void Kill()
    {
        EnemyManager.RemoveEnemy(GetComponent<Enemy>());
        WaveManager.Get.EnemyDead();
        Destroy(gameObject);
    }
}
