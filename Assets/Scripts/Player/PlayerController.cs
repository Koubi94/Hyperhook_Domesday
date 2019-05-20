using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float m_WalkSpeed = 5.5f;
    public float m_SprintSpeed = 11.0f;
    public float m_JumpForce = 12.0f;

    [Header("Crosshair")]
    public Image m_Crosshair;
    public Color m_CrosshairColor = Color.white;

    private PlayerMotor m_motor;
    private Camera m_cam;

    private void Awake()
    {
        m_motor = GetComponent<PlayerMotor>();
        m_cam = GetComponentInChildren<Camera>();

        m_motor.SetSpeed(m_WalkSpeed);
    }

    private void Update()
    {
        InputUpdate();
        HookUpdate();
    }

    private void InputUpdate()
    {
        if (Input.GetButtonDown("Jump"))
        {
            m_motor.Jump(m_JumpForce);
        }

        if (Input.GetButtonDown("Sprint"))
        {
            m_motor.SetSpeed(m_SprintSpeed);
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            m_motor.SetSpeed(m_WalkSpeed);
        }

        if (Input.GetButtonDown("Hook"))
        {
            m_motor.Hook();
        }
        else if (Input.GetButtonUp("Hook"))
        {
            m_motor.StopHook();
        }

        m_motor.Move(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
    }

    private void HookUpdate()
    {
        RaycastHit hit;
        if (m_motor.CanHook(out hit))
        {
            m_Crosshair.color = m_CrosshairColor;
            m_Crosshair.rectTransform.position = m_cam.WorldToScreenPoint(hit.point);
        }
        else
        {
            m_Crosshair.color = Color.clear;
        }
    }
}