using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawShooterScript : MonoBehaviour
{
    public GameObject m_sawblade1;
    public Transform m_bladePoint1;
    public GameObject m_sawblade2;
    public Transform m_bladePoint2;
    public GameObject m_sawblade3;
    public Transform m_bladePoint3;
    public float m_retrieveSpeed = 15f;
    public float m_maxRetrieveTimer = 1.6f;
    public float m_bladeSpeed = 1400f;

    private float m_retrieveTimer1;
    private float m_retrieveTimer2;
    private float m_retrieveTimer3;
    private bool blade1Loaded = true;
    private bool blade2Loaded = true;
    private bool blade3Loaded = true;
    private bool retrieving1 = false;
    private bool retrieving2 = false;
    private bool retrieving3 = false;

    private Camera m_Camera;

    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GetComponentInParent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessShoot();

        //Timer counts down
        m_retrieveTimer1 -= Time.deltaTime;
        m_retrieveTimer2 -= Time.deltaTime;
        m_retrieveTimer3 -= Time.deltaTime;

        CheckRetrieveTimers();

        CheckBladesLoaded();
    }

    void ProcessShoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //if (!retrieving)
            //{
            Debug.Log("Mouse clicked");

            if (blade1Loaded)
            {
                m_sawblade1.GetComponent<Rigidbody>().isKinematic = false;
                m_sawblade1.transform.parent = null;
                m_sawblade1.GetComponent<Rigidbody>().AddForce(m_Camera.transform.forward * m_bladeSpeed);
                m_retrieveTimer1 = m_maxRetrieveTimer;
                //go.GetComponent<BulletController>().m_player = this;
            }
            else if (blade2Loaded)
            {
                m_sawblade2.GetComponent<Rigidbody>().isKinematic = false;
                m_sawblade2.transform.parent = null;
                m_sawblade2.GetComponent<Rigidbody>().AddForce(m_Camera.transform.forward * m_bladeSpeed);
                m_retrieveTimer2 = m_maxRetrieveTimer;
            }
            else if (blade3Loaded)
            {
                m_sawblade3.GetComponent<Rigidbody>().isKinematic = false;
                m_sawblade3.transform.parent = null;
                m_sawblade3.GetComponent<Rigidbody>().AddForce(m_Camera.transform.forward * m_bladeSpeed);
                m_retrieveTimer3 = m_maxRetrieveTimer;
            }
        }
    }

    void RetrieveBlade1()
    {
        if (!blade1Loaded)
        {
            if (!m_sawblade1.GetComponent<Rigidbody>().isKinematic)
            {
                m_sawblade1.GetComponent<Rigidbody>().isKinematic = true;
            }
            m_sawblade1.transform.position = Vector3.MoveTowards(m_sawblade1.transform.position, m_bladePoint1.position, m_retrieveSpeed * Time.deltaTime);
            m_sawblade1.transform.LookAt(m_bladePoint1);
            //m_sawblade.transform.localEulerAngles = new Vector3(m_sawblade.transform.rotation.x, 0, 90);
        }
        else
        {
            m_sawblade1.transform.position = m_bladePoint1.transform.position;
            //m_sawblade.GetComponent<Rigidbody>().isKinematic = true;
            m_sawblade1.transform.SetParent(m_bladePoint1);
            m_sawblade1.transform.localEulerAngles = new Vector3(0, 0, 0);
            retrieving1 = false;
        }
    }

    void RetrieveBlade2()
    {
        if (!blade2Loaded)
        {
            if (!m_sawblade2.GetComponent<Rigidbody>().isKinematic)
            {
                m_sawblade2.GetComponent<Rigidbody>().isKinematic = true;
            }
            m_sawblade2.transform.position = Vector3.MoveTowards(m_sawblade2.transform.position, m_bladePoint2.position, m_retrieveSpeed * Time.deltaTime);
            m_sawblade2.transform.LookAt(m_bladePoint2);
            //m_sawblade.transform.localEulerAngles = new Vector3(m_sawblade.transform.rotation.x, 0, 90);
        }
        else
        {
            m_sawblade2.transform.position = m_bladePoint2.transform.position;
            //m_sawblade.GetComponent<Rigidbody>().isKinematic = true;
            m_sawblade2.transform.SetParent(m_bladePoint2);
            m_sawblade2.transform.localEulerAngles = new Vector3(0, 0, 0);
            retrieving2 = false;
        }
    }

    void RetrieveBlade3()
    {
        if (!blade3Loaded)
        {
            if (!m_sawblade3.GetComponent<Rigidbody>().isKinematic)
            {
                m_sawblade3.GetComponent<Rigidbody>().isKinematic = true;
            }
            m_sawblade3.transform.position = Vector3.MoveTowards(m_sawblade3.transform.position, m_bladePoint3.position, m_retrieveSpeed * Time.deltaTime);
            m_sawblade3.transform.LookAt(m_bladePoint1);
            //m_sawblade.transform.localEulerAngles = new Vector3(m_sawblade.transform.rotation.x, 0, 90);
        }
        else
        {
            m_sawblade3.transform.position = m_bladePoint3.transform.position;
            //m_sawblade.GetComponent<Rigidbody>().isKinematic = true;
            m_sawblade3.transform.SetParent(m_bladePoint3);
            m_sawblade3.transform.localEulerAngles = new Vector3(0, 0, 0);
            retrieving3 = false;
        }
    }

    void CheckBladesLoaded()
    {
        //Checks if Blade is loaded in the gun or somewhere else every frame
        if (m_sawblade1.transform.position == m_bladePoint1.transform.position)
        {
            blade1Loaded = true;
            //Rotates Blade slowly when idle
            m_sawblade1.transform.Rotate(1, 0, 0);
        }
        else
        {
            blade1Loaded = false;
        }

        if (m_sawblade2.transform.position == m_bladePoint2.transform.position)
        {
            blade2Loaded = true;
            //Rotates Blade slowly when idle
            m_sawblade2.transform.Rotate(1, 0, 0);
        }
        else
        {
            blade2Loaded = false;
        }

        if (m_sawblade3.transform.position == m_bladePoint3.transform.position)
        {
            blade3Loaded = true;
            //Rotates Blade slowly when idle
            m_sawblade3.transform.Rotate(1, 0, 0);
        }
        else
        {
            blade3Loaded = false;
        }
    }

    void CheckRetrieveTimers()
    {
        //If timer reaches zero retrieving starts
        if (m_retrieveTimer1 > 0 && m_retrieveTimer1 < 0.1)
        {
            retrieving1 = true;
        }

        if (m_retrieveTimer2 > 0 && m_retrieveTimer2 < 0.1)
        {
            retrieving2 = true;
        }

        if (m_retrieveTimer3 > 0 && m_retrieveTimer3 < 0.1)
        {
            retrieving3 = true;
        }

        if (retrieving1)
        {
            RetrieveBlade1();
        }

        if (retrieving2)
        {
            RetrieveBlade2();
        }

        if (retrieving3)
        {
            RetrieveBlade3();
        }
    }
}