using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckStart : MonoBehaviour
{
    public Rigidbody[] ducks;
    public bool start = false;
    public float force = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartDucks()
    {
        start = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            foreach (var item in ducks)
            {
                item.isKinematic = false;
                item.AddForce(Vector3.back * force, ForceMode.Acceleration);
            }
        }
    }
}
