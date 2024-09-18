using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseumPickupOBJ : MonoBehaviour
{
    private MuseumPuzzleManager puzzleScript;
    private EndPaintingRoomTrigger endPuzzle;
    Interactable interactScript;
    public bool attached = false;
    public bool holding = false;
    Rigidbody rb;
    public bool rightPlace = false;

    public int paintingIndex;

    public CablePickupLocationIndex cableScript;

    public GameObject dialogueStrange;
    public GameObject dialogueRight;

    void Start()
    {
        interactScript = FindObjectOfType<Interactable>();
        rb = GetComponent<Rigidbody>();
        puzzleScript = FindObjectOfType<MuseumPuzzleManager>();
        endPuzzle = FindObjectOfType<EndPaintingRoomTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!attached)
        {
            rightPlace = false;
        }
    }

    public void ShowPlaceLocations()
    {
        puzzleScript.EnablePlaceLocations();
        if (paintingIndex == 1)
        {
            dialogueStrange.SetActive(true);
            dialogueRight.SetActive(false);
        }
    }
    public void HidePlaceLocations()
    {
        puzzleScript.DisablePlaceLocation();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cable") && paintingIndex < 4)
        {
            other.GetComponent<CablePickupLocationIndex>().hasPainting = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cable") && paintingIndex < 4)
        {
            cableScript = other.GetComponentInChildren<CablePickupLocationIndex>();
            if (cableScript != null)
            {
                if (!cableScript.hasPainting)
                {
                    cableScript.GetComponent<BoxCollider>().enabled = false;
                    //StartCoroutine(interactScript.PauseInteraction());
                    holding = false;                   
                    attached = true;
                    GetComponent<Rigidbody>().isKinematic = true;
                    transform.SetPositionAndRotation(other.transform.position, other.transform.rotation);
                    int index = other.GetComponent<CablePickupLocationIndex>().index;
                    if (index == paintingIndex)
                    {
                        rightPlace = true;
                        if (paintingIndex == 1)
                        {
                            dialogueStrange.SetActive(false);
                            dialogueRight.SetActive(true);
                        }
                    }
                    HidePlaceLocations();
                    other.GetComponent<CablePickupLocationIndex>().hasPainting = true;
                    interactScript.ResestGrab(rb);
                }
                              
            }
            
        }

        //For last in level
        if (other.CompareTag("Cable") && paintingIndex >= 4)
        {
            holding = false;   
            attached = true;
            transform.SetPositionAndRotation(other.transform.position, other.transform.rotation);
            int index = other.GetComponent<CablePickupLocationIndex>().index;
            if (index == paintingIndex)
            {
                rightPlace = true;
            }
            endPuzzle.ReturnToNormal();
            HidePlaceLocations();
            interactScript.ResestGrab(rb);
            gameObject.SetActive(false);
            
        }
    }
}
