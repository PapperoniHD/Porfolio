using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeChop : MonoBehaviour, IGeneral
{

    private MeshRenderer skin;
    public GameObject stub;

    private int amountOfChop = 0;
    public int totalChops = 2;
    public bool chopped = false;

    [SerializeField] private FMODUnity.EventReference _chopSound;
    private FMOD.Studio.EventInstance chopSound;

    public void Event()
    {
        if (amountOfChop >= totalChops)
        {
            Chop();
            PlaySound(chopSound);
        }
        else
        {
            amountOfChop++;
            PlaySound(chopSound);
        }
    }

    void Chop()
    {
        skin.enabled = false;
        stub.SetActive(true);
        GetComponent<BoxCollider>().enabled = false;
        chopped = true;
        FindObjectOfType<TreeChopManager>().CheckIfAllChopped();
    }

    public void PlaySound(FMOD.Studio.EventInstance sound)
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(sound, transform);
        sound.start();
    }

    // Start is called before the first frame update
    void Start()
    {
        skin = GetComponent<MeshRenderer>();
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
