using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Cinemachine;
public class BattleshipsGame : MonoBehaviour
{
    public GameObject ui;
    public TextMeshProUGUI battleshipText;

    public GridElement[] shipTilesArray;

    public GridElement[] allGridElements;

    public OpponentElement[] opponentShipTilesArray;

    public OpponentElement[] aiElements;

    public List<OpponentElement> aiElementList;

    public OpponentElement chosenAiElement;

    public GridElement chosenElement;

    public GameObject rollSound;

    public CinemachineVirtualCamera vcam;

    public GameObject dialogueCollider;

    private DialogueScript dialogueScript;

    public GameObject winDialogueOBJ;

    public Transform pos;

    public GameObject player;

    public SkinnedMeshRenderer skin;

    public GameObject loseDialogue;

    public GameObject originalDialogueCollider;

    // Start is called before the first frame update
    void Start()
    {
        ui.SetActive(false);
        dialogueScript = FindObjectOfType<DialogueScript>();
        rollSound.SetActive(false);
    }

 
    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBattleship()
    {
        ui.SetActive(true);
        StartCoroutine(StartGame());
    }

    public IEnumerator Win()
    {

        dialogueCollider.SetActive(false);

        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = pos.position;
        player.transform.rotation = pos.rotation;

        battleshipText.SetText("YOU WIN!!!");
        yield return new WaitForSeconds(2);
        ui.SetActive(false);
        vcam.Priority = 0;
        yield return new WaitForSeconds(2);
        //GameManager.GM.Gameplay();
        dialogueScript.dialogueObj = winDialogueOBJ;
        dialogueScript.SetLines(winDialogueOBJ.GetComponent<DialogueText>().firstLines);
        dialogueScript.SetLookAt(winDialogueOBJ.transform);
        StartCoroutine(dialogueScript.StartDialogue());
        skin.enabled = true;
    }

    public IEnumerator Lose()
    {   
        battleshipText.SetText("you lost...");
        yield return new WaitForSeconds(2);
        ResetGame();
        ui.SetActive(false);
        vcam.Priority = 0;
        originalDialogueCollider.SetActive(false);
        loseDialogue.SetActive(true);
        yield return new WaitForSeconds(2);
        dialogueScript.dialogueObj = loseDialogue;
        dialogueScript.SetLines(loseDialogue.GetComponent<DialogueText>().firstLines);
        dialogueScript.SetLookAt(loseDialogue.transform);
        StartCoroutine(dialogueScript.StartDialogue());
        skin.enabled = true;
    }

    public void ResetGame()
    {
        battleshipText.SetText("Battleships");
        aiElementList.Clear();
        foreach (var item in allGridElements)
        {
            item.ResetButton();
        }
        foreach (var item in aiElements)
        {
            item.ResetButton();
        }
    }

    public void Clicked()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(PlayerHitOrMissText());
        StartCoroutine(Delay(2));
    }

    public IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (shipTilesArray.All(obj => obj.clicked))
        {
            StartCoroutine(Win());
        }
        else
        {
            OpponentTurn();
        }
        
    }

    public void OpponentTurn()
    {
        battleshipText.SetText("Opponents Turn");
        chosenAiElement = ChooseRandom();

        StartCoroutine(OpponentTurnDelay());
        
    }

    public IEnumerator OpponentTurnDelay()
    {
        StartCoroutine(OpponentTurnAnimation());
        rollSound.SetActive(true);
        yield return new WaitForSeconds(5);
        rollSound.SetActive(false);
        foreach (var item in aiElements)
        {
            item.GetComponent<Animator>().Play("Normal");
        }
        chosenAiElement.Clicked();
        OpponentHitOrMissText();
        if (chosenAiElement != null)
        {
            StartCoroutine(YourTurn());
        }
        else
        {
            print("Theres no Element Chosen");
        }
    }

    void OpponentHitOrMissText()
    {
        if (chosenAiElement.shipTile)
        {
            battleshipText.SetText("HIT!!!");
        }
        else
        {
            battleshipText.SetText("miss...");
        }
    }

    IEnumerator PlayerHitOrMissText()
    {
        yield return new WaitForSeconds(0.1f);
        if (chosenElement.shipTile)
        {
            battleshipText.SetText("HIT!!!");
        }
        else
        {
            battleshipText.SetText("miss...");
        }
    }

    public IEnumerator OpponentTurnAnimation()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < aiElements.Length; j++)
            {
                aiElements[j].GetComponent<Animator>().Play("GridElementAnim");
                yield return new WaitForSeconds(0.1f);
            }
            foreach (var item in aiElements)
            {
                item.GetComponent<Animator>().Play("Normal");
            }
        }
       
    }

    public OpponentElement ChooseRandom()
    {
        //var randomTile = aiElements[Random.Range(0, aiElements.Length)];   
        var randomTile = aiElementList[Random.Range(0, aiElementList.Count)];
        aiElementList.Remove(randomTile);
        return randomTile;       
    }

    public IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(YourTurn());
    }

    public IEnumerator YourTurn()
    {
        yield return new WaitForSeconds(2);

        if (opponentShipTilesArray.All(obj => obj.alreadyClicked))
        {
            StartCoroutine(Lose());
        }
        else
        {
            battleshipText.SetText("Your turn");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }       
    }
}
