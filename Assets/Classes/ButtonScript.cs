using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var objName = this.transform.name.Replace("(Clone)", "");

        //Debug.Log(objName);

        if (objName.Equals("RestartButton"))
        {
            Button btn = GameObject.Find("RestartButton").GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => RestartGame());
        }

        if (objName.Equals("ChangeColor"))
        {
            Button btn = GameObject.Find("ChangeColor").GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => ChangeColors());
        }
    }

    public void RestartGame()
    {
        //Debug.Log("Restart");
        GameOverInfo.RestartGame = true;
    }

    public void ChangeColors()
    {
        //SetColor.DarkMode = !SetColor.DarkMode;
        SetColor.WillChangeColor = true;

        SetColor.ChangeColorTheme();
        //Debug.Log(SetColor.GetDarkMode());

        //Debug.Log(SetColor.DarkMode);
    }
}
