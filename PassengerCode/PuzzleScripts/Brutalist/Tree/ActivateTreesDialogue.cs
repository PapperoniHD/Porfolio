using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTreesDialogue : MonoBehaviour, IDialogueEndEvent
{
    TreeChopManager treeManager;

    public void Event()
    {
        treeManager.ActivateTrees();
    }

    // Start is called before the first frame update
    void Start()
    {
        treeManager = FindObjectOfType<TreeChopManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
