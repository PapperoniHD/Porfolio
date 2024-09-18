using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Febucci.UI.Core;
public enum GameState
{
    Gameplay,
    Hiding,
    Cutscene,
    Reading,
    Paused,
    Elevator,
    Dialogue,
    Battleship
}

public class GameManager : MonoBehaviour
{
    public static GameManager GM;
    public GameState State { get; private set; }

    [Header("Canvas for transition")]
    public GameObject transition;

    // Start is called before the first frame update
    void Awake()
    {
        GM = this;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //Gameplay();
        transition = GameObject.Find("Transition");
        TAnimBuilder.InitializeGlobalDatabase();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Transition()
    {
        StartCoroutine(TransitionEnum());
    }

    IEnumerator TransitionEnum()
    {
        Animator canvasAnim = transition.GetComponent<Animator>();
        canvasAnim.Play("TransitionClose");
        yield return new WaitForSeconds(1f);
        canvasAnim.Play("TransitionOpen");
    }

    public void Gameplay()
    {
        State = GameState.Gameplay;
    }
    public void Hiding()
    {
        State = GameState.Hiding;
    }

    public void Dialogue()
    {
        State = GameState.Dialogue;
    }

    public void Elevator()
    {
        State = GameState.Elevator;
    }

    public void Cutscene()
    {
        State = GameState.Cutscene;
    }

    public void Reading()
    {
        State = GameState.Reading;
    }

    public void Paused()
    {
        State = GameState.Paused;
    }

    public void Battleship()
    {
        State = GameState.Battleship;
    }

}
