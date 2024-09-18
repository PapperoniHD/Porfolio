using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishString : MonoBehaviour
{
    private RectTransform hookPos;


    public float xResetSpeed = 1;
    public float xSpeed = 5;
    public float ySpeed = 5;

    public float yMax = 300;
    public float yLowest = 100;

    public float xMax;
    public float xLowest;

    private float yPos;
    private WaterBackground waterBackground;
    private bool catchEnabled = false;


    public GameObject underwaterCanvas;

    public GameObject missGO;   
    private Animator missAnim;

    public GameObject catchGO;
    private Animator catchAnim;

    public GameObject winGO;
    private Animator winAnim;

    public bool caughtFish = false;

    public GameObject duckOnString;
    private DuckPickup duckPickup;

    public GameObject reelSound;

    // Start is called before the first frame update
    void Awake()
    {
        hookPos = GetComponent<RectTransform>();
        waterBackground = FindObjectOfType<WaterBackground>();
        duckPickup = FindObjectOfType<DuckPickup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (FishGameManager.fGM.inputEnabled)
        {
            Move();
        }
        if (catchEnabled )
        {
            CatchUpdate();
        }
        
    }

    private void OnEnable()
    {
        var stringAnim = GetComponentInChildren<Animator>();

        stringAnim.SetTrigger("Reset");
        stringAnim.ResetTrigger("Reset");

        FishGameManager.fGM.inputEnabled = true;
        waterBackground.ResetGameField();

        caughtFish = false;
        FishGameManager.fGM.UpdateCaughtDucksText();
    }

    public IEnumerator Catch()
    {
        reelSound.SetActive(true);
        caughtFish = duckPickup.Caught();
        if (caughtFish)
        {
            duckOnString.SetActive(true);
        }
        FishGameManager.fGM.inputEnabled = false;
        print(hookPos.position.y);
        catchEnabled = true;
        while (waterBackground.t > 0)
        {
            yield return null;
        }

        catchEnabled = false;

        if (FishGameManager.fGM.caughtDucks < FishGameManager.fGM.amountOfDucks - 1)
        {
            if (caughtFish)
            {
                CatchAnim();
            }
            else
            {
                MissAnim();
            }
        }
        else
        {
            if (caughtFish)
            {
                Win();
            }
            else
            {
                MissAnim();
            }
            
        }

        reelSound.SetActive(false);
        duckOnString.SetActive(false);
        underwaterCanvas.SetActive(false);
    }

    void Win()
    {
        winGO.SetActive(true);
        winAnim = winGO.GetComponent<Animator>();
        if (winAnim != null)
        {
            winAnim.Play("WinAnim");
        }
    }

    void MissAnim()
    {
        missGO.SetActive(true);
        missAnim = missGO.GetComponent<Animator>();
        if (missAnim != null)
        {
            missAnim.Play("MissAnim");
        }
    }

    void CatchAnim()
    {
        FishGameManager.fGM.caughtDucks++;
        catchGO.SetActive(true);
        catchAnim = catchGO.GetComponent<Animator>();
        if (catchAnim != null)
        {
            catchAnim.Play("CatchAnim");
        }
    }

    void CatchUpdate()
    {
        hookPos.anchoredPosition = Vector3.Lerp(hookPos.anchoredPosition,
                new Vector3(hookPos.anchoredPosition.x, yMax, 0), Time.deltaTime * ySpeed);

        waterBackground.t -= Time.deltaTime;      
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(Catch());        
        }


        if (Input.GetKey(KeyCode.A))
        {
            hookPos.anchoredPosition = Vector3.Lerp(hookPos.anchoredPosition,
                new Vector3(xLowest, hookPos.anchoredPosition.y, 0), Time.deltaTime * xSpeed);

            waterBackground.XMove(true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            hookPos.anchoredPosition = Vector3.Lerp(hookPos.anchoredPosition,
                new Vector3(xMax, hookPos.anchoredPosition.y, 0), Time.deltaTime * xSpeed);

            waterBackground.XMove(false);
        }
        else
        {
            hookPos.anchoredPosition = Vector3.Lerp(hookPos.anchoredPosition,
                new Vector3(0, hookPos.anchoredPosition.y, 0), Time.deltaTime * xResetSpeed);
        }


        if (Input.GetKey(KeyCode.W))
        {
            hookPos.anchoredPosition = Vector3.Lerp(hookPos.anchoredPosition,
                new Vector3(hookPos.anchoredPosition.x, yMax, 0), Time.deltaTime * ySpeed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            hookPos.anchoredPosition = Vector3.Lerp(hookPos.anchoredPosition,
                new Vector3(hookPos.anchoredPosition.x, yLowest, 0), Time.deltaTime * ySpeed);
        }
        else
        {
            hookPos.anchoredPosition = Vector3.Lerp(hookPos.anchoredPosition,
                new Vector3(hookPos.anchoredPosition.x, Mathf.Lerp(yMax, yLowest, 0.5f), 0), Time.deltaTime * ySpeed);
        }
    }
}
