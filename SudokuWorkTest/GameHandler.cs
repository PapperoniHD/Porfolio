using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Difficulty
{
    Easy,
    Medium,
    Hard,
    Expert,
    Custom
}

/// <summary>
/// This class is the Game Handler/Game Manager which handles the game
/// and is a singleton instance for other scripts to call
/// </summary>
// Should've named this GameManager but works the same
public class GameHandler : MonoBehaviour
{
    public Sudoku sudoku = new Sudoku();
    public static GameHandler singleton;
    public Difficulty difficulty;
    public Tile selectedTile = null;
    private Grid grid;

    private int revealedNumbers;

    private float timer = 0f;
    private bool isTimer = false;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI winTimerText;
    public GameObject startScreen;

    public GameObject winText;


    // Start is called before the first frame update
    void Start()
    {
        singleton = this;
        grid = FindObjectOfType<Grid>();
        difficulty = Difficulty.Easy;

        winText.SetActive(false);     
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
    }

    //Initializes the game, aka checks difficulty and genereates sudoku for instance
    //Gets called when pressing the start button
    public void InitializeGame()
    {       
        Destroy(startScreen);
        DetermineDifficultyValues();
        sudoku = Sudoku.Generate(revealedNumbers);
        grid.SetGridNumber();
        isTimer = true;
    }

    //Sets the difficulty from the dropdown button function in NumpadAndButtons
    public void SetDifficulty(Difficulty setDiff)
    {
        difficulty = setDiff;
    }

    void DetermineDifficultyValues()
    {
        //Determines Difficulty
        switch (difficulty)
        {
            case Difficulty.Easy:
                revealedNumbers = GameData.Easy;
                break;
            case Difficulty.Medium:
                revealedNumbers = GameData.Medium;
                break;
            case Difficulty.Hard:
                revealedNumbers = GameData.Hard;
                break;
            case Difficulty.Expert:
                revealedNumbers = GameData.Expert;
                break;
            case Difficulty.Custom:
                revealedNumbers = GameData.Custom;
                break;
            default:
                break;
        }
    }
    
    public void SetSelectedTile(Tile tile)
    {
        selectedTile = tile;
    }

    //Checks if sudoku is complete, runs every time a number in the sudoku is changed
    public void SudokuCompleteCheck()
    {
        if (sudoku.IsComplete())
        {
            winTimerText.text = "Time: " + timerText.text;
            winText.SetActive(true);
            isTimer = false;

        }
    }

    void Timer()
    {
        //Simple Timer
        if (isTimer)
        {
            timer += Time.deltaTime;

            int minutes = Mathf.FloorToInt(timer / 60.0f);
            int seconds = Mathf.FloorToInt(timer - minutes * 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        
    }
}
