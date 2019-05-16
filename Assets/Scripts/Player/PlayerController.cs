using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float m_WalkSpeed = 5.5f;
    public float m_SprintSpeed = 11.0f;
    public float m_JumpForce = 12.0f;

    private PlayerMotor m_motor;

    private void Awake()
    {
        m_motor = GetComponent<PlayerMotor>();

        m_motor.SetSpeed(m_WalkSpeed);
    }

    private void Update()
    {
        if (GlobalInput.GetKeyDown("Jump"))
        {
            m_motor.Jump(m_JumpForce);
        }

        if (GlobalInput.GetKeyDown("Sprint"))
        {
            m_motor.SetSpeed(m_SprintSpeed);
        }
        else if (GlobalInput.GetKeyUp("Sprint"))
        {
            m_motor.SetSpeed(m_WalkSpeed);
        }

        if (GlobalInput.GetKeyDown("Hook"))
        {
            m_motor.Hook();
        }
        else if (GlobalInput.GetKeyUp("Hook"))
        {
            m_motor.StopHook();
        }

        m_motor.Move(GlobalInput.GetDPad("Movement"));
    }
}