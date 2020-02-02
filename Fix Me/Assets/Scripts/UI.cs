using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public static UI M;
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

    public Image ChargeBar;
    public TextMeshProUGUI BatteryCount;
    public TextMeshProUGUI ObjectivesText;
    public Image[] BodyParts;

    public GameObject FadePanel;
    public TextMeshProUGUI FadeText;

    public TextMeshProUGUI InteractText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Parts();
        Objectives();
        Battery();
    }

    void Objectives()
    {
        ObjectivesText.text = Game.M.Objective();
    }

    void Parts()
    {
        for(int i = 0; i < 4; i++)
        {
            if (Player.M.PartsFound[i])
                BodyParts[i].color = new Color(0f,0f,150f/255f);
            else
                BodyParts[i].color = new Color(200f/255f, 0f, 0);
        }
    }

    void Battery()
    {
        ChargeBar.rectTransform.sizeDelta = new Vector2(ChargeBar.rectTransform.sizeDelta.x, 200 * (Player.M.PowerLevel / 100));
        BatteryCount.text = "x" + Player.M.BatteryCount;
    }

    public void FadeOut(string reason)
    {
        FadePanel.GetComponent<Animation>().Play("FadeOut");
        FadeText.text = reason;
    }

    public void ToggleInteract(bool toggle)
    {
        InteractText.enabled = toggle;
    }
}
