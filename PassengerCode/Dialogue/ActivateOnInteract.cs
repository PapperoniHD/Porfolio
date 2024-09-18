using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnInteract : MonoBehaviour, IGeneral
{
    public GameObject[] activate;

    bool doOnce = true;
    public void Event()
    {
        if (doOnce)
        {
            foreach (var item in activate)
            {
                item.SetActive(true);
            }
            doOnce = false;
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
