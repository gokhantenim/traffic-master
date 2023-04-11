using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraState
{
    public virtual void Init(CameraFollow cameraFollow) {}
    public virtual void Update(CameraFollow cameraFollow) {}
}
