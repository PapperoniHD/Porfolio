using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Give30Min : MonoBehaviour,IDialogueEndEvent
{
    private MinuteManager mm;
    public int giveMinutes = 30;

    public GameObject arrow;


    public void Event()
    {
        if (enabled)
        {
            if (arrow != null)
            {
                arrow.SetActive(false);
            }
            mm.minutes += giveMinutes;
            mm.UpdateText();
            this.enabled = false;
            FindObjectOfType<DeveloperTools>().EnablePlayer();
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        mm = FindObjectOfType<MinuteManager>();
        if (arrow != null)
        {
            arrow.SetActive(true);
            arrow.GetComponent<Animator>().Play("UpDownExclamation2");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
