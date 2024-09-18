using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueLook : MonoBehaviour
{

    public Transform[] lookObjects;
    private DialogueText dialogueText;
    private DialogueScript dialogueScript;
    //public int[] indexToChange;
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        dialogueText = GetComponent<DialogueText>();
        dialogueScript = FindObjectOfType<DialogueScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextIndex()
    {
        index++;
        ChangeLook();
    }
    public void ChangeLook()
    {
        if (lookObjects[index] != null)
        {
            dialogueScript.lookTarget = lookObjects[index];
        }     
    }
}
