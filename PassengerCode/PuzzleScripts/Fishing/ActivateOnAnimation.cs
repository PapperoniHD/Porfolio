using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnAnimation : MonoBehaviour
{
    public GameObject[] activate;
    public GameObject[] deactivate;


    public void Activate()
    {
        foreach (var item in activate)
        {
            if (item != null)
            {
                item.SetActive(true);
            }
        }
    }
    public void Deactivate()
    {
        foreach (var item in deactivate)
        {
            if (item != null)
            {
                item.SetActive(false);
            }
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
