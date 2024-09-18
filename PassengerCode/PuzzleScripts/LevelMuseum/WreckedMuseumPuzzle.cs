using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckedMuseumPuzzle : MonoBehaviour
{
    Instant_Teleportation tpScript;
    public GameObject npc1;
    public GameObject npc2;
    public int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        tpScript = GetComponent<Instant_Teleportation>();
        npc1.SetActive(false);
        npc2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !tpScript.puzzleDone && index == 0)
        {
            StartCoroutine(ActivateNPC(npc1, npc2));
        }
        if (other.CompareTag("Player") && !tpScript.puzzleDone && index == 1)
        {
            StartCoroutine(ActivateNPC(npc2, npc1));            
        }
    }
    
    public void PuzzleDone()
    {
        tpScript.puzzleDone = true;
    }

    IEnumerator ActivateNPC(GameObject activate, GameObject remove)
    {
        yield return new WaitForSeconds(1);
        remove.SetActive(false);
        activate.SetActive(true);       
    }
}
