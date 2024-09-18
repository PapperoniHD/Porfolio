using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TriggerForDialogueWhileWalk : MonoBehaviour
{
    private DialogueScript script;
    public bool firstDialogue = false;
    public bool lastDialogue = false;
    public TextMeshProUGUI dialogueText;
    public Animator dialogueAnim;
    public GameObject dialogueWindow;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (firstDialogue)
            {
                dialogueWindow.SetActive(true);
                dialogueAnim.Play("PopUp");
                Invoke(nameof(SetLines), 1);
            }
            else if(lastDialogue)
            {
                dialogueText.text = string.Empty;
                dialogueAnim.Play("PopDown");
            }
            else
            {
                SetLines();
            }
            
                      
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void SetLines()
    {
        dialogueText.SetText(GetComponent<DialogueText>().firstLines[0]);
    }
}
