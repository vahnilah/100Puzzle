using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    // Did a row/col complete
    public static bool canRowsBeCleared = false;
    public static bool canColsBeCleared = false;

    // Time
    public static float startTime = 0f;
    public static float endTime = 2f;

    // Define the Grid
    public static int width = 10;
    public static int height = 10;

    // Create the invisble grid to manage pieces
    public static Transform[,] grid = new Transform[width, height];

    public static HashSet<int> whatRowIsFilled = new HashSet<int>();
    public static HashSet<int> whatColIsFilled = new HashSet<int>();

    public static int howManyRowsColsWereFilled = 0;

    public static int numberOfBlocks { get; private set; }
    public static int numberOfRows { get; private set; }

    //public void Start()
    //{
    //    for (int x = 0; x < width; x++)
    //        for (int y = 0; y < height; y++)
    //        grid[x, y].gameObject.GetComponentInChildren<Animator>().enabled = false;
    //}


    // Obtain nearest whole number on grid
    public static Vector2 Vector2Round(Vector2 vec)
    {
        return new Vector2(Mathf.Round(vec.x),
                           Mathf.Round(vec.y));
    }

    // Check if inside in border
    public static bool InsideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 &&
            (int)pos.x < width &&
            (int)pos.y >= 0);
    }

    // Return true if a piece is at a grid location
    public static bool IsAGamePieceInsideGridAt(Vector2 v)
    {
        try
        {
            if (grid[(int)v.x, (int)v.y] == null)
                return false;
            return true;
        }

        catch (System.IndexOutOfRangeException)
        {
            return false;
        }
    }

    // Update grid & count the pieces on the grid
    public static void UpdatePlayedPiecesOnGrid(MonoBehaviour mono)
    {
        // Count the number of played blocks on the grid for scoring
        numberOfBlocks = 0;

        // Remove old children from GameGrid
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                if (grid[x, y] != null)
                {
                    if (grid[x, y].parent == mono.transform)
                        grid[x, y] = null;
                    numberOfBlocks++;
                }

        // Add new children to GameGrid
        foreach (Transform child in mono.transform)
        {
            Vector2 v = Vector2Round(child.position);
            grid[(int)v.x, (int)v.y] = child;
        }
    }

    // Delete rows if filled
    public static void DeleteFilledRows()
    {
        for (int y = 0; y < height; y++)
        {
            if (IsRowFilled(y))
            {
                canRowsBeCleared = true;
                AnimateGrid.runAnimationScript = true;

                // What row is filled is added to the hashset
                whatRowIsFilled.Add(y);

            }
        }
    }

    #region Repeated helper logic to delete rows

    // Check if row is filled
    private static bool IsRowFilled(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
                return false;
        }
        return true;
    }

    // Delete row
    public static void DeletePlayedBlocksOnRow()
    {

        foreach (int y in whatRowIsFilled)
        {
            //Debug.Log(xy);
            for (int x = 0; x < width; x++)
            {
                // Destroy blocks from the player
                if (grid[x, y] != null)
                    Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }
        }
    }


    #endregion

    // Delete colums if filled
    public static void DeleteFilledCollumns()
    {
        for (int x = 0; x < width; x++)
        {
            if (IsCollumnFilled(x))
            {
                canColsBeCleared = true;

                //DeletePlayedBlocksOnCollumns(x);
                whatColIsFilled.Add(x);
            }
        }
    }

    #region Repeated helper logic to delete collumns

    // Check if row is filled
    private static bool IsCollumnFilled(int x)
    {
        for (int y = 0; y < height; y++)
        {
            if (grid[x, y] == null)
                return false;
        }
        return true;
    }

    // Delete row
    public static void DeletePlayedBlocksOnCollumns()
    {
        foreach (int x in whatColIsFilled)
        {
            for (int y = 0; y < height; y++)
            {
                // Destroy blocks from the player
                if (grid[x, y] != null)
                    Destroy(grid[x, y].gameObject);
                grid[x, y] = null;

                // Obtain background grid location
                //AnimateGrid.cols.Add(AnimateGrid.GetIndexOfBgGrid(x, y));

            }
        }
        canRowsBeCleared = true;
    }

    #endregion


    public static void DeleteBlock(int x, int y)
    {
        Destroy(grid[x, y].gameObject);
        grid[x, y] = null;
    }


}
