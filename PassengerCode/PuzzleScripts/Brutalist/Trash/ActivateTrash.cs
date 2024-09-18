using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTrash : MonoBehaviour, IDialogueEndEvent
{
    private TrashManager tManager;

    public GameObject arrow;

    public void Event()
    {
        tManager.ActivateTrash();
        arrow.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        tManager = FindObjectOfType<TrashManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
