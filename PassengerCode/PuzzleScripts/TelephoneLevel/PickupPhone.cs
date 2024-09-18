using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPhone : MonoBehaviour, IGeneral
{
    public GameObject telephoneSound;
    [SerializeField] private FMODUnity.EventReference _pickup;
    private FMOD.Studio.EventInstance pickup;
    public CameraMovement camMove;

    public GameObject floor;

    public void Event()
    {
        PlaySound();
        telephoneSound.SetActive(false);
        floor.SetActive(false);
        camMove.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!_pickup.IsNull)
        {
            pickup = FMODUnity.RuntimeManager.CreateInstance(_pickup);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlaySound()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(pickup, transform);
        pickup.start();
    }
}
