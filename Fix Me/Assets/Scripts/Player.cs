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
    public bool[] PartsFound = new bool[4];
    public int BatteryCount;
    public int BatteryMax;
    public float PowerUsageRate = 0.1f;

    [Header("Animations")]
    GameObject Root;
    Animation anim;

    public UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController Movement;


    void Start()
    {
        PartsFound = new bool[4];
        Movement = GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>();
        anim = GetComponent<Animation>();
        Root = transform.parent.gameObject;
    }

    void Update()
    {
        PowerUsage();
        Controls();
    }

    void Controls()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            UseBattery();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Clamber();
        }
    }

    #region Abilities

    void Clamber()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit) && hit.collider.tag == "Ledge")
        {
            Root.transform.position = transform.position;
            transform.localPosition = new Vector3(0, 0, 0);
            Root.transform.rotation = transform.rotation;
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            anim.Play("ClimbAnim");
        }
    }
    #endregion

    #region Inventory
    public bool Pickup(string item)
    {
        switch (item)
        {
            case "Battery":
                if (BatteryCount < BatteryMax)
                    BatteryCount++;
                else
                    return false;
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

        return true;
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
