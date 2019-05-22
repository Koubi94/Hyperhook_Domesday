using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawBlade : MonoBehaviour
{
    private Rigidbody m_rigidBody;

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        m_rigidBody.velocity = Vector3.zero;
        m_rigidBody.useGravity = false;

        //transform.position = other.ClosestPointOnBounds(transform.position);
        Debug.Log(other.name);
    }
}