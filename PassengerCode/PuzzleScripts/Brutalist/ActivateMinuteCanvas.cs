using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateMinuteCanvas : MonoBehaviour, IDialogueEndEvent
{

    public GameObject minUI;

    public GameObject[] arrows;
    public void Event()
    {
        minUI.SetActive(true);

        foreach (var item in arrows)
        {
            item.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        minUI.SetActive(false);
        foreach (var item in arrows)
        {
            item.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
