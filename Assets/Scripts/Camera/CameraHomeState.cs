using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraHomeState : CameraState
{
    Vector3 rotateAroundPosition = new Vector3(0, 0, 3);
    Vector2 _lastTouchPosition;
    float defaultFactor = 10;
    float rotationFactor = 10;

    public override void Init(CameraFollow cameraFollow)
    {
        cameraFollow.transform.position = rotateAroundPosition + cameraFollow.HomeDistance;
    }

    public override void Update(CameraFollow cameraFollow)
    {
        TouchMoveControl();
        //cameraFollow.transform.position = rotateAroundPosition + cameraFollow.HomeDistance;
        cameraFollow.transform.LookAt(rotateAroundPosition);
        cameraFollow.transform.RotateAround(
            rotateAroundPosition,
            Vector3.up,
            rotationFactor * Time.deltaTime);
    }

    void TouchMoveControl()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.touches[0];
        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;

        switch (touch.phase)
        {
            case TouchPhase.Began:
                _lastTouchPosition = touch.position;
                break;
            case TouchPhase.Moved:
                Vector3 delta = touch.position - _lastTouchPosition;
                rotationFactor = delta.x * 20;
                _lastTouchPosition = touch.position;
                break;
            case TouchPhase.Ended:
                rotationFactor = defaultFactor;
                break;
        }
    }
}
