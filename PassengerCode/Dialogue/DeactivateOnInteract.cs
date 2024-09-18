using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOnInteract : MonoBehaviour, IGeneral
{
    public GameObject[] deActivate;

    bool doOnce = true;
    public void Event()
    {
        if (doOnce)
        {
            foreach (var item in deActivate)
            {
                item.SetActive(false);
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
