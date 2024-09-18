using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPaintingRoomTrigger : MonoBehaviour
{
    private MuseumPuzzleManager puzzleManager;

    // Start is called before the first frame update
    void Start()
    {
        puzzleManager = FindObjectOfType<MuseumPuzzleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            puzzleManager.ActivatePainting();
            Destroy(gameObject);
        }
    }
}
