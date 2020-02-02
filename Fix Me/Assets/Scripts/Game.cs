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
    }

    bool dead;

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
        Dead("Caught by Robots");
    }

    public IEnumerator Dead(string item)
    {
        UI.M.FadeOut(item);
        dead = true;
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public string Objective()
    {
        if(!Player.M.PartsFound[3])
        {
            string temp = "";

            if (!Player.M.PartsFound[0])
                temp += "- Find Eye \n";

            if (!Player.M.PartsFound[1])
                temp += "- Find Arm \n";

            if (!Player.M.PartsFound[2])
                temp += "- Find Leg \n";

            if (!Player.M.PartsFound[3])
                temp += "- Find Power Core \n";

            return temp;
        }
        else
        {
            return "RUN!!!";
        }
    }
}
