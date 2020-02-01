using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player M;
    private void Awake()
    {
        if (M == null)
        {
            M = this;
        }
        else if (M != this)
        {
            Destroy(this);
        }
    }

    [Header("Player Info")]
    public float PowerLevel = 100;
    public bool[] PartsFound;
    public int BatteryCount;

    [Header("State Parameters")]
    public float PowerUsageRate = 0.1f;
    void Start()
    {
        
    }

    void Update()
    {
        PowerUsage();
        Controls();
    }

    void Controls()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            UseBattery();
        }
    }

    #region Inventory
    public void Pickup(string item)
    {
        switch (item)
        {
            case "Battery":
                BatteryCount++;
                break;
            case "Leg":
                PartsFound[0] = true;
                break;
            case "Arm":
                PartsFound[1] = true;
                break;
            case "Eye":
                PartsFound[2] = true;
                break;
            case "Power Core":
                PartsFound[3] = true;
                break;
        }
    }
    #endregion

    #region Power Level
    public void UseBattery()
    {
        if(BatteryCount > 1)
        {
            if (PowerLevel <= 99)
            {
                BatteryCount--;
                PowerLevel = 100;
            }
            else
            {
                Debug.Log("Dont need to recharge");
            }
        }
        else
        {
            Debug.Log("No Batteries");
        }


    }

    void PowerUsage()
    {
        PowerLevel -= PowerUsageRate * Time.deltaTime;

        if(PowerLevel < 0)
        {
            Debug.Log("Power Out");
        }
    }
    #endregion
}
