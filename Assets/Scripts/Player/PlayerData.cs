using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public float m_EyeHeight;

    private void Awake()
    {
        
    }

    public Vector3 GetPosition()
    {
        return transform.position + Vector3.up * m_EyeHeight;
    }
}