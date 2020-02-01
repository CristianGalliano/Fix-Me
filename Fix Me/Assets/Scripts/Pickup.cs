﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public GameObject Model;
    private GameObject spawned;
    public string Item;
    public bool Spin = true;
    public float SpinRate = 10f;

    // Start is called before the first frame update
    void Start()
    {
        name = Item;
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        SpinUpdate();
    }

    void Spawn()
    {
        foreach(Transform child in GetComponentInChildren<Transform>())
        {
            if (child != transform)
                Destroy(child.gameObject);
        }

        if(Model)
            spawned = Instantiate(Model, transform);
    }

    void SpinUpdate()
    {
        if(Spin)
        {
            Vector3 rot = spawned.transform.localEulerAngles;
            rot.y += SpinRate * Time.deltaTime;
            spawned.transform.localRotation = Quaternion.Euler(45, rot.y, 0);
        }
        else
        {
            spawned.transform.localRotation = Quaternion.Euler(0,0,0);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(Player.M.Pickup(Item))
                    Destroy(gameObject);
                else
                    Debug.Log("No Space");
            }
        }
    }
}