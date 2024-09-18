using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactBubbleSpawn : MonoBehaviour
{
    public GameObject impactBubble;


    IEnumerator BubbleCoroutine()
    {
        GameObject bubbles = Instantiate(impactBubble, gameObject.transform);
        yield return new WaitForSeconds(5);
        Destroy(bubbles);
    }

    private void OnEnable()
    {
        StartCoroutine(BubbleCoroutine());
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
