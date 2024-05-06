using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int xDim;
    public int yDim;

    public float tileOffset = 0f;

    public GameObject tileAsset;

    public List<GameObject> tiles = new List<GameObject>();
    public Vector2 startPos = new Vector2(0f, 0f);

    void Start()
    {
        InitializeGrid();       
    }

    public void InitializeGrid()
    {
        //Adds tiles to the grid and sets their parent scale and tile coordinate (tileCoordinate)
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                //Add Tiles
                tiles.Add(Instantiate(tileAsset, this.transform));
                tiles[tiles.Count - 1].transform.parent = transform.Find("Tiles");
                tiles[tiles.Count - 1].name = "Tile(" + x + "," + y + ")";
                tiles[tiles.Count - 1].GetComponent<Tile>().tileCoordinate = new Vector2(x,y);
                tiles[tiles.Count - 1].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
        }
        SetTilePosition();
    }

    void SetTilePosition()
    {
        //Sets the tiles position with an offset of their scale and a custom offset
        var tileRect = tiles[0].GetComponent<RectTransform>();
        Vector2 offset = new Vector2();

        offset.x = (tileRect.rect.width * tileRect.transform.localScale.x + tileOffset);
        offset.y = (tileRect.rect.height * tileRect.transform.localScale.y + tileOffset);

        int column = 0;
        int row = 0;

        
        foreach (GameObject tile in tiles)
        {
            if (column + 1 > yDim)
            {
                row++;
                column = 0;
            }

            var xOffset = offset.x * column;
            var yOffset = offset.y * row;

            tile.GetComponent<RectTransform>().anchoredPosition = new Vector3(startPos.x + xOffset, startPos.y - yOffset);
            column++;

        }
       
    }

    //Checks each sudoku value and sets the number on each correct tile 
    //This gets called on InitializeGame(); in GameHandler
    public void SetGridNumber()
    {     
        int index = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                tiles[index].GetComponent<Tile>().SetNumber(GameHandler.singleton.sudoku.GetValue(i, j));
                index++;
            }
        }

    }
}
