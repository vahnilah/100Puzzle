using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    private readonly int ScoreBlock = 1;

    // Score when a block is added
    public int PlayedScore { get; private set; }

    // Score with the block added & rows cleared [ for animation ]

    public int TotalScore { get; private set; }

    public int HighScore { get; private set; }

    private int previousCount;
    private int previous1;

    public int outsidePieces { get; private set; }

    SaveHighScore SaveData = new SaveHighScore();

    private int previousBoardCount;
    private int currentBoardCount;

    private bool saved = false;
    private bool isThereValidMoves = true;

    //private Text[] texts;
    Text highScoreText;
    Text totalScoreText;
    private int previous3;
    private int previous2;
    private int blockPlayed;

    private void Awake()
    {
        SaveData = new SaveHighScore();
        try
        {
            SavedData data = new SavedData();
            SaveHighScore oldScore = (SaveHighScore)data.Load();
            HighScore = oldScore.highScore;
            data = null;
        }
        catch (NullReferenceException e)
        {
            HighScore = 0;
            SaveData.SetHighScore(HighScore, HighScore);
            SavedData data = new SavedData();
            data.Save(SaveData);
        }

        //Debug.Log("High Score: " + HighScore);
    }

    // Start is called before the first frame update
    void Start()
    {
    
        PlayedScore = 0;
        TotalScore = 0;
        previousBoardCount = 0;
        currentBoardCount = 0;
        //PrintScore();

        GameObject[] objs = GameObject.FindGameObjectsWithTag("ScoreText");
        // Set UI Text to objects to access
        //Debug.Log(objs.Length);
        foreach (var i in objs)
        {
            var t = i.GetComponent<Text>();
            if (t.name.Equals("HighScore"))
            {
                highScoreText = t;
            }
            if (t.name.Equals("CurrentScore"))
            {
                totalScoreText = t;
            }
        }

        // Set labels
        highScoreText.text = HighScore.ToString();
        GameOverInfo.highScore = HighScore;
        totalScoreText.text = TotalScore.ToString();


    }

    // Update is called once per frame
    void Update()
    {
        if (DetectPossibleMoves.IsGameOver == true)
        {
            // Condenced game update score below for game over text w/o animation
            totalScoreText.text = PlayedScore.ToString();
            int diff = TotalScore - PlayedScore;
            int loops = 10;
            int valueToIncrement = diff / loops;
            int temp = PlayedScore + valueToIncrement;
            totalScoreText.text = temp.ToString();
            GameOverInfo.totalScore = temp;

            TotalScore = temp;
        }
        else
        {
            if (GamePieces.CountGameBlocksOutsideGrid() != outsidePieces)
            {
                previous1 = outsidePieces;
                outsidePieces = GamePieces.CountGameBlocksOutsideGrid();
            }

            if (GamePieces.HasAnGamePieceBeenPlayedToUpdateScore == true)
            {
                isThereValidMoves = DetectPossibleMoves.IsThereAValidMove();

                GamePieces.HasAnGamePieceBeenPlayedToUpdateScore = false;

                previous3 = previous2;
                previous2 = outsidePieces;
                if (previous3 != 0)
                {
                    blockPlayed = Math.Abs(previous2 - previous3);
                    //Debug.Log(blockPlayed);

                    // Adjust score
                    int max = Math.Max(PlayedScore, TotalScore);
                    PlayedScore = max;
                    TotalScore = max;

                    // Update scores on played & total
                    PlayedScore += blockPlayed;
                    TotalScore += blockPlayed;

                    // If rows were cleared, add them to total score & played score when complete
                    int clear = GameGrid.howManyRowsColsWereFilled;
                    if (clear > 0)
                        GetRowsClears(GameGrid.howManyRowsColsWereFilled);
                    GameGrid.howManyRowsColsWereFilled = 0;

                    //PrintScore();
                    UpdateScore();
                }

                //Debug.Log(previous1 + " , " + previous2 + " , " + previous3);


                //int blocksPlayed = 

            }

            if (!isThereValidMoves && !saved)
            {
                if (TotalScore > HighScore)
                {
                    SaveData.SetHighScore(TotalScore, HighScore);
                    SavedData data = new SavedData();
                    data.Save(SaveData);
                }

                //Debug.Log("Game Over");
                DetectPossibleMoves.IsGameOver = true;

                saved = true;
            }
        }
    }

    private void UpdateScore()
    {
        // Init score play update

        totalScoreText.text = PlayedScore.ToString();
        GameOverInfo.totalScore = PlayedScore;


        // 10 updates to get total score

        int diff = TotalScore - PlayedScore;
        //TotalScore = PlayedScore;

        // Run total score animation if row is cleared
        if (diff > 0)
        {
            StartCoroutine(Delay(diff).GetEnumerator());
        }
    }

    // Delay animation score for a nice pause between block being played
    IEnumerable Delay(int diff)
    {
        yield return new WaitForSeconds(.1f);

        int loops = 10;
        int valueToIncrement = diff / loops;
        int value = 0;
        for (int i = 0; i < loops; i++)
        {
            value += valueToIncrement;
            //StartCoroutine("AnimateScore");
            //StartCoroutine(AnimateScore());
            StartCoroutine(AnimateScore(i, value).GetEnumerator());

        }
    }

    IEnumerable AnimateScore(int i, int value)
    {
        //Debug.Log("Hi");
        //yield return null;
        yield return new WaitForSeconds(i/17f);

        //Debug.Log("Hi");

        int temp = PlayedScore + value;
        totalScoreText.text = temp.ToString();
        GameOverInfo.totalScore = temp;

        TotalScore = temp;
    }

    private void PrintScore()
    {
        Debug.Log("OnPlay: " + PlayedScore);
        Debug.Log("Total: " + TotalScore);
    }

    // (OnBoard + Missing) - Previous Board Count = Played Block Count
    private void GetPlayedCount(int onBoard)
    {
        PlayedScore += onBoard;
        TotalScore += onBoard;
        //previousBoardCount = onBoard;
    }

    // Missing blocks rounded up to 10th number = Rows Cleared
    private void GetRowsClears(int cleared)
    {
        //int rowsCleared = (int)Mathf.Ceil(missing / 10) * 10;
        //Debug.Log(rowsCleared);
        TotalScore += (cleared * 10);
    }


    [Serializable]
    private class SaveHighScore
    {
        public int highScore { get; private set; }

        public void SetHighScore(int score, int oldHighScore)
        {
            highScore = Math.Max(score, oldHighScore);
        }
    }

    private class SavedData
    {
        readonly String Name = "Score";

        public void Save(SaveHighScore theClass)
        {

            ///Open or Create Save File
            //Debug.Log("Saving File to: " + Application.persistentDataPath + " ...");

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/SaveData" + Name + ".dat", FileMode.OpenOrCreate);

            //Create new SaveData. This will be everything that is saved.
            SaveHighScore saveData = theClass;
            bf.Serialize(file, saveData);
            file.Close();
        }

        //Load the file into SaveData.
        public SaveHighScore Load()
        {
            if (!File.Exists(Application.persistentDataPath + "/SaveData" + Name + ".dat"))
            {
                //Debug.Log("File Not Found! Load Failed.");
                return null;
            }
            BinaryFormatter bf = new BinaryFormatter(); //Serializer
            FileStream file = File.Open(Application.persistentDataPath + "/SaveData" + Name + ".dat", FileMode.Open); //Open File
            SaveHighScore savedData = (SaveHighScore) bf.Deserialize(file); //Load Data.
            file.Close(); //Close File.
            return savedData; //Return Saved Data.    
        }

    }

}





