using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    public int a = 0;

    // Start is called before the first frame update
    void Start()
    {
        //XmlSerializer serializer = new XmlSerializer(typeof(Test));
        //TextWriter writer = new StreamWriter("saveFile.xml");
        //serializer.Serialize(writer, this);
        //writer.Close();

        SaveData save = new SaveData();
        //Info i = new Info();
        //save.Save(i);
        //Info i = save.Load();
        //Debug.Log(i.a);
    }


}

[Serializable]
public class Info
{
    public int a = 0;
}