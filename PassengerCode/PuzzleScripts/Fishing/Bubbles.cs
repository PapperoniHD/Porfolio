using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Bubbles : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float size;

    private RectTransform pos;
    private float yPos;
    private Image sprite;

    public bool randomSize = true;

    // Start is called before the first frame update
    void Start()
    {
        pos = GetComponent<RectTransform>();
        speed = Random.Range(200, 400);
        size = Random.Range(0.3f, 1f);
        if (randomSize)
        {
            pos.localScale = new Vector3(size, size, 0);
        }
        
        sprite = GetComponent<Image>();
        sprite.color = new Color(255, 255, 255, 255 * size);
        float randomRot = Random.Range(0, 360);
        pos.rotation = Quaternion.Euler(0f, 0f, randomRot);
        yPos = pos.anchoredPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        yPos += Time.deltaTime * speed;
        float time = 0;
        time += Time.deltaTime;

        pos.anchoredPosition = new Vector3(pos.anchoredPosition.x, yPos, 0);

        if (time > 20)
        {
            Destroy(gameObject);
        }
    }
}
