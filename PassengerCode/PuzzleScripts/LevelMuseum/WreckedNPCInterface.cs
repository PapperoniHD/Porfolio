using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckedNPCInterface : MonoBehaviour, IGeneral
{
    public WreckedMuseumPuzzle museumPuzzle;
    private bool doOnce = true;
    public void Event()
    {
        if (doOnce)
        {
            print("did something");
            museumPuzzle.index++;
            if (museumPuzzle.index > 1)
            {
                museumPuzzle.PuzzleDone();
            }
            doOnce = false;
        }
        
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
