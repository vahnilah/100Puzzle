using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePieces : MonoBehaviour
{
    private static bool hasClickedObjectBeenReleased = true;
    public static string UUIDofObject { get; private set; }
    public static DetectObjectClicked gameObjectDetection;
    private static ClickAndDrop objectClickAndDrop;
    private static readonly float OriginalScale = 0.75f;
    public static bool isAPlayValid;
    public static bool HasAnGamePieceBeenPlayedToUpdateScore = false;

    // GamePiece is the main playable piece
    // GameBlock is the blocks within a gamepiece
    // Therefore: A GamePiece will usually contain many game blocks
    private static GameObject[] CollectionOfgameBlocks { get; set; }
    private static GameObject[] CollectionOfGamePieces { get; set; }
    public static Vector3 ParentOriginalPosition { get; private set; }
    public static Vector3 ParentUnusedBlockPosition { get; private set; }

    public static Vector3 ParentOriginalScale { get; private set; }


    // Static constructors to call to run this class.
    public static void FindGameBlocks()
    {
        CollectionOfgameBlocks = GameObject.FindGameObjectsWithTag("GameBlock");
    }
    public static void FindGamePieces()
    {
        CollectionOfGamePieces = GameObject.FindGameObjectsWithTag("GamePiece");

    }

    public static GameObject[] WhatGamePieces()
    {
        return GameObject.FindGameObjectsWithTag("GamePiece");
    }

    // Find what objects are in play.
    public static int CountGameBlocks()
    {
        //Debug.Log("Blocks: " + CollectionOfgameBlocks.Length);
        return CollectionOfgameBlocks.Length;
    }
    public static int CountGamePieces()
    {
        //Debug.Log("Pieces: " + CollectionOfGamePieces.Length);
        return CollectionOfGamePieces.Length;
    }
    public static void CountTotal()
    {
        Debug.Log("Blocks: (" + CollectionOfgameBlocks.Length +
            ") | Pieces: (" + CollectionOfGamePieces.Length + ")");
    }

    // Find where the game blocks are at
    public static void WhereAreGameBlocks()
    {
        foreach (var obj in CollectionOfgameBlocks)
        {
            float x = obj.transform.position.x;
            float y = obj.transform.position.y;
            float z = obj.transform.position.z;
            Debug.Log(x + " " + y + " " + z);

            //Debug.Log(InsideBorder(x, y));

        }
    }

    // Find where the game blocks are at
    // Return: [0] # of pieces on board
    //         [1] # of pieces missing from board
    public static int[] CountGameBlocksInsideGrid()
    {
        int onBoard = 0;
        int missingFromBoard = 0;
        foreach (var obj in CollectionOfgameBlocks)
        {
            try
            {
                float x = obj.transform.position.x;
                float y = obj.transform.position.y;

                if (GameGrid.InsideBorder(new Vector2(x, y)))
                {
                    onBoard++;
                }
            }
            catch (MissingReferenceException)
            {
                //Debug.Log("Hi");
                //missingFromBoard++;
            }
        }
        //Debug.Log(onBoard);
        //Debug.Log(missingFromBoard);
        return new int[] { onBoard, missingFromBoard };
    }

    // Find where the game blocks are at
    // Return: [0] # of pieces on board
    //         [1] # of pieces missing from board
    public static int CountGameBlocksOutsideGrid()
    {
        int offBoard = 0;
        foreach (var obj in CollectionOfgameBlocks)
        {
            try
            {
                float x = obj.transform.position.x;
                float y = obj.transform.position.y;

                if (!GameGrid.InsideBorder(new Vector2(x, y)))
                {
                    offBoard++;
                }
            }
            catch (MissingReferenceException)
            {
                //Debug.Log("Hi");
                //missingFromBoard++;
            }
        }
        //Debug.Log(onBoard);
        //Debug.Log(missingFromBoard);
        return offBoard;
    }


    // Main method that runs to find what objects are valid and plays it on the grid
    public static void WhereAreGamePiecesToPlay()
    {
        GamePieces.isAPlayValid = false;
        foreach (GameObject oneGamePiece in CollectionOfGamePieces)
        {
            bool isAGameBlockOutsideTheBoard = false;
            bool isAGameBlockOverLappingAValidPlay = false;
            bool isObjectClicked = false;
            gameObjectDetection = oneGamePiece.GetComponent<DetectObjectClicked>();
            objectClickAndDrop = oneGamePiece.GetComponent<ClickAndDrop>();
            if (gameObjectDetection.IsObjectClicked)
                objectClickAndDrop.enabled = true;


            // Save position of gamePiece if invalid placement
            if (Input.GetMouseButtonDown(0))
            {
                // Detect if gamePiece has the script
                if (oneGamePiece.GetComponent<DetectObjectClicked>() != null)
                {                    
                    isObjectClicked = gameObjectDetection.IsObjectClicked;
                        
                    // Get the Block ID if an object has been clicked.
                    if(isObjectClicked && hasClickedObjectBeenReleased)
                    {
                        UUIDofObject = gameObjectDetection.UUID;

                        hasClickedObjectBeenReleased = false;
                    }
                }

            } 

            // Get children
            Transform[] manyGameBlocks = oneGamePiece.GetComponentsInChildren<Transform>();

            // Get children position for valid placement
            foreach (var oneGameBlock in manyGameBlocks)
            {
                //  The main game block [Not the children of it]
                if (GetGameBlockTag(oneGameBlock))
                {
                    float x = oneGameBlock.position.x;
                    float y = oneGameBlock.position.y;

                    //Debug.Log("X: " + x + " Y: " + y);
                    //Debug.Log(InsideBorder(x, y));
                    isAGameBlockOutsideTheBoard = InsideBorder(x, y) ? isAGameBlockOutsideTheBoard : true;
                    //if (!InsideBorder(x, y))
                    //    isAGameBlockOutsideTheBoard = true;

                    Vector2 v = GameGrid.Vector2Round(oneGameBlock.position);

                    // Mark true: if a game piece is overlapping
                    if (GameGrid.IsAGamePieceInsideGridAt(v))
                        isAGameBlockOverLappingAValidPlay = true;

                }
            }

            // FOR VALID PLAY
            // If: children position is valid, destory gamepiece & leave gameblocks on board
            // When the mouse is released
            bool isAGameBlockInsideTheBoard = !isAGameBlockOutsideTheBoard;
            if (!isAGameBlockOverLappingAValidPlay && isAGameBlockInsideTheBoard && Input.GetMouseButtonUp(0))
            {
                foreach (var oneGameBlock in manyGameBlocks)
                {
                    // Align the gameblocks on the grid
                    var pos = GameGrid.Vector2Round(oneGameBlock.position);
                    oneGameBlock.position = pos;

                    if (GetGameBlockTag(oneGameBlock))
                    {
                        oneGameBlock.parent = null;
                        Destroy(oneGamePiece);

                        // Place valid piece on game grid
                        Vector2 v = GameGrid.Vector2Round(oneGameBlock.position);
                        GameGrid.grid[(int)v.x, (int)v.y] = oneGameBlock;

                        // Change the background grid color
                        AnimateGrid.ChangeBgGridColor((int)v.x, (int)v.y);

                        hasClickedObjectBeenReleased = true;
                        isAPlayValid = true;


                    }
                }
            }

            // FOR INVALID PLAY
            // Else: restore gamepiece to it's orginal location for an invalid play
            else if (isAGameBlockOverLappingAValidPlay && Input.GetMouseButtonUp(0) ||
                        isAGameBlockOutsideTheBoard && Input.GetMouseButtonUp(0))
            {
                if (UUIDofObject == gameObjectDetection.UUID)
                {
                    oneGamePiece.transform.position = gameObjectDetection.PositionOfObject;
                    var localScale = oneGamePiece.transform.localScale;
                    localScale.x = OriginalScale;
                    localScale.y = OriginalScale;
                    oneGamePiece.transform.localScale = localScale;
                    hasClickedObjectBeenReleased = true;
                    //objectClickAndDrop.enabled = false;
                }          
            } 
        }
    }

    // Helper method to find gameblock tag
    private static bool GetGameBlockTag(Transform child)
    {
        return child.tag == "GameBlock";
    }

    // Helper method to determine if the game is within valid placement
    private static bool InsideBorder(float x, float y)
    {
        return (Mathf.Round(x) >= 0 &&
                Mathf.Round(x) < GameGrid.width &&
                Mathf.Round(y) >= 0) &&
                Mathf.Round(y) < GameGrid.height;
    }

}
