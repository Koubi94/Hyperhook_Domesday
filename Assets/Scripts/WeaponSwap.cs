using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    public GameObject m_gun1;
    public GameObject m_gun2;

    //private Gunhandler m_gunhandler;

    //m_currGun is the gun the Player has equipped currently
    //private int m_currGun;

    // Start is called before the first frame update
    void Start()
    {
        //m_currGun = 1;
    }

    // Update is called once per frame
    void Update()
    {
        SwapGuns();
        //CheckCurrGun();
    }

    void SwapGuns()
    {
        //Check for Input and change m_currGun when necessary
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            //m_currGun = 1;
            m_gun1.SetActive(true);
            m_gun2.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //m_currGun = 2;
            m_gun2.SetActive(true);
            m_gun1.SetActive(false);
        }
    }

    //void CheckCurrGun()
    //{
    //    //check if current gun equals m_currGun and swap gun is necessary
    //
    //}
}
