using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckEndDialogue : MonoBehaviour, IDialogueEndEvent
{

    public Animator trainAnim;
    public GameObject arrow;
    public void Event()
    {
        GetComponent<Animator>().SetBool("down", true);
        StartCoroutine(TrainReady());
    }


    IEnumerator TrainReady()
    {
        yield return new WaitForSeconds(10);
        trainAnim.Play("MuseumOpenDoors");
        arrow.SetActive(true);
    }
    // Start is called before the first frame updates
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
