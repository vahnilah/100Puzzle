using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateGrid : MonoBehaviour
{
    public static bool setGrid = false;
    public static bool runAnimationScript = false;
    public static float animationRate = 0.05f;
    public static float changeDeleteTime = 0.25f;
    public static float changeTranstionalTime = 0.035f;

    // Get the background grid
    public static GameObject[] bg_grid;

    // Obtain the visual background grid to animiate on

    public static HashSet<int> bg_rows = new HashSet<int>();
    public static HashSet<int> bg_cols = new HashSet<int>();

    public static void FindBackgroundBlocks()
    {
        AnimateGrid.bg_grid = GameObject.FindGameObjectsWithTag("background_block");
    }

    // Change the grid color for dark / light mode
    public static void SetGrid()
    {
        GameObject[] grid = GameObject.FindGameObjectsWithTag("background_block");


        for (int i = 0; i < grid.Length; i++)
        {
            MeshRenderer bgBlock = grid[i].GetComponent<MeshRenderer>();
            Material[] meshMats = bgBlock.materials;

            if (SetColor.GetDarkMode() == true)
            {
                meshMats[1] = null;
                meshMats[1] = SetColor.GetMaterial("Grid");
            }
            else
            {
                meshMats[1] = null;
                meshMats[1] = SetColor.GetMaterial("Grid");

            }

            bgBlock.materials = meshMats;
        }
    }

    // Change the color of the background grid on valid play.
    public static void ChangeBgGridColor(int x, int y)
    {
        Transform obj = GameGrid.grid[x, y];
        int i = GetIndexOfBgGrid(x, y);

        // Get the MeshRenderer of each gameblock within a gamepiece
        MeshRenderer bgBlock = bg_grid[i].GetComponent<MeshRenderer>();
        Material[] meshMats = bgBlock.materials;

        Material mat = obj.GetComponentInChildren<MeshRenderer>().material;
        // Set a random color to all the gameblocks
        //meshMats[0] = (Material)randomColor;
        meshMats[2] = null;
        meshMats[2] = mat;

        bgBlock.materials = meshMats;
    }

    // Animate row
    public static void PlayClearAnimationFromRow()
    {
        float delay = 0f;

        foreach (int y in GameGrid.whatRowIsFilled)
        {
            Debug.Log(y);
            for (int x = GameGrid.width - 1; x >= 0; x--)
            {
                if (SetColor.GetDarkMode() == true)
                {
                    GameGrid.grid[x, y].gameObject.GetComponentInChildren<Animator>().enabled = true;
                    GameGrid.grid[x, y].gameObject.GetComponentInChildren<Animator>().Play("BlockClearDark", 0, delay); // _name, _layer, _delay
                    delay += .1f;
                }
                else
                {
                    GameGrid.grid[x, y].gameObject.GetComponentInChildren<Animator>().enabled = true;
                    GameGrid.grid[x, y].gameObject.GetComponentInChildren<Animator>().Play("BlockClearLight", 0, delay); // _name, _layer, _delay
                    delay += .1f;
                }
            }
        }

        runAnimationScript = false;
    }

    // Animate the BgGrid after the blocks are clear to give an illusion
    // the blocks are still there and animating away.
    public static void AnimateBgGrid(MonoBehaviour mono)
    {

        bool[] row = new bool[GameGrid.width];
        bool[] col = new bool[GameGrid.height];

        foreach (int rowLoc in GameGrid.whatRowIsFilled)
        {
            row[rowLoc] = true;
            for (int a = 0; a < 10; a++)
            {
                bg_rows.Add(GetIndexOfBgGrid(a, rowLoc));
            }
        }

        foreach (int colLoc in GameGrid.whatColIsFilled)
        {
            col[colLoc] = true;
            for (int a = 0; a < 10; a++)
                bg_cols.Add(GetIndexOfBgGrid(colLoc, a));
        }

        // Decide the order of what row/col will play 
        // To help better animate multiple clears
        for (int i = 0; i < GameGrid.width; i++)
        {
            if (row[i] && col[i])
            {
                RunBgRow(mono);
                RunBgCol(mono);
            } else if (row[i] && !col[i])
            {
                RunBgRow(mono);
            } else if (!row[i] && col[i])
            {
                RunBgCol(mono);
            }
        }
    }

    private static void RunBgRow(MonoBehaviour mono)
    {
        float wait = 0.0f;
        int reset = 0;
        foreach (int a in bg_rows)
        {
            // Run animation
            float loc = -PlayAtLocation(a % 10);
            if (SetColor.GetDarkMode() == true)
            {
                bg_grid[a].GetComponent<Animator>().Play("BlockClearDark", 0, loc);
            }
            else
            {
                bg_grid[a].GetComponent<Animator>().Play("BlockClearLight", 0, loc);

            }
            // Delete the changed block on the MeshRender.materal[1] on the bg
            MeshRenderer bgBlock = bg_grid[a].GetComponent<MeshRenderer>();
            mono.StartCoroutine(DeleteColorChange(bgBlock, wait));
            wait += changeTranstionalTime;

            // Reset the time every row change for multiple clears in one play
            if (++reset % 10 == 0)
                wait = 0.0f;
        }
    }
    private static void RunBgCol(MonoBehaviour mono)
    {
        float wait = 0.0f;
        int reset = 0;
        foreach (int a in bg_cols)
        {
            // Run animation
            float loc = -PlayAtLocation(a / 10);
            //if (a % 10 == rowLoc) loc += PlayAtLocation(a / 10);
            if (SetColor.GetDarkMode() == true)
            {
                bg_grid[a].GetComponent<Animator>().Play("BlockClearDark", 0, loc);
            } else
            {
                bg_grid[a].GetComponent<Animator>().Play("BlockClearLight", 0, loc);

            }
            // Delete the changed block on the MeshRender.materal[1] on the bg 
            MeshRenderer bgBlock = bg_grid[a].GetComponent<MeshRenderer>();
            mono.StartCoroutine(DeleteColorChange(bgBlock, wait));
            wait += changeTranstionalTime;

            // Reset the time every col change for multiple clears in one play
            if (++reset % 10 == 0)
                wait = 0.0f;
        }
    }

    static IEnumerator DeleteColorChange(MeshRenderer bgBlock, float value)
    {
        // Wait time
        yield return new WaitForSeconds(changeDeleteTime + value);

        // Remove the material change on the valid play
        Material[] meshMats = bgBlock.materials;
        //meshMats[0] = (Material)randomColor;
        meshMats[2] = null;

        bgBlock.materials = meshMats;
    }

    // Reliable method to get the delay of the animation based on location.
    public static float PlayAtLocation(int loc)
    {
        //switch (loc)
        //{
        //    case 0:
        //        return 0;
        //    case 1:
        //        return 0.05f;
        //    case 2:
        //        return 0.1f;
        //    case 3:
        //        return 0.15f;
        //    case 4:
        //        return 0.2f;
        //    case 5:
        //        return 0.25f;
        //    case 6:
        //        return 0.3f;
        //    case 7:
        //        return 0.35f;
        //    case 8:
        //        return 0.4f;
        //    case 9:
        //        return 0.45f;
        //    default:
        //        return 0f;
        //}
        return animationRate * loc;
    }


    public static void DeletedBgGridOnRow()
    {
        foreach (int y in GameGrid.whatRowIsFilled) {
            for (int x = 0; x < GameGrid.width; x++)
            {
                bg_rows.Add(GetIndexOfBgGrid(x, y));

            }
        }
    }

    // Disable the bg grid
    public static void DisableBgGrid()
    {
        foreach (int i in bg_cols) bg_grid[i].SetActive(false);
        foreach (int i in bg_rows) bg_grid[i].SetActive(false);

    }

    // Enable the bg grid
    public static void EnableBgGrid()
    {
        foreach (int i in bg_cols) bg_grid[i].SetActive(true);
        foreach (int i in bg_rows) bg_grid[i].SetActive(true);
    }

    // Helper method to get the location of the bg grid
    public static int GetIndexOfBgGrid(int x, int y)
    {
        int i = 0;
        if (y == 0)
        {
            i = x;
        }
        else
        {
            i = y * 10;
            i += x;
        }
        //Debug.Log("[i] = " + i + " (" + x + ", " + y + ")");
        return i;
    }
}
