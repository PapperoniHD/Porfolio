using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAxe : MonoBehaviour, IDialogueEndEvent2
{
    public GameObject axe;
    public GameObject spiritAxe;
    public bool activate;

    public GameObject arrow;
    public GameObject arrowYellow;

    public void Event()
    {
        print("work?");
        if (activate)
        {
            axe.SetActive(true);
            spiritAxe.SetActive(false);
            arrow.SetActive(false);
        }
        else
        {
            axe.SetActive(false);
            spiritAxe.SetActive(true);
            arrowYellow.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
