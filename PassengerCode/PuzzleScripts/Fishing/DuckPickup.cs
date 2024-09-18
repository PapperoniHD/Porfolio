using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckPickup : MonoBehaviour
{
    public GameObject duck;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Caught()
    {
        if (duck != null)
        {
            Destroy(duck);
            return true;          
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        if (col.CompareTag("Pickup"))
        {
            duck = col.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        duck = null;
    }
}
