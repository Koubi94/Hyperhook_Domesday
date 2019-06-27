using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CursorLockMode m_lockState;
    public float m_MouseSensitivity = 45;
    [Range(0, 90)] public float m_NegativeCamClamp = 75;
    [Range(0, 90)] public float m_PositiveCamClamp = 90;

    private Camera m_camera;

    private void Awake()
    {
        m_camera = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        Cursor.lockState = m_lockState;
        Cursor.visible = false;
    }

    private void Update()
    {
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        // calculate vertical rotation
        Vector3 rot = m_camera.transform.localEulerAngles;
        float value = -GlobalInput.GetMouseVector().y * Time.deltaTime * m_MouseSensitivity;
        rot.x = (rot.x + 180) % 360;
        rot.x = Mathf.Clamp(rot.x + value, 180 - m_PositiveCamClamp, 180 + m_NegativeCamClamp);
        rot.x = (rot.x - 180) % 360;
        m_camera.transform.localEulerAngles = rot;

        // calculate horizontal rotation
        rot = transform.localEulerAngles;
        value = GlobalInput.GetMouseVector().x * Time.deltaTime * m_MouseSensitivity;
        rot.y += value;
        transform.localEulerAngles = rot;
    }

    public Vector3 GetMoveDirection(Vector3 _dir)
    {
        return GetMoveDirection(new Vector2(_dir.x, _dir.z));
    }

    public Vector3 GetMoveDirection(Vector2 _dir)
    {
        Vector3 dir = m_camera.transform.TransformDirection(_dir.x, 0, _dir.y);
        dir.y = 0;
        return dir.normalized;
    }
}