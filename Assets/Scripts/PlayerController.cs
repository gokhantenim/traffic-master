using DG.Tweening;
using UnityEngine;

public class PlayerController : AbstractSingleton<PlayerController>
{
    public Vehicle MyVehicle;
    public static int StartSpeed = 15;
    public static float MaxBoost = 1.5f;
    Tweener _boostTween;
    Tweener _speedTween;
    public GameObject[] Vehicles;

    public int SelectedVehicle
    {
        get
        {
            return PlayerPrefs.GetInt("player-vehicle", 0);
        }
        set
        {
            PlayerPrefs.SetInt("player-vehicle", value);
        }
    }

    public void CreateMyVehicle()
    {
        GameObject myVehicleGameObject = Instantiate(Vehicles[SelectedVehicle]);
        myVehicleGameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        MyVehicle = myVehicleGameObject.GetComponent<Vehicle>();
        MyVehicle.isMyVehicle = true;
        CameraFollow.Instance.SetFollowTarget(myVehicleGameObject.transform);
    }

    public void ResetMyVehicle()
    {
        DeleteVehicle();
        CreateMyVehicle();
    }

    public void DeleteVehicle()
    {
        if (MyVehicle == null) return;
        if(_speedTween != null)
        {
            _speedTween.Kill();
            _speedTween = null;
        }
        if (_boostTween != null)
        {
            _boostTween.Kill();
            _boostTween = null;
        }
        CameraFollow.Instance.SetFollowTarget(null);
        Destroy(MyVehicle.gameObject);
        MyVehicle = null;
    }

    public void SelectVehicle(int vehicleIndex = 0)
    {
        SelectedVehicle = vehicleIndex;
        ResetMyVehicle();
    }

    public void NextVehicle()
    {
        int nextVehicle = SelectedVehicle + 1;
        if (nextVehicle >= Vehicles.Length)
        {
            nextVehicle = Vehicles.Length - 1;
        }
        SelectVehicle(nextVehicle);
    }

    public void PreviousVehicle()
    {
        int prevVehicle = SelectedVehicle - 1;
        if (prevVehicle <= 0)
        {
            prevVehicle = 0;
        }
        SelectVehicle(prevVehicle);
    }

    public void StartDrive()
    {
        _speedTween = DOTween.To((x) =>
        {
            MyVehicle.Speed = x;
        }, 0, StartSpeed, 2);
    }

    public void StopDrive()
    {
        if(_speedTween != null)
        {
            _speedTween.Kill();
        }
        MyVehicle.Speed = 0;
    }

    public void BoostUp()
    {
        if(_boostTween != null)
        {
            _boostTween.Kill();
        }
        _boostTween = DOTween.To((x) =>
        {
            MyVehicle.BoostFactor = x;
        }, 1, MaxBoost, 1);
    }

    public void BoostDown()
    {
        if (_boostTween != null)
        {
            _boostTween.Kill();
        }
        _boostTween = DOTween.To((x) =>
        {
            MyVehicle.BoostFactor = x;
        }, MaxBoost, 1, 1);
    }

    public void PlayerLeft()
    {
        if (GameManager.Instance.Status != GameManager.Statuses.GAME) return;
        MyVehicle.CurrentLane--;
        if (MyVehicle.CurrentLane < 0)
        {
            MyVehicle.CurrentLane = 0;
        }
        SoundManager.Instance.PlayMoveSound();
    }

    public void PlayerRight()
    {
        if (GameManager.Instance.Status != GameManager.Statuses.GAME) return;
        MyVehicle.CurrentLane++;
        if (MyVehicle.CurrentLane > 2)
        {
            MyVehicle.CurrentLane = 2;
        }
        SoundManager.Instance.PlayMoveSound();
    }

    public void Revive()
    {
        FindRevivePosition();
        StartDrive();
    }

    public void FindRevivePosition()
    {
        Vehicle myVehicle = PlayerController.Instance.MyVehicle;
        float currentZ = myVehicle.transform.position.z - 10;
        while (true)
        {
            currentZ -= 0.1f;
            for (int i = 0; i < 3; i++)
            {
                Vector3 currentPosition = new Vector3(i * LevelManager.LaneSize.x - LevelManager.LaneSize.x, myVehicle.transform.position.y, currentZ);
                Vehicle.CheckAroundResult check = myVehicle.CheckAround(currentPosition, myVehicle.EndPoint.localPosition.z);
                if (!check.Center && !check.Front && !check.Left && !check.Right)
                {
                    PlayerController.Instance.MyVehicle.CurrentLane = i;
                    PlayerController.Instance.MyVehicle.transform.position = currentPosition;
                    return;
                }
            }
        }
    }
}
