using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleshipNO : MonoBehaviour, INoEvent
{
    private DialogueText text;
    public int newIndexForQuestion = 2;
 
    public void Event()
    {
        text.indexForQuestion = newIndexForQuestion;
    }

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<DialogueText>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
