using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FishGameManager : MonoBehaviour
{
    public static FishGameManager fGM;
    // Start is called before the first frame update

    public bool inputEnabled = false;

    public int amountOfDucks = 5;
    public int caughtDucks;

    public TextMeshProUGUI amountOfDucksText;
    public TextMeshProUGUI depthText;

    public int depth;

    private void Awake()
    {
        fGM = this;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Start()
    {
        StartCoroutine(StartGame());
        UpdateCaughtDucksText();
    }

    // Update is called once per frame
    void Update()
    {
        depthText.SetText("Depth: " + depth.ToString() + " m");
    }

    public void UpdateCaughtDucksText()
    {
        amountOfDucksText.SetText("Caught Ducks " + caughtDucks.ToString() + " / 5");
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(7);
        inputEnabled = true;
}
}
