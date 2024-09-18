using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseToStay : MonoBehaviour, INoEvent
{
    public LastSpiritChoice choice;

    public void Event()
    {
        choice.mesh.enabled = false;
        choice.goBack = false;
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
