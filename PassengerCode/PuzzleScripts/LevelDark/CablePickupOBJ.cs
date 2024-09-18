using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CablePickupOBJ : MonoBehaviour
{
    public LightSwitchPuzzle puzzleScript;
    Interactable interactScript;
    public bool attached = false;
    public bool holding = false;
    Rigidbody rb;

    public MeshRenderer[] placeLocations;
    // Start is called before the first frame update
    void Start()
    {
        interactScript = FindObjectOfType<Interactable>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attached)
        {
            rb.isKinematic = true;
        }
        else
        {
            puzzleScript.CableBools[0] = false;
            puzzleScript.CableBools[1] = false;
            puzzleScript.CableBools[2] = false;
        }
        ShowPlaceLocations();
        
    }

    void ShowPlaceLocations()
    {
        if (holding)
        {
            foreach (var item in placeLocations)
            {
                item.enabled = true;
            }
        }
        else
        {
            foreach (var item in placeLocations)
            {
                item.enabled = false;
            }
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cable"))
        {
            int index = other.GetComponent<CablePickupLocationIndex>().index;
            puzzleScript.CableBools[index] = true;
            holding = false;
            interactScript.ResestGrab(rb);
            attached = true;
            transform.SetPositionAndRotation(other.transform.position, other.transform.rotation);
        }
    }
}
