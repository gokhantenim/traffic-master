using UnityEngine;

public class CameraFollow : AbstractSingleton<CameraFollow>
{
    public Transform FollowTarget;
    public Vector3 FollowDistance = new Vector3(0, 4.20f, -1);
    public Vector3 HomeDistance = new Vector3(0, 4f, 3.5f);
    public Vector3 FollowRotation = new Vector3(24, 0, 0);
    public Vector3 FollowPosition => FollowTarget.position + FollowDistance;
    CameraState _state = null;
    
    public void SetFollowTarget(Transform target)
    {
        FollowTarget = target;
    }

    public void SetState(CameraState state)
    {
        _state = state;
        _state.Init(this);
        System.GC.Collect();
    }

    //public void SetState(Type classType)
    //{
    //    _state = (CameraState) Activator.CreateInstance(classType, new object[] {this});
    //}

    void Update()
    {
        _state.Update(this);
    }
}
