using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGoOff : MonoBehaviour
{
    public GameObject[] deactivate;
    public GameObject[] activate;

    public GameObject colliderGO;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveCollider()
    {
        colliderGO.SetActive(false);
    }

    public void RemoveNPC()
    {
        foreach (var item in deactivate)
        {
            item.SetActive(false);
        }
    }

    public void StartNPC()
    {
        foreach (var item in deactivate)
        {
            item.SetActive(false);
        }
        foreach (var item in activate)
        {
            item.SetActive(true);
        }
    }
}
