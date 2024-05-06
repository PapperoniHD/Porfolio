using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// This class is in every tile in the sudoku board and handles
/// tile properties
/// </summary>
public class Tile : Selectable, ISelectHandler
{
    public Grid grid;

    int tileNumber = 0;
    public int tileBlock;
    public TextMeshProUGUI numberText;
    public bool canChange;

    public Vector2 tileCoordinate;

    //Colors
    public Color revealedValueColor;
    public Color normalColor;
    public Color highlightedColor;
    public Color highlightGray;

    public void SetText()
    {            
        if (tileNumber <= 0)
        {
            //Makes the tile empty and changeable if tilenumber is zero
            numberText.text = " ";
            canChange = true;         
        }
        else
        {
            //Sets the tile number to text, disabling number changeability and disables interactability
            numberText.text = tileNumber.ToString();
            canChange = false;
            GetComponent<Tile>().interactable = false;
        }
        //Sets color
        ResetColor();
        //Finds grid
        grid = FindObjectOfType<Grid>();
    }
    //Sets the tile number
    public void SetNumber(int number)
    {
        tileNumber = number;
        SetText();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        GameHandler.singleton.SetSelectedTile(this);
        HighlightTiles();
    }

    public override void OnDeselect(BaseEventData data)
    {
        ResetColor();
    }

    //void for resetting color value for unselected tile
    public void ResetColor()
    {
        Image color = GetComponentInChildren<Image>();
        if (canChange)
        {
            color.color = normalColor;
        }
        else
        {
            color.color = revealedValueColor;
        }
    }
    //void for setting highlighted tile color
    public void HighlightTiles()
    {
        //Checks if grid isnt null and highlights the colors in the y and x direction, and resets other tiles colors
        if (grid == null) return;
        
        foreach (var tile in grid.tiles)
        {      
            var tileScript = tile.GetComponent<Tile>();

            if (tileScript.tileCoordinate.x == this.tileCoordinate.x
                || tileScript.tileCoordinate.y == this.tileCoordinate.y)
            {
                tileScript.SetGray();
            }
            else
            {
                tileScript.ResetColor();
            }
        }
        Image color = GetComponentInChildren<Image>();
        color.color = highlightedColor;     
    }

    public void SetGray()
    {
        //Sets the color of surrounding, row and column tiles and different color if its changeable
        Image color = GetComponentInChildren<Image>();
        if (canChange)
        {
            color.color = highlightGray;
        }
        else
        {
            color.color = highlightGray - new Color(20,20,20);
        }
        
    }
}

