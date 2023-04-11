using UnityEngine;
using DG.Tweening;

public class CameraGameState : CameraState
{
    float _followSpeed = 0;
    public override void Init(CameraFollow cameraFollow)
    {
        // look at my vehicle while starting
        DOTween.To((x) => {
            Vector3 lookAtPosition = cameraFollow.FollowTarget.position + new Vector3(0, 0, 3);
            cameraFollow.transform.LookAt(lookAtPosition, Vector3.up);
        }, 0, 1, cameraFollow.transform.position.z < cameraFollow.FollowTarget.position.z + 3.5f ? 0.6f : 1)
            .OnComplete(() => {
                // start following my vehicle
                DOTween.To((x) =>
                {
                    _followSpeed = x;
                }, 0, 15, 2);
                cameraFollow.transform.DORotate(cameraFollow.FollowRotation, 2);
            });
    }

    public override void Update(CameraFollow cameraFollow)
    {
        //cameraFollow.transform.LookAt(cameraFollow.FollowTarget);
        cameraFollow.transform.position = Vector3.Lerp(
            cameraFollow.transform.position, cameraFollow.FollowPosition, Time.deltaTime * _followSpeed);
    }
}
