using UnityEngine;

//https://forum.unity.com/threads/window-resize-event.40253/

public class WindowManager : MonoBehaviour
{
    public delegate void ScreenSizeChangeEventHandler(int Width, int Height);       //  Define a delgate for the event
    public event ScreenSizeChangeEventHandler ScreenSizeChangeEvent;                //  Define the Event
    protected virtual void OnScreenSizeChange(int Width, int Height)
    {              //  Define Function trigger and protect the event for not null;
        if (ScreenSizeChangeEvent != null) ScreenSizeChangeEvent(Width, Height);
    }
    private Vector2 lastScreenSize;
    public static WindowManager instance = null;                                    //  Singleton for call just one instance
    void Awake()
    {
        lastScreenSize = new Vector2(Screen.width, Screen.height);
        instance = this;                                                            // Singleton instance
    }

    bool _canUpdate = false;
    float _gatherTime = 0f;
    float _setTime = 1f;

    void Update()
    {
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButton(0))
        {
            _canUpdate = true;
            _gatherTime = 0f;
        }

        if (_canUpdate == true)
        {
            _gatherTime += Time.deltaTime;
            if (_gatherTime > _setTime)
                _canUpdate = false;

         Vector2 screenSize = new Vector2(Screen.width, Screen.height);
            if (this.lastScreenSize != screenSize)
            {
                this.lastScreenSize = screenSize;
                OnScreenSizeChange(Screen.width, Screen.height);                        //  Launch the event when the screen size change
            }
        }
    }
}

