using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankChop : MonoBehaviour, IGeneral
{
    public Rigidbody[] rbs;

    [SerializeField] private FMODUnity.EventReference _chopSound;
    private FMOD.Studio.EventInstance chopSound;

    public GameObject[] activate;
    public GameObject[] deactivate;

    public bool activateAndDeactivate = false;
    public void Event()
    {
        foreach (var item in rbs)
        {
            item.isKinematic = false;
            item.AddExplosionForce(20, item.transform.position, 5, 3.0f);
        }
        PlaySound(chopSound);
        GetComponent<BoxCollider>().enabled = false;

        if (activateAndDeactivate)
        {
            foreach (var item in activate)
            {
                item.SetActive(true);
            }
            foreach (var item in deactivate)
            {
                item.SetActive(false);
            }
        }
    }

    public void PlaySound(FMOD.Studio.EventInstance sound)
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(sound, transform);
        sound.start();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!_chopSound.IsNull)
        {
            chopSound = FMODUnity.RuntimeManager.CreateInstance(_chopSound);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
