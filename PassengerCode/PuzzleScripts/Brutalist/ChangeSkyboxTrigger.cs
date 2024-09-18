using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkyboxTrigger : MonoBehaviour
{
    public Material newSkybox;

    public GameObject[] objectsToHide;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeSkybox();
        }
    }

    private void ChangeSkybox()
    {
        RenderSettings.skybox = newSkybox;

        foreach (var item in objectsToHide)
        {
            if (item != null)
            {
                item.SetActive(false);
            }
        }
    }
}
