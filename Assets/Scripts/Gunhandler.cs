using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gunhandler : MonoBehaviour
{
    public GameObject m_BulletPrefab;
    public Transform m_BulletPoint;

    public Text m_ammoText;
    public int m_currAmmo;

    //private Anima

    private Camera m_Camera;

    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GetComponentInParent<Camera>();
        m_currAmmo = 30;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessShoot();

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        RefreshAmmoText();
    }

    void ProcessShoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (m_currAmmo > 0)
            {
                GameObject go = Instantiate(m_BulletPrefab, m_BulletPoint.position, m_BulletPoint.rotation);
                go.GetComponent<Rigidbody>().AddForce(m_Camera.transform.forward * 3000);
                //go.GetComponent<BulletController>().m_player = this;
                // Zerstört abgefeuerte Kugeln in 5 Sekunden
                Destroy(go, 5f);
                m_currAmmo--;
                //m_ammoText.text = m_currAmmo + " / 30 ";
                //RefreshAmmoText();
                Debug.Log("hehexD");
            }
            else
            {
                Debug.Log("No Ammo!");
            }
        }
    }

    void Reload()
    {
        m_currAmmo = 30;
        //m_ammoText.text = m_currAmmo + " / 30 ";
        //RefreshAmmoText();
    }

    public void RefreshAmmoText()
    {
        m_ammoText.text = m_currAmmo + " / 30 ";
    }
}
