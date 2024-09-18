using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using tripolygon.UModeler;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueText : MonoBehaviour
{
    [System.NonSerialized]
    public string[] lines;

    public int voice = 0;

    public string playerName;

    public string[] firstLines;
    public string[] secondLines;
    public string[] optionalLines1;
    public string[] optionalLines2;
    public string[] optionalLines3;

    public bool hasQuestion = false;
    public int indexForQuestion;

    public int[] indexToChangeToName;

    public CameraLookAround cameraLookAround; // Reference to the CameraLookAround script
    public GameObject lookpos1;

    private int currentLineIndex = 0;
    private bool isCameraLookAroundTriggered = false;

    void Start()
    {
        
        playerName = PlayerPrefs.GetString("PlayerName", "NoName");       
        lines = firstLines;
        if (indexToChangeToName != null)
        {
            foreach (var index in indexToChangeToName)
            {
                if (lines[index].Contains("PlayerName"))
                {
                    lines[index] = lines[index].Replace("PlayerName", playerName);
                }
            }
        }
        
        DisplayNextLine(); // Start the dialogue sequence      
    }

    public void SetSecondLines()
    {
        if (secondLines != null)
        {
            lines = secondLines;
        }
    }

    public void DisplayNextLine()
    {
        if (!isCameraLookAroundTriggered && currentLineIndex == 2)
        {
            TriggerCameraLookAround();
            isCameraLookAroundTriggered = true;

        }
        currentLineIndex++;

        if (currentLineIndex >= lines.Length)
        {
            // Dialogue ends
            return;
        }
    }

    void TriggerCameraLookAround()
    {
        DialogueScript dialogueScript = FindObjectOfType<DialogueScript>();
        if (dialogueScript != null)
        {
            dialogueScript.lookTarget = lookpos1.transform;
        }
        if (cameraLookAround != null)
        {
            cameraLookAround.StartCameraRotation(() =>
            {
                // Callback to proceed with remaining dialogue lines after camera has looked around
                SetSecondLines();
                DisplayNextLine();
            });
        }
    }
}