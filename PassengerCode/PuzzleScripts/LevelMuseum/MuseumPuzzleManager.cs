using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MuseumPuzzleManager : MonoBehaviour
{
    public static MuseumPuzzleManager PuzzleManager;



    public bool level1 = false;
    public Instant_Teleportation tp1;
    public Instant_Teleportation tp2;

    public MeshRenderer[] placeLocations;

    public MuseumPickupOBJ[] pickupOBJLevel1;

    public SwitchPaintingTexture[] bigRoomPaintings;
    public Animator bigDoors;

    public GameObject wrongDialogue;
    public GameObject rightDialogue;

    // Start is called before the first frame update
    void Start()
    {
        PuzzleManager = this;
        DisablePlaceLocation();
    }

    // Update is called once per frame
    void Update()
    {
        if (level1)
        {
            if (tp1 != null)
            {
                tp1.puzzleDone = true;
            }
            wrongDialogue.SetActive(false);
            rightDialogue.SetActive(true);
        }
        else
        {
            if (tp1 != null)
            {
                tp1.puzzleDone = false;
            }
            wrongDialogue.SetActive(true);
            rightDialogue.SetActive(false);
        }
        if (pickupOBJLevel1.All(obj => obj.rightPlace))
        {
            level1 = true;
        }
        else
        {
            level1 = false;
        }

    }

    public void EnablePlaceLocations()
    {
        foreach (var item in placeLocations)
        {
            CablePickupLocationIndex cableScript = item.GetComponentInChildren<CablePickupLocationIndex>();
            if (cableScript != null)
            {
                if (!cableScript.hasPainting)
                {
                    item.enabled = true;
                }
            }
            else
            {
                item.enabled = true;
            }
            
            
        }
    }

    public void DisablePlaceLocation()
    {
        foreach (var item in placeLocations)
        {
            item.enabled = false;
        }
    }

    public void ActivatePainting()
    {
        foreach (var item in bigRoomPaintings)
        {
            item.SwitchToOtherMaterial();
        }
        StartCoroutine(LightAndDoorOpen());
    }

    IEnumerator LightAndDoorOpen()
    {

        yield return new WaitForSeconds(3);
        bigDoors.Play("BigDoorsMuseum");
    }

}
