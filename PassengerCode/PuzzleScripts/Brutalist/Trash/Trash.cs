using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour, IGeneral
{
    private GameObject trash;

    public bool cleaned = false;

    [SerializeField] private FMODUnity.EventReference _cleanSound;
    private FMOD.Studio.EventInstance cleanSound;

    public void Event()
    {
        
        Clean();
        PlaySound(cleanSound);
        
    }

    void Clean()
    {
        trash.SetActive(false);
        var mesh = GetComponent<MeshCollider>();
        var boxCol = GetComponent<BoxCollider>();
        if (mesh != null)
        {
            mesh.enabled = false;
        }
        if (boxCol != null)
        {
            boxCol.enabled = false;
        }
        cleaned = true;
        FindObjectOfType<TrashManager>().CheckIfAllCleaned();
    }

    public void PlaySound(FMOD.Studio.EventInstance sound)
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(sound, transform);
        sound.start();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!_cleanSound.IsNull)
        {
            cleanSound = FMODUnity.RuntimeManager.CreateInstance(_cleanSound);
        }

        trash = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
