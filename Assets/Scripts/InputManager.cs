using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputManager : AbstractSingleton<InputManager>
{
    public UnityEvent Left;
    public UnityEvent Right;
    Vector2 _startTouchPosition;
    bool touchStarted = false;

    void Update()
    {
        TouchControl();
#if UNITY_EDITOR
        KeyControl();
#endif
    }

    void TouchControl()
    {
        if (Input.touchCount == 0) return;
        Touch[] touches = Input.touches.Where(t => !EventSystem.current.IsPointerOverGameObject(t.fingerId)).ToArray();
        if (touches.Length == 0) return;
        Touch touch = touches[0];

        switch (touch.phase)
        {
            case TouchPhase.Began:
                touchStarted = true;
                _startTouchPosition = touch.position;
                break;
            case TouchPhase.Moved:
                if (!touchStarted) return;
                Vector3 moveDelta = touch.position - _startTouchPosition;
                if(moveDelta.x > 2)
                {
                    Right.Invoke();
                    touchStarted = false;
                } else if(moveDelta.x < -2)
                {
                    Left.Invoke();
                    touchStarted = false;
                }
                break;
        }
        
    }

    void KeyControl()
    {
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            Left.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            Right.Invoke();
        }
    }
}
