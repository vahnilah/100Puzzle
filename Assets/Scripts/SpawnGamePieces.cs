using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGamePieces : MonoBehaviour
{

    // Groups
    private Object[] allGamePieces;
    private int[] rotation = new int[] { 0, 90, 180, 270 };
        

    void Start()
    {
        allGamePieces = Resources.LoadAll("Prefab/GamePiecesAdj", typeof(GameObject));

        // Spawn game pieces
        SpawnPiecees();
    }

    private void Update()
    {
        // If there are no more game pieces to play, spawn more
        if (GamePieces.CountGamePieces() <= 0)
        {
            SpawnPiecees();
            DetectPossibleMoves.DidNewBlocksSpawn = true;
        }
    }

    public void SpawnPiecees()
    {
        // Get spawn location
        Vector2 v0 = new Vector2(-4, .5f);
        Vector2 v1 = new Vector2(-4, 5.5f);
        Vector2 v2 = new Vector2(-4, 10.5f);

        // Get a random rotation
        int r0 = rotation[Random.Range(0, rotation.Length)];
        int r1 = rotation[Random.Range(0, rotation.Length)];
        int r2 = rotation[Random.Range(0, rotation.Length)];

        // Get the pieces
        int[] pieceNumber = PreventThreeDuplicates();

        // Adjust the spawn location if it is an even gamepiece due to alignment issues
        // if it was align to .5 in the editor
        Vector2 v0u = AdjustBlock(allGamePieces[pieceNumber[0]].name, r0, v0);
        Vector2 v1u = AdjustBlock(allGamePieces[pieceNumber[1]].name, r1, v1);
        Vector2 v2u = AdjustBlock(allGamePieces[pieceNumber[2]].name, r2, v2);

        // Spawn the pieces
        CreateGamePiece(pieceNumber[0], v0u, r0);
        CreateGamePiece(pieceNumber[1], v1u, r1);
        CreateGamePiece(pieceNumber[2], v2u, r2);
    }


    // Prevent all 3 blocks to be dups to ensure gameplay is varied and fair
    private int[] PreventThreeDuplicates()
    {
        int n0 = GetRandomGamePiece();
        int n1 = GetRandomGamePiece();
        int n2 = GetRandomGamePiece();

        while ((n0.Equals(n1)) && (n0.Equals(n2)))
        {
            n1 = GetRandomGamePiece();
            n2 = GetRandomGamePiece();
        }

        return new int[] { n0, n1, n2 };
    }

    // Helper to create a game piece based on location.
    private void CreateGamePiece(int pieceNumber, Vector2 loc, int rotation)
    {
        Instantiate(allGamePieces[pieceNumber], loc, GetRandomRotation(rotation));
    }

    // Helper to get a random game piece.
    private int GetRandomGamePiece()
    {
        return Random.Range(0, allGamePieces.Length);
    }
    
    // Helper to assign a random rotation to a game piece.
    private Quaternion GetRandomRotation(int rotation)
    {
        Vector3 rotationVector = new Vector3(0, 0, rotation);
        return Quaternion.Euler(rotationVector);
    }

    // Helper method to adjust the position where it spawns
    private Vector2 AdjustBlock(string name, int rotate, Vector2 vec)
    {

        // Block_I2, .375f;
        if (name.Contains("Block_I2"))
        {
            if (rotate == 0)
            {
                vec.y -= .375f;
            }
            else if (rotate == 90)
            {
                vec.x += .375f;
            }
            else if (rotate == 180)
            {
                vec.y += .375f;
            }
            else if (rotate == 270)
            {
                vec.x -= .375f;
            }
        }
        // Block_I4, 1.125f;
        else if (name.Contains("Block_I4"))
        {
            if (rotate == 0)
            {
                vec.y -= 1.125f;
            }
            else if (rotate == 90)
            {
                vec.x += 1.125f;
            }
            else if (rotate == 180)
            {
                vec.y += 1.125f;
            }
            else if (rotate == 270)
            {
                vec.x -= 1.125f;
            }
        }
        // Block_O2
        else if (name.Contains("Block_O2"))
        {
            if (rotate == 0)
            {
                vec.x -= .375f;
                vec.y -= .375f;
            }
            else if (rotate == 90)
            {
                vec.x += .375f;
                vec.y -= .375f;
            }
            else if (rotate == 180)
            {
                vec.x += .375f;
                vec.y += .375f;
            }
            else if (rotate == 270)
            {
                vec.x -= .375f;
                vec.y -= .375f;
            }
        }
        // Block_L2
        else if (name.Contains("Block_L2"))
        {
            if (rotate == 0)
            {
                vec.x -= .375f;
                vec.y -= .375f;
            }
            else if (rotate == 90)
            {
                vec.x += .375f;
                vec.y -= .375f;
            }
            else if (rotate == 180)
            {
                vec.x += .375f;
                vec.y += .375f;
            }
            else if (rotate == 270)
            {
                vec.x -= .375f;
                vec.y -= .375f;
            }
        }
        return vec;
    }

}
