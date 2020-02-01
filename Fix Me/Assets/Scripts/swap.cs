using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swap : MonoBehaviour
{
    public GameObject a, b;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            a.SetActive(!a.activeSelf);
            b.SetActive(!b.activeSelf);
        }
    }
}
