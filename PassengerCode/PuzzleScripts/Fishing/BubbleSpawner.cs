using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{

    public GameObject bubbles;

    public float maxTime = 10f;
    public float minTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        SpawnBubbles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void SpawnBubbles()
    {
        GameObject bubblesGO = Instantiate(bubbles, gameObject.transform);

        Invoke(nameof(SpawnBubbles), Random.Range(minTime, maxTime));
    }
}
