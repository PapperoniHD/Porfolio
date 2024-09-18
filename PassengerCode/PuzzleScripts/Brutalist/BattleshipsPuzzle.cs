using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BattleshipsPuzzle : MonoBehaviour, IDialogueEvent
{
    public CinemachineVirtualCamera BattleshipCam;
    public SkinnedMeshRenderer mesh;
    public GameObject player;
    public Transform pos;
    private DialogueScript dialogueScript;
    private BattleshipStartGame startGame;
    public GameObject[] sitNpcSwitch;

    public void Event()
    {
        StartCoroutine(StartBattleships());
    }

    public IEnumerator StartBattleships()
    {
        GameManager.GM.Battleship();
        player.GetComponent<CharacterController>().enabled = false;
        //StartCoroutine(dialogueScript.EndDialogue());
        BattleshipCam.Priority = 10;
        mesh.enabled = false;
        yield return new WaitForSeconds(3);
        player.transform.position = pos.position;
        player.transform.rotation = pos.rotation;
        startGame.enabled = true;
        sitNpcSwitch[0].SetActive(false);
        sitNpcSwitch[1].SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        dialogueScript = FindObjectOfType<DialogueScript>();
        startGame = GetComponent<BattleshipStartGame>();
        startGame.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
