using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBob : MonoBehaviour
{
    public float frequency = 1.0f;    //movement speed
    public float amplitude = 1.0f;    //movement amount
    public float speed = 100.0f;
    Vector3 startPos;
    float elapsedTime = 0.0f;

    // Use this for initialization
    void Start()
    {
        startPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime * Time.timeScale * frequency;
        transform.position = startPos + Vector3.up * Mathf.Sin(elapsedTime) * amplitude;
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}
