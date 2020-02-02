using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game M;
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

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Caught()
    {
        Player.M.Movement.enabled = false;
    }

    public string Objective()
    {
        if(!Player.M.PartsFound[3])
        {
            string temp = "";

            if (!Player.M.PartsFound[0])
                temp += "Find Eye \n";

            if (!Player.M.PartsFound[0])
                temp += "Find Arm \n";

            if (!Player.M.PartsFound[0])
                temp += "Find Leg \n";

            return temp;
        }
        else
        {
            return "RUN!!!";
        }
    }
}
