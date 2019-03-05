using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    private bool getRefresh = false;
    private bool isMouseReleased = false;
    private bool checkedAvaliablePieces = false;
    private bool triggerOnce = true;

    public ClickAndDrop clickAndDropScript;

    private GameObject gameOverCanvas;
    private GameObject restartButton;
    private GameObject changeColorButton;
    private GameObject highScoreIcon;

    private GameObject background;

    private void Awake()
    {
        SetColor.LoadColorTheme();
    }

    // Start is called before the first frame update
    void Start()
    {
        //WindowManager.instance.ScreenSizeChangeEvent += Instance_ScreenSizeChangeEvent;

        DetectPossibleMoves.SetBooleans();
        GamePieces.FindGameBlocks();
        GamePieces.FindGamePieces();

        clickAndDropScript = gameObject.GetComponent<ClickAndDrop>();
        gameOverCanvas = GameObject.FindGameObjectWithTag("GameOverCanvas");
        restartButton = GameObject.FindGameObjectWithTag("RestartButton");
        changeColorButton = GameObject.FindGameObjectWithTag("ChangeColor");
        highScoreIcon = GameObject.FindGameObjectWithTag("HighScoreIcon");

        background = GameObject.FindGameObjectWithTag("BlurBG");

        gameOverCanvas.SetActive(false);
        restartButton.SetActive(false);
        changeColorButton.SetActive(true);
        background.SetActive(false);
        highScoreIcon.SetActive(true);



        //GameOver();
    }

    private void Instance_ScreenSizeChangeEvent(int Width, int Height)
    {
        Debug.Log("Screen Size Width = " + Width.ToString() + "  Height = " + Height.ToString());
    }

    void GameOver()
    {
        gameOverCanvas.SetActive(true);
        restartButton.SetActive(true);
        changeColorButton.SetActive(false);
        background.SetActive(true);
        highScoreIcon.SetActive(false);


        GameObject[] objs = GameObject.FindGameObjectsWithTag("ScoreText");

        foreach (var i in objs)
        {
            var t = i.GetComponent<Text>();

            if (t.name.Equals("HighScoreMenu"))
            {
                if (GameOverInfo.totalScore < GameOverInfo.highScore)
                {
                    t.text = "BEST " + GameOverInfo.highScore.ToString();
                }
                else
                {
                    t.text = "NEW HIGH " + GameOverInfo.totalScore.ToString();

                }
            }
            if (t.name.Equals("CurrentScoreMenu"))
            {
                t.text = GameOverInfo.totalScore.ToString();
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Set the color of the grid
        if (AnimateGrid.setGrid == false)
        {
            AnimateGrid.SetGrid();
            AnimateGrid.setGrid = true;
        }

        // Change colors to Light / Dark mode
        if (SetColor.WillChangeColor == true)
        {
            // Change BG
            var camera = (Camera) FindObjectOfType(typeof(Camera));
            camera.backgroundColor = SetColor.GetColor("Background");

            // Change Grid
            AnimateGrid.SetGrid();

            // Change Icon Color on highscore
            SpriteRenderer HighScoreIconRender = highScoreIcon.GetComponent<SpriteRenderer>();
            if (SetColor.GetDarkMode() == true)
            {
                HighScoreIconRender.color = SetColor.GetColor("Font");
            }
            else
            {
                HighScoreIconRender.color = SetColor.GetColor("Font");
            }

            // Change ChangeIconImage Image
            Image changeColorButtonImage = changeColorButton.GetComponent<Image>();
            //Debug.Log(changeColorButtonImage);

            if (SetColor.GetDarkMode() == true)
            {
                changeColorButtonImage.sprite = Resources.Load<Sprite>("Images/GridIconAlt");

            }
            else
            {

                changeColorButtonImage.sprite = Resources.Load<Sprite>("Images/GridIconDark");


            }

            // Change Labels
            GameObject[] labels = GameObject.FindGameObjectsWithTag("TextLabel");
            foreach (var a in labels)
            {
                MeshRenderer bgBlock = a.GetComponent<MeshRenderer>();
                Material[] meshMats = bgBlock.materials;

                if (SetColor.GetDarkMode() == true)
                {
                    meshMats[0] = null;
                    meshMats[0] = SetColor.GetMaterial("Grid");
                }
                else
                {
                    meshMats[0] = null;
                    meshMats[0] = SetColor.GetMaterial("Grid");

                }

                bgBlock.materials = meshMats;
            }

            // Changel Text on Labels
            GameObject[] textLabels = GameObject.FindGameObjectsWithTag("ScoreText");
            // Set UI Text to objects to access
            //Debug.Log(objs.Length);
            foreach (var i in textLabels)
            {
                var t = i.GetComponent<Text>();
                if (t.name.Equals("HighScore"))
                {
                    if (SetColor.GetDarkMode() == true)
                    {
                        t.color = SetColor.GetColor("Font");
                    }
                    else
                    {
                        t.color = SetColor.GetColor("Font");
                    }

                }
                if (t.name.Equals("CurrentScore"))
                {
                    if (SetColor.GetDarkMode() == true)
                    {
                        t.color = SetColor.GetColor("Font");
                    }
                    else
                    {
                        t.color = SetColor.GetColor("Font");
                    }

                }
            }


            SetColor.WillChangeColor = false;
        }

        // Restart game
        if (GameOverInfo.RestartGame == true)
        {
            gameOverCanvas.SetActive(false);
            restartButton.SetActive(false);
            background.SetActive(false);
            GameOverInfo.RestartGame = false;
            AnimateGrid.setGrid = false;
            SetColor.WillChangeColor = true;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Check if Game Over
        if (DetectPossibleMoves.IsGameOver == true)
        {
            if (triggerOnce == true)
            {
                GameOver();

                //Debug.Log("Game Over");
                triggerOnce = false;
            }
        }
        else
        {
            if (!checkedAvaliablePieces || DetectPossibleMoves.DidNewBlocksSpawn)
            {

                DetectPossibleMoves.SetBooleans();

                DetectPossibleMoves.AnalyzeGrid();

                DetectPossibleMoves.IsThereAValidMove();

                checkedAvaliablePieces = true;
                GamePieces.HasAnGamePieceBeenPlayedToUpdateScore = true;

            }
            
            // Logic for the grid
            GamePieces.FindGameBlocks();
            GamePieces.FindGamePieces();
            GamePieces.WhereAreGamePiecesToPlay();
            AnimateGrid.FindBackgroundBlocks();

            /* If the left (main) mouse click is released
             * remove cleared blocks */
            if (Input.GetMouseButtonUp(0) || isMouseReleased)
            {

                if (GamePieces.isAPlayValid)
                    GameGrid.UpdatePlayedPiecesOnGrid(this);

                UpdateGameLogic();

                getRefresh = true;
                isMouseReleased = false;

                UpdateAnimation();
            }

            if (GamePieces.isAPlayValid)
            {
                if (GamePieces.isAPlayValid)
                    GameGrid.UpdatePlayedPiecesOnGrid(this);

                checkedAvaliablePieces = false;
            }

        }
    }

    void Debugging()
    {

        DetectPossibleMoves.AnalyzeGrid();

    }

    // All methods to update the game logic and effects
    void UpdateGameLogic()
    {
        //UpdatePlayedPiecesOnGrid();

        // Delete successful rows
        GameGrid.DeleteFilledRows();
        GameGrid.DeleteFilledCollumns();

        // Disable clicking after valid placement
        BoxCollider2D[] myColliders = gameObject.GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D bc in myColliders) bc.enabled = false;
    }

    void UpdateAnimation()
    {
        // If there is a row clear, del played blocks
        if (GameGrid.canRowsBeCleared)
        {
            // Delete played row blocks
            GameGrid.DeletePlayedBlocksOnRow();
        }

        // If there is a col clear, del played blocks
        if (GameGrid.canColsBeCleared)
        {
            GameGrid.DeletePlayedBlocksOnCollumns();
        }

        if (GameGrid.canRowsBeCleared || GameGrid.canColsBeCleared)
        {
            // Reset the bool variables
            GameGrid.canRowsBeCleared = false;
            GameGrid.canColsBeCleared = false;

            // Remove cleared pieces from board
            if (GamePieces.isAPlayValid)
                GameGrid.UpdatePlayedPiecesOnGrid(this);

            // Run the clear row/col animation
            AnimateGrid.AnimateBgGrid(this);

            // Add the amount of rows cleared for scoring, must be reset in score class
            GameGrid.howManyRowsColsWereFilled += GameGrid.whatRowIsFilled.Count;
            GameGrid.howManyRowsColsWereFilled += GameGrid.whatColIsFilled.Count;

            // Clear the HashSet's of stored clears
            GameGrid.whatRowIsFilled.Clear();
            GameGrid.whatColIsFilled.Clear();
            AnimateGrid.bg_rows.Clear();
            AnimateGrid.bg_cols.Clear();
        }
    }

    // Method to reset the number of stored locations on the bg array
    void ClearStoredBgArrays()
    {
        AnimateGrid.bg_rows.Clear();
        AnimateGrid.bg_cols.Clear();
    }

    /* Fixes bug:
     * to solve clearing blocks when mouse is released */
    void OnMouseUp()
    {
        isMouseReleased = true;
    }
}
