using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SetColor : MonoBehaviour
{
    //public static bool DarkMode = false;
    public static bool WillChangeColor = false;
    static SaveTheme theme = new SaveTheme();


    //private static readonly Color32 DarkBG = new Color32(21, 1, 34, 255);
    private static readonly Color32 DarkBG = new Color32(16, 16, 27, 255);
    private static readonly Color32 LightBG = new Color32(255, 255, 255, 255);

    //private static readonly Color32 DarkGrid = new Color32(21, 1, 34, 255);
    private static readonly Color32 DarkGrid = new Color32(16, 16, 27, 255);
    private static readonly Color32 LightGrid = new Color32(255, 255, 255, 255);

    private static readonly Color32 DarkFont = new Color32(80,80,80, 255);
    private static readonly Color32 LightFont = new Color32(175, 175, 175, 255);

    public static void LoadColorTheme()
    {
        SavedData data = new SavedData();
        try
        {
            SaveTheme savedData = (SaveTheme)data.Load();
            theme.SetDarkMode(savedData.darkMode);
            WillChangeColor = savedData.darkMode;
        }
        catch (NullReferenceException)
        {
            theme.SetDarkMode(false);
            WillChangeColor = false;
            data.Save(theme);
        }
    }

    public static void ChangeColorTheme()
    {
        theme.SetDarkMode(!theme.darkMode);
        SavedData data = new SavedData();
        data.Save(theme);
    }

    public static bool GetDarkMode()
    {
        return theme.darkMode;
    }

    public static Color32 GetColor(string colorName)
    {

        if (colorName.Equals("Background")) {
            if (GetDarkMode() == true)
            {
                return DarkBG;
            }
            else
            {
                return LightBG;
            }
        }
        else if (colorName.Equals("Grid"))
        {
            if (GetDarkMode() == true)
            {
                return DarkGrid;
            }
            else
            {
                return LightGrid;
            }
        }
        else if (colorName.Equals("Font"))
        {
            if (GetDarkMode() == true)
            {
                return LightFont;
            }
            else
            {
                return DarkFont;
            }
        }

        return new Color32(0, 0, 0, 0);
    }

    public static Material GetMaterial(string colorName)
    {
        Material Grey_Dark = null;
        Material Grey_Light = null;
        UnityEngine.Object[] background = Resources.LoadAll("Background", typeof(Material));
        foreach (var mat in background)
        {
            if (mat.name.Equals("DarkGrey"))
            {
                Grey_Dark = (Material) mat;
            }
            if (mat.name.Equals("LightGrey"))
            {
                Grey_Light = (Material)mat;
            }
        }

        if (colorName.Equals("Grid"))
        {
            if (GetDarkMode() == true)
            {
                return Grey_Dark;
            }
            else
            {
                return Grey_Light;
            }
        }

        return null;
    }

    [Serializable]
    private class SaveTheme
    {
        public bool darkMode { get; private set; }

        public void SetDarkMode(bool value)
        {
            darkMode = value;
        }
    }

    private class SavedData
    {
        readonly String Name = "Theme";

        public void Save(SaveTheme theClass)
        {

            ///Open or Create Save File
            //Debug.Log("Saving File to: " + Application.persistentDataPath + " ...");

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/SaveData" + Name + ".dat", FileMode.OpenOrCreate);

            //Create new SaveData. This will be everything that is saved.
            SaveTheme saveData = theClass;
            bf.Serialize(file, saveData);
            file.Close();
        }

        //Load the file into SaveData.
        public SaveTheme Load()
        {
            if (!File.Exists(Application.persistentDataPath + "/SaveData" + Name + ".dat"))
            {
                //Debug.Log("File Not Found! Load Failed.");
                return null;
            }
            BinaryFormatter bf = new BinaryFormatter(); //Serializer
            FileStream file = File.Open(Application.persistentDataPath + "/SaveData" + Name + ".dat", FileMode.Open); //Open File
            SaveTheme savedData = (SaveTheme)bf.Deserialize(file); //Load Data.
            file.Close(); //Close File.
            return savedData; //Return Saved Data.    
        }

    }
}
