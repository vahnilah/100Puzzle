using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveData
{
    readonly String Name = "SavedData";

    public void Save(Info theClass)
    {

        ///Open or Create Save File
        Debug.Log("Saving File to: " + Application.persistentDataPath + " ...");

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/SaveData" + Name + ".dat", FileMode.OpenOrCreate);

        //Create new SaveData. This will be everything that is saved.
        Info saveData = theClass;
        bf.Serialize(file, saveData);
        file.Close();
    }

    //Load the file into SaveData.
    public Info Load()
    {
        if (!File.Exists(Application.persistentDataPath + "/SaveData" + Name + ".dat"))
        {
            Debug.Log("File Not Found! Load Failed.");
            return null;
        }
        BinaryFormatter bf = new BinaryFormatter(); //Serializer
        FileStream file = File.Open(Application.persistentDataPath + "/SaveData" + Name + ".dat", FileMode.Open); //Open File
        Info savedData = (Info)bf.Deserialize(file); //Load Data.
        file.Close(); //Close File.
        return savedData; //Return Saved Data.    
    }

}