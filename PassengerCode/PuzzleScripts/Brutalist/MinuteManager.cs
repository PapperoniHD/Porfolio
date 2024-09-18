using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinuteManager : MonoBehaviour
{
    public int minutes = 0;
    public TextMeshProUGUI text;

    public GameObject[] doors;

    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        if (minutes >= 60)
        {
            text.color = Color.green;
            doors[0].SetActive(false);
            doors[1].SetActive(true);
        }
    }

    public void UpdateText()
    {
        text.SetText(minutes.ToString() + " " + "/ 60 min");
    }
}
