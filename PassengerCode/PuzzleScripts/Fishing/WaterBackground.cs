using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBackground : MonoBehaviour
{

    public RectTransform[] backroundImages;

    public Color lightWater;
    public Color darkWater;

    public Camera cam;

    public float timeMultiplier;
    public float t = 0;

    public float depth = 0;
    public float maxDepth = 2000;

    public float xAreaMax = 300;
    public float xArea = 0;
    public float xAreaTimeMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        depth = t * maxDepth;

        FishGameManager.fGM.depth = (int)(depth * 0.01f);

        if (FishGameManager.fGM.inputEnabled)
        {
            if (Input.GetKey(KeyCode.S))
            {
                if (t < 1)
                {
                    t += Time.deltaTime * timeMultiplier;
                }
            }
            if (Input.GetKey(KeyCode.W))
            {
                if (t > 0)
                {
                    t -= Time.deltaTime * timeMultiplier;
                }
            }
        }
        

        Color col = Color.Lerp(lightWater, darkWater, t);
        cam.backgroundColor = col;

        foreach (var item in backroundImages)
        {
            item.anchoredPosition = new Vector3(item.anchoredPosition.x, depth, 0);
        }

    }

    public void ResetGameField()
    {
        xArea = 0;
        foreach (var item in backroundImages)
        {
            item.anchoredPosition = new Vector3(xArea, item.anchoredPosition.y, 0);
        }
    }

    public void XMove(bool Add)
    {
        if (Add)
        {          
            if (xArea < xAreaMax)
            {
                xArea += Time.deltaTime * xAreaTimeMultiplier;
            }
        }
        else
        {           
            if (xArea > -xAreaMax)
            {
                xArea -= Time.deltaTime * xAreaTimeMultiplier;
            }
        }

        foreach (var item in backroundImages)
        {
            var pLax = item.GetComponent<ParallaxEffect>();
            if (pLax != null)
            {
                item.anchoredPosition = new Vector3(xArea * pLax.parallaxAmount, item.anchoredPosition.y, 0);
            }
            else
            {
                item.anchoredPosition = new Vector3(xArea, item.anchoredPosition.y, 0);
            }
            
        }
    }
}
