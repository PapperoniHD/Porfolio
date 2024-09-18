using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallEnableCollider : MonoBehaviour
{
    private AnimController anim;

    public bool enable;

    // Start is called before the first frame update
    void Start()
    {
        anim = FindObjectOfType<AnimController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (enable)
            {
                anim.fallEnabled = true;
            }
            else
            {
                anim.fallEnabled = false;
            }
        }
    }
}
