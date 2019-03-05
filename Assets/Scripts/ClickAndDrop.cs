using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// http://unity.grogansoft.com/drag-and-drop/

public class ClickAndDrop : MonoBehaviour
{
    private bool draggingItem = false;
    private GameObject draggedObject;
    private Vector2 touchOffset;

    void Update()
    {
        if (HasInput)
        {
            DragOrPickUp();
        }
        else
        {
            if (draggingItem)
                DropItem();
        }
    }

    Vector2 CurrentTouchPosition
    {
        get
        {
            Vector2 inputPos;
            inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return inputPos;
        }
    }

    private void DragOrPickUp()
    {
        var inputPosition = CurrentTouchPosition;

        if (draggingItem)
        {
            draggedObject.transform.position = inputPosition + touchOffset;

            // Set the dragged object to be above everything else while dragging
            var changeLayer = draggedObject.transform.position;
            changeLayer.z = -9;
            draggedObject.transform.position = changeLayer;
        }
        else
        {
            RaycastHit2D[] touches = Physics2D.RaycastAll(inputPosition, inputPosition, 0.5f);
            if (touches.Length > 0)
            {
                var hit = touches[0];
                if (hit.transform != null)
                {
                    draggingItem = true;
                    draggedObject = hit.transform.gameObject;
                    touchOffset = (Vector2)hit.transform.position - inputPosition;
                    draggedObject.transform.localScale = new Vector3(1f, 1f, 1f);

                    // Set the dragged object to be above everything else while dragging
                    var changeLayer = draggedObject.transform.position;
                    changeLayer.z = -9;
                    draggedObject.transform.position = changeLayer;
                }
            }
        }
    }

    private bool HasInput
    {
        get
        {
            // returns true if either the mouse button is down or at least one touch is felt on the screen
            return Input.GetMouseButton(0);
        }
    }

    void DropItem()
    {
        draggingItem = false;
        //draggedObject.transform.localScale = new Vector3(1f, 1f, 1f);

        // Snap to grid when dropped
        var currentPos = transform.position;
        transform.position = new Vector3(Mathf.Round(currentPos.x),
                                         Mathf.Round(currentPos.y),
                                         Mathf.Round(currentPos.z));

    }

}