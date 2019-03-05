using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPossibleMoves : MonoBehaviour
{
    public static bool DidNewBlocksSpawn { get; set; }
    public static bool IsGameOver { get; set; }

    // 270 is -90 when running
    public static bool Block_I2_0 { get; private set; }
    public static bool Block_I3_0 { get; private set; }
    public static bool Block_I4_0 { get; private set; }
    public static bool Block_I5_0 { get; private set; }
    public static bool Block_I2_90 { get; private set; }
    public static bool Block_I3_90 { get; private set; }
    public static bool Block_I4_90 { get; private set; }
    public static bool Block_I5_90 { get; private set; }
    public static bool Block_O1 { get; private set; }
    public static bool Block_O2 { get; private set; }
    public static bool Block_O3 { get; private set; }
    public static bool Block_L2_0 { get; private set; }
    public static bool Block_L2_90 { get; private set; }
    public static bool Block_L2_180 { get; private set; }
    public static bool Block_L2_270 { get; private set; }
    public static bool Block_L3_0 { get; private set; }
    public static bool Block_L3_90 { get; private set; }
    public static bool Block_L3_180 { get; private set; }
    public static bool Block_L3_270 { get; private set; }

    public static void SetBooleans()
    {
        DidNewBlocksSpawn = false;
        IsGameOver = false;

        Block_I2_0 = false;
        Block_I3_0 = false;
        Block_I4_0 = false;
        Block_I5_0 = false;

        Block_I2_90 = false;
        Block_I3_90 = false;
        Block_I4_90 = false;
        Block_I5_90 = false;

        Block_O1 = false;
        Block_O2 = false;
        Block_O3 = false;

        Block_L2_0 = false;
        Block_L2_90 = false;
        Block_L2_270 = false;
        Block_L2_180 = false;

        Block_L3_0 = false;
        Block_L3_90 = false;
        Block_L3_180 = false;
        Block_L3_270 = false;
    }

    //public static bool AnyMovesLeft()
    //{
    //    if (Block_I2_0) return true;
    //    if (Block_I3_0) return true;
    //    if (Block_I4_0) return true;
    //    if (Block_I5_0) return true;
    //    if (Block_I2_90) return true;
    //    if (Block_I3_90) return true;
    //    if (Block_I4_90) return true;
    //    if (Block_I5_90) return true;
    //    if (Block_O1) return true;
    //    if (Block_O2) return true;
    //    if (Block_O3) return true;
    //    if (Block_L2_0) return true;
    //    if (Block_L2_90) return true;
    //    if (Block_L2_270) return true;
    //    if (Block_L2_180) return true;
    //    if (Block_L3_0) return true;
    //    if (Block_L3_90) return true;
    //    if (Block_L3_180) return true;
    //    if (Block_L3_270) return true;
    //    return false;
    //}

    

    public static bool IsThereAValidMove()
    {
        bool value = false;

        foreach (var obj in GamePieces.WhatGamePieces())
        {
            var b = obj.name.Replace("(Clone)", ""); ;
            var r = obj.transform.localEulerAngles.z;
            value = Test(b, r);

            // Uncomment to see if avaliable move is valid to play
            //Debug.Log(b + " " + r + " " + value);
            if (value == true)
                break;
        }
        //Debug.Log("");

        return value;
    }

    public static void AnalyzeGrid()
    {
        string s = "";
        for (int y = 0; y < GameGrid.height; y++) // Going up each row
        {
            s += "\n";
            for (int x = 0; x < GameGrid.width; x++) // Going across each row
            {
                if (GameGrid.grid[x, y] == null)
                {
                    s += " O ";
                } else
                {
                    s += " X ";
                }

                CheckNeighbors(x, y);
            }
            //s += "\n";
        }
        //Debug.Log(s);
        //Debug.Log("Block_O3 " + DetectPossibleMoves.Block_O3);

    }

    // Check Neighbors
    private static void CheckNeighbors(int x, int y)
    {
        // bxy blocks

        // Block_O1 block
        bool b00 = true;

        // i2 -> i5 block
        bool b01 = true;
        bool b02 = true;
        bool b03 = true;
        bool b04 = true;

        bool b10 = true;
        bool b20 = true;
        bool b30 = true;
        bool b40 = true;

        // Block_O2 block
        bool b11 = true;

        // Block_O3 block
        bool b12 = true;
        bool b21 = true;
        bool b22 = true;

        // Check for Block_O1
        if (GameGrid.grid[x, y] != null)
        {
            //Debug.Log("Piece Here @ " + x + " " + y);
        }
        else
        {
            b00 = false;
        }

        for (int i = 1; i < 5; i++)
        {
            // Boundary checking
            int dx = x + i;
            int dy = y + i;

            bool insideX = true;
            bool insideY = true;

            if (dx >= GameGrid.width)
                insideX = false;
            if (dy >= GameGrid.height)
                insideY = false;

            //Debug.Log(dx + " " + y + " " + insideX);

            // Check neighbors of  i2, i3, i4, i5
            // l2, l3
            if (insideX)
            {
                //Debug.Log(x + " " + y);
                if (GameGrid.grid[dx, y] != null)
                {

                }
                else
                {
                    if (i == 1)
                        b10 = false;
                    if (i == 2)
                        b20 = false;
                    if (i == 3)
                        b30 = false;
                    if (i == 4)
                        b40 = false;
                }
            }


            // Check up
            if (insideY)
            {
                if (GameGrid.grid[x, dy] != null)
                {

                }
                else
                {
                    if (i == 1)
                        b01 = false;
                    if (i == 2)
                        b02 = false;
                    if (i == 3)
                        b03 = false;
                    if (i == 4)
                        b04 = false;
                }
            }
            // Check Block_O2 [ other conditions were checked already ]
            if (i == 1 && (insideX && insideY))
            {
                // Top right block 
                if (GameGrid.grid[dx, dy] != null)
                {
                }
                else
                {
                    b11 = false;
                }
            }

            //// Check Block_O3 [ other conditions were checked up to here ]
            if (i == 2 && (insideX && insideY))
            {
                // Top row, 2 block 
                if (GameGrid.grid[dx - 1, dy] != null)
                {

                } else
                {
                    b12 = false;
                }

                // Top right block
                if (GameGrid.grid[dx, dy] != null)
                {

                } else
                {
                    b22 = false;
                }

                // Second row, furthest right block
                if (GameGrid.grid[dx, dy - 1] != null)
                {

                } else
                {
                    b21 = false;
                }
            }
        }

        // Determine which blocks can be played

        // I moves up
        if (Block_I2_0 == false)
            Block_I2_0 = CheckI2_0(x, y, b00, b01, b02, b03, b04, b10, b11, b12, b20, b21, b22, b30, b40);

        if (Block_I3_0 == false)
            Block_I3_0 = CheckI3_0(x, y, b00, b01, b02, b03, b04, b10, b11, b12, b20, b21, b22, b30, b40);

        if (Block_I4_0 == false)
            Block_I4_0 = CheckI4_0(x, y, b00, b01, b02, b03, b04, b10, b11, b12, b20, b21, b22, b30, b40);

        if (Block_I5_0 == false)
            Block_I5_0 = CheckI5_0(x, y, b00, b01, b02, b03, b04, b10, b11, b12, b20, b21, b22, b30, b40);

        // I moves right

        if (Block_I2_90 == false)
            Block_I2_90 = CheckI2_90(x, y, b00, b01, b02, b03, b04, b10, b11, b12, b20, b21, b22, b30, b40);

        if (Block_I3_90 == false)
            Block_I3_90 = CheckI3_90(x, y, b00, b01, b02, b03, b04, b10, b11, b12, b20, b21, b22, b30, b40);

        if (Block_I4_90 == false)
            Block_I4_90 = CheckI4_90(x, y, b00, b01, b02, b03, b04, b10, b11, b12, b20, b21, b22, b30, b40);

        if (Block_I5_90 == false)
            Block_I5_90 = CheckI5_90(x, y, b00, b01, b02, b03, b04, b10, b11, b12, b20, b21, b22, b30, b40);

        // O moves
        if (Block_O1 == false)
            Block_O1 = CheckO1(x, y, b00, b01, b02, b03, b04, b10, b11, b12, b20, b21, b22, b30, b40);

        if (Block_O2 == false)
            Block_O2 = CheckO2(x, y, b00, b01, b02, b03, b04, b10, b11, b12, b20, b21, b22, b30, b40);

        if (Block_O3 == false)
            Block_O3 = CheckO3(x, y, b00, b01, b02, b03, b04, b10, b11, b12, b20, b21, b22, b30, b40);

        // L2 Moves

        //
        //    |__   0
        //  
        if (Block_L2_0 == false)
            Block_L2_0 = CheckL2_0(x, y, b00, b01, b02, b03, b04, b10, b11, b12, b20, b21, b22, b30, b40);



        //
        // __|      90
        //        
        if (Block_L2_90 == false)
            Block_L2_90 = CheckL2_90(x, y, b00, b01, b02, b03, b04, b10, b11, b12, b20, b21, b22, b30, b40);


        //
        // __        180
        //   |
        if (Block_L2_180 == false)
            Block_L2_180 = CheckL2_180(x, y, b00, b01, b02, b03, b04, b10, b11, b12, b20, b21, b22, b30, b40);


        //     __
        //    |      270
        //
        if (Block_L2_270 == false)
            Block_L2_270 = CheckL2_270(x, y, b00, b01, b02, b03, b04, b10, b11, b12, b20, b21, b22, b30, b40);


        // L3 Moves
        if (Block_L3_0 == false)
            Block_L3_0 = CheckL3_0(x, y, b00, b01, b02, b03, b04, b10, b11, b12, b20, b21, b22, b30, b40);

        if (Block_L3_90 == false)
            Block_L3_90 = CheckL3_90(x, y, b00, b01, b02, b03, b04, b10, b11, b12, b20, b21, b22, b30, b40);

        if (Block_L3_180 == false)
            Block_L3_180 = CheckL3_180(x, y, b00, b01, b02, b03, b04, b10, b11, b12, b20, b21, b22, b30, b40);

        if (Block_L3_270 == false)
            Block_L3_270 = CheckL3_270(x, y, b00, b01, b02, b03, b04, b10, b11, b12, b20, b21, b22, b30, b40);

        //if (Block_O3 == true)
        //    Debug.Log("O3: " + x + " " + y);
        //if (Block_I4_0 == true)
        //    Debug.Log("I4_0: " + x + " " + y);
        //if (Block_I5_0 == true)
        //    Debug.Log("I5_0: " + x + " " + y);

        //if (i4_180 == true)
        //    Debug.Log("i4_180: " + x + " " + sy);

        //if (Block_L2_180 == true)
        //    Debug.Log("Block_L2_180: " + x + " " + y);

        //Debug.Log("B00: " + b00 + " | x: " + x + " y: " + y);
        //Debug.Log("B01: " + b01 + " | x: " + x + " y: " + y);
        //Debug.Log("B02: " + b02 + " | x: " + x + " y: " + y);
        //Debug.Log("B10: " + b10 + " | x: " + x + " y: " + y);
        //Debug.Log("B11: " + b11 + " | x: " + x + " y: " + y);
        //Debug.Log("B12: " + b12 + " | x: " + x + " y: " + y);
        //Debug.Log("B20: " + b20 + " | x: " + x + " y: " + y);
        //Debug.Log("B21: " + b21 + " | x: " + x + " y: " + y);
        //Debug.Log("B22: " + b22 + " | x: " + x + " y: " + y);

    }


    private static bool CheckI2_0(int x, int y, bool b00, bool b01, bool b02, bool b03, bool b04, bool b10, bool b11, bool b12, bool b20, bool b21, bool b22, bool b30, bool b40)
    {
        if (b00) return false;
        if (b01) return false;
        if (y >= GameGrid.height - 1) return false;
        return true;
    }

    private static bool CheckI3_0(int x, int y, bool b00, bool b01, bool b02, bool b03, bool b04, bool b10, bool b11, bool b12, bool b20, bool b21, bool b22, bool b30, bool b40)
    {
        if (b00) return false;
        if (b01) return false;
        if (b02) return false;
        if (y >= GameGrid.height - 2) return false;
        return true;
    }

    private static bool CheckI4_0(int x, int y, bool b00, bool b01, bool b02, bool b03, bool b04, bool b10, bool b11, bool b12, bool b20, bool b21, bool b22, bool b30, bool b40)
    {
        if (b00) return false;
        if (b01) return false;
        if (b02) return false;
        if (b03) return false;
        if (y >= GameGrid.height - 3) return false;
        return true;
    }

    private static bool CheckI5_0(int x, int y, bool b00, bool b01, bool b02, bool b03, bool b04, bool b10, bool b11, bool b12, bool b20, bool b21, bool b22, bool b30, bool b40)
    {
        if (b00) return false;
        if (b01) return false;
        if (b02) return false;
        if (b03) return false;
        if (b04) return false;
        if (y >= GameGrid.height - 4) return false;
        return true;
    }

    private static bool CheckI2_90(int x, int y, bool b00, bool b01, bool b02, bool b03, bool b04, bool b10, bool b11, bool b12, bool b20, bool b21, bool b22, bool b30, bool b40)
    {
        if (b00) return false;
        if (b10) return false;
        if (x >= GameGrid.width - 1) return false;
        return true;
    }

    private static bool CheckI3_90(int x, int y, bool b00, bool b01, bool b02, bool b03, bool b04, bool b10, bool b11, bool b12, bool b20, bool b21, bool b22, bool b30, bool b40)
    {
        if (b00) return false;
        if (b10) return false;
        if (b20) return false;
        if (x >= GameGrid.width - 2) return false;
        return true;
    }

    private static bool CheckI4_90(int x, int y, bool b00, bool b01, bool b02, bool b03, bool b04, bool b10, bool b11, bool b12, bool b20, bool b21, bool b22, bool b30, bool b40)
    {
        if (b00) return false;
        if (b10) return false;
        if (b20) return false;
        if (b30) return false;
        if (x >= GameGrid.width - 3) return false;
        return true;
    }

    private static bool CheckI5_90(int x, int y, bool b00, bool b01, bool b02, bool b03, bool b04, bool b10, bool b11, bool b12, bool b20, bool b21, bool b22, bool b30, bool b40)
    {
        if (b00) return false;
        if (b10) return false;
        if (b20) return false;
        if (b30) return false;
        if (b40) return false;
        if (x >= GameGrid.width - 4) return false;
        return true;
    }

    private static bool CheckO1(int x, int y, bool b00, bool b01, bool b02, bool b03, bool b04, bool b10, bool b11, bool b12, bool b20, bool b21, bool b22, bool b30, bool b40)
    {
        if (b00) return false;
        return true;
    }

    private static bool CheckO2(int x, int y, bool b00, bool b01, bool b02, bool b03, bool b04, bool b10, bool b11, bool b12, bool b20, bool b21, bool b22, bool b30, bool b40)
    {
        if (b00) return false;
        if (b01) return false;
        if (b10) return false;
        if (b11) return false;
        if (x >= GameGrid.width - 1) return false;
        if (y >= GameGrid.height - 1) return false;
        return true;
    }

    private static bool CheckO3(int x, int y, bool b00, bool b01, bool b02, bool b03, bool b04, bool b10, bool b11, bool b12, bool b20, bool b21, bool b22, bool b30, bool b40)
    {
        if (b00) return false;
        if (b01) return false;
        if (b02) return false;
        if (b10) return false;
        if (b11) return false;
        if (b12) return false;
        if (b20) return false;
        if (b21) return false;
        if (b22) return false;
        if (x >= GameGrid.width - 2) return false;
        if (y >= GameGrid.height - 2) return false;
        return true;
    }

    private static bool CheckL2_0(int x, int y, bool b00, bool b01, bool b02, bool b03, bool b04, bool b10, bool b11, bool b12, bool b20, bool b21, bool b22, bool b30, bool b40)
    {
        if (b00) return false;
        if (b10) return false;
        if (b01) return false;
        if (x >= GameGrid.width - 1) return false;
        if (y >= GameGrid.height - 1) return false;
        return true;
    }

    private static bool CheckL2_90(int x, int y, bool b00, bool b01, bool b02, bool b03, bool b04, bool b10, bool b11, bool b12, bool b20, bool b21, bool b22, bool b30, bool b40)
    {
        if (b00) return false;
        if (b10) return false;
        if (b11) return false;
        if (y >= GameGrid.height - 1) return false;
        return true;
    }

    private static bool CheckL2_180(int x, int y, bool b00, bool b01, bool b02, bool b03, bool b04, bool b10, bool b11, bool b12, bool b20, bool b21, bool b22, bool b30, bool b40)
    {
        if (b01) return false;
        if (b11) return false;
        if (b10) return false;
        //if (y == 0) return false;
        return true;
    }

    private static bool CheckL2_270(int x, int y, bool b00, bool b01, bool b02, bool b03, bool b04, bool b10, bool b11, bool b12, bool b20, bool b21, bool b22, bool b30, bool b40)
    {
        if (b00) return false;
        if (b01) return false;
        if (b11) return false;
        if (x >= GameGrid.width - 1) return false;
        return true;
    }

    private static bool CheckL3_0(int x, int y, bool b00, bool b01, bool b02, bool b03, bool b04, bool b10, bool b11, bool b12, bool b20, bool b21, bool b22, bool b30, bool b40)
    {
        if (b00) return false;
        if (b10) return false;
        if (b20) return false;
        if (b01) return false;
        if (b02) return false;
        if (x >= GameGrid.width - 1) return false;
        if (y >= GameGrid.height - 1) return false;
        return true;
    }

    private static bool CheckL3_90(int x, int y, bool b00, bool b01, bool b02, bool b03, bool b04, bool b10, bool b11, bool b12, bool b20, bool b21, bool b22, bool b30, bool b40)
    {
        if (b00) return false;
        if (b10) return false;
        if (b20) return false;
        if (b21) return false;
        if (b22) return false;
        if (y >= GameGrid.height - 1) return false;
        return true;
    }

    private static bool CheckL3_180(int x, int y, bool b00, bool b01, bool b02, bool b03, bool b04, bool b10, bool b11, bool b12, bool b20, bool b21, bool b22, bool b30, bool b40)
    {
        if (b02) return false;
        if (b12) return false;
        if (b22) return false;
        if (b21) return false;
        if (b20) return false;
        //if (x <= 1) return false;
        return true;
    }

    private static bool CheckL3_270(int x, int y, bool b00, bool b01, bool b02, bool b03, bool b04, bool b10, bool b11, bool b12, bool b20, bool b21, bool b22, bool b30, bool b40)
    {
        if (b00) return false;
        if (b01) return false;
        if (b02) return false;
        if (b12) return false;
        if (b22) return false;
        if (x >= GameGrid.width - 1) return false;
        return true;
    }

    private static bool Test(string name, float i)
    {
        if (name.Equals("Block_I2"))
        {
            switch (i)
            {
                case 0:
                    if (Block_I2_0 == false)
                        return false;
                    break;
                case 90:
                    if (Block_I2_90 == false)
                        return false;
                    break;
                case 180:
                    if (Block_I2_0 == false)
                        return false;
                    break;
                case 270:
                    if (Block_I2_90 == false)
                        return false;
                    break;
                default:
                    break;
            }
        }
        else if (name.Equals("Block_I3"))
        {
            switch (i)
            {
                case 0:
                    if (Block_I3_0 == false)
                        return false;
                    break;
                case 90:
                    if (Block_I3_90 == false)
                        return false;
                    break;
                case 180:
                    if (Block_I3_0 == false)
                        return false;
                    break;
                case 270:
                    if (Block_I3_90 == false)
                        return false;
                    break;
                default:
                    break;
            }
        }
        else if (name.Equals("Block_I4"))
        {
            switch (i)
            {
                case 0:
                    if (Block_I4_0 == false)
                        return false;
                    break;
                case 90:
                    if (Block_I4_90 == false)
                        return false;
                    break;
                case 180:
                    if (Block_I4_0 == false)
                        return false;
                    break;
                case 270:
                    if (Block_I4_90 == false)
                        return false;
                    break;
                default:
                    break;
            }
        }
        else if (name.Equals("Block_I5"))
        {
            switch (i)
            {
                case 0:
                    if (Block_I5_0 == false)
                        return false;
                    break;
                case 90:
                    if (Block_I5_90 == false)
                        return false;
                    break;
                case 180:
                    if (Block_I5_0 == false)
                        return false;
                    break;
                case 270:
                    if (Block_I5_90 == false)
                        return false;
                    break;
                default:
                    break;
            }
        }
        else if (name.Equals("Block_L2"))
        {
            switch (i)
            {
                case 0:
                    if (Block_L2_0 == false)
                        return false;
                    break;
                case 90:
                    if (Block_L2_90 == false)
                        return false;
                    break;
                case 180:
                    if (Block_L2_180 == false)
                        return false;
                    break;
                case 270:
                    if (Block_L2_270 == false)
                        return false;
                    break;
                default:
                    break;
            }
        }
        else if (name.Equals("Block_L3"))
        {
            switch (i)
            {
                case 0:
                    if (Block_L3_0 == false)
                        return false;
                    break;
                case 90:
                    if (Block_L3_90 == false)
                        return false;
                    break;
                case 180:
                    if (Block_L3_180 == false)
                        return false;
                    break;
                case 270:
                    if (Block_L3_270 == false)
                        return false;
                    break;
                default:
                    break;
            }
        }
        else if (name.Equals("Block_O1"))
        {
            if (Block_O1 == false)
                return false;
        }
        else if (name.Equals("Block_O2"))
        {
            if (Block_O2 == false)
                return false;
        }
        else if (name.Equals("Block_O3"))
        {
            if (Block_O3 == false)
                return false;
        }

        return true;
    }
}
