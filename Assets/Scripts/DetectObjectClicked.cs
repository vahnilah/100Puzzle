using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObjectClicked : MonoBehaviour
{

    public bool IsObjectClicked { get; private set; }

    public Vector3 PositionOfObject { get; private set; }

    // Create a unique identification number for every game object block to retain position
    public string UUID { get; private set; }

    public void Start()
    {
        UUID = System.Guid.NewGuid().ToString();
    }

    void OnMouseUp()
    {
        IsObjectClicked = false;
    }

    void OnMouseDown()
    {
        IsObjectClicked = true;
        PositionOfObject = this.transform.position;
    }

}