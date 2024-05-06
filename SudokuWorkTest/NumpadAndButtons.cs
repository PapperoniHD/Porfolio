using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// This class initializes every button 
/// and handles functions for every button
/// (that aren't the sudoku board)
/// </summary>
public class NumpadAndButtons : MonoBehaviour
{
    public Button[] numpadButtons;
    public TMP_Dropdown difficultyDropdown;

    public TextMeshProUGUI difficultyText;

    private void Start()
    {
        //Sets the number value for each button in order
        for (int i = 0; i < numpadButtons.Length; i++)
        {
            numpadButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = (i+1).ToString();
        }
    }


    //Function for the numpad button press, runs when changing number in a tile
    public void ChangeNumber()
    {
        GameObject clickedbutton = EventSystem.current.currentSelectedGameObject;

        //Will continue if there is a selected tile and its a changeable tile
        if (GameHandler.singleton.selectedTile == null) return;
        if (GameHandler.singleton.selectedTile.canChange == false) return;

        //Changes the text of the selected tile
        Tile currentTile = GameHandler.singleton.selectedTile;
        TextMeshProUGUI tileText = currentTile.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI buttonText = clickedbutton.GetComponentInChildren<TextMeshProUGUI>();

        tileText.text = buttonText.text;

        //Converting the string number to int
        int newValue = Convert.ToInt32(buttonText.text);

        //Sets the value in the sudoku
        GameHandler.singleton.sudoku.SetValue((int)currentTile.tileCoordinate.x, (int)currentTile.tileCoordinate.y, newValue);

        //Checks if sudoku is complete
        GameHandler.singleton.SudokuCompleteCheck();

    }

    public void Erase()
    {
        //Will continue if there is a selected tile and its a changeable tile
        if (GameHandler.singleton.selectedTile == null) return;
        if (GameHandler.singleton.selectedTile.canChange == false) return;

        //Changes the text of the selected tile
        Tile currentTile = GameHandler.singleton.selectedTile;
        TextMeshProUGUI tileText = currentTile.gameObject.GetComponentInChildren<TextMeshProUGUI>();

        //Resets the tile text and value in the sudoku
        tileText.text = " ";
        GameHandler.singleton.sudoku.SetValue((int)currentTile.tileCoordinate.x, (int)currentTile.tileCoordinate.y, 0);
    }
    public void StartGame()
    {
        GameHandler.singleton.InitializeGame();
    }

    //Difficulty dropdown function for changing difficulty
    public void ChangeDifficulty()
    {
        var index = difficultyDropdown.value;
        switch (index)
        {
            case 0:
                GameHandler.singleton.SetDifficulty(Difficulty.Easy);
                break;
            case 1:
                GameHandler.singleton.SetDifficulty(Difficulty.Medium);
                break;
            case 2:
                GameHandler.singleton.SetDifficulty(Difficulty.Hard);
                break;
            case 3:
                GameHandler.singleton.SetDifficulty(Difficulty.Expert);
                break;
            default:
                break;
        }

        difficultyText.text = "Current Difficulty: " + GameHandler.singleton.difficulty.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
   
}
