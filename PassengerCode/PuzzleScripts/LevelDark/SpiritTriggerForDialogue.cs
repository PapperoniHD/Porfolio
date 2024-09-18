using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritTriggerForDialogue : MonoBehaviour
{
    private DialogueScript script;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            script = FindObjectOfType<DialogueScript>();
            script.dialogueObj = gameObject;
            script.SetLines(GetComponent<DialogueText>().firstLines);
            if (GetComponent<DialogueText>().lookpos1 != null)
            {
                script.SetLookAt(GetComponent<DialogueText>().lookpos1.transform);
            }
            else
            {
                script.SetLookAt(this.transform);
            }
            
            StartCoroutine(script.StartDialogue());
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
