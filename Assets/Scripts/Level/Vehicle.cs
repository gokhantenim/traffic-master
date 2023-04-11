using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public Transform EndPoint;
    [HideInInspector]
    public bool isMyVehicle = false;
    [HideInInspector]
    public float Speed = 0;
    [HideInInspector]
    public float BoostFactor = 1;
    float LevelFactor => (GameManager.Instance.Level > 18 ? 18 : GameManager.Instance.Level) * 0.05f + 0.95f;
    [HideInInspector]
    public int SideMovementSpeed = 10;
    [HideInInspector]
    public int CurrentLane = 1;
    public Vector3 CurrentLanePosition => CurrentLane * LevelManager.LaneSize - LevelManager.LaneSize;
    [SerializeField]
    GameObject[] _wheels;
    BoxCollider _collider;

    void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (Speed == 0) return;

        Vector3 forwardMovement = Speed * BoostFactor * LevelFactor * Vector3.forward;
        Vector3 targetPositionDistance = (CurrentLanePosition - new Vector3(transform.position.x, 0));
        Vector3 sideMovement = SideMovementSpeed * (targetPositionDistance / 2);
        for(int i=0; i<_wheels.Length; i++)
        {
            GameObject wheel = _wheels[i];
            wheel.transform.Rotate(300 * Time.deltaTime, 0, 0);
        }
        transform.position += Time.deltaTime * (forwardMovement + sideMovement);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isMyVehicle) return;
        if (Speed <= 0) return;
        switch (other.tag)
        {
            case "Road":
                LevelManager.Instance.ReSpawnAll();
                break;
            case "LevelGate":
                LevelGate gate = other.GetComponent<LevelGate>();
                gate.Confetti.SetActive(true);
                break;
            case "Vehicle":
                GameManager.Instance.GameOver();
                break;
        }
    }

    public class CheckAroundResult {
        public bool Center = false;
        public bool Front = false;
        public bool Left = false;
        public bool Right = false;
    }

    public CheckAroundResult CheckAround(Vector3? checkPosition = null, float checkDistance = 0)
    {
        if(checkPosition == null)
        {
            checkPosition = _collider.bounds.center;
        }

        Vector3 centerCheckPosition = (Vector3)checkPosition;
        Vector3 frontCheckPosition = (Vector3)checkPosition + new Vector3(0, 0, _collider.bounds.size.z);
        Vector3 leftCheckPosition = (Vector3)checkPosition + new Vector3(_collider.bounds.size.x * -1, 0, 0);
        Vector3 rightCheckPosition = (Vector3)checkPosition + new Vector3(_collider.bounds.size.x, 0, 0);

        RaycastHit[] centerCastHits = Physics.BoxCastAll(
            centerCheckPosition, _collider.bounds.extents,
            transform.forward, Quaternion.identity, 0);
        bool centerCast = centerCastHits.Length > 0;

        RaycastHit[] frontCastHits = Physics.BoxCastAll(
            frontCheckPosition, _collider.bounds.extents,
            transform.forward, Quaternion.identity, checkDistance);
        bool frontCast = frontCastHits.Length > 0;

        RaycastHit[] leftCastHits = Physics.BoxCastAll(
            leftCheckPosition, _collider.bounds.extents,
            transform.forward, Quaternion.identity, 0);
        bool leftCast = leftCastHits.Length > 0;

        RaycastHit[] rightCastHits = Physics.BoxCastAll(
            rightCheckPosition, _collider.bounds.extents,
            transform.forward, Quaternion.identity, 0);
        bool rightCast = rightCastHits.Length > 0;

#if UNITY_EDITOR
        TestGizmos.Instance.Clear();
        TestGizmos.Instance.AddGizmo(new WireCubeGizmo(
                centerCheckPosition,
                _collider.bounds.size,
                centerCast ? Color.red : Color.black
            ));
        TestGizmos.Instance.AddGizmo(new WireCubeGizmo(
                frontCheckPosition,
                _collider.bounds.size,
                frontCast ? Color.red : Color.black
            ));
        TestGizmos.Instance.AddGizmo(new WireCubeGizmo(
                leftCheckPosition,
                _collider.bounds.size,
                leftCast ? Color.red : Color.black
            ));
        TestGizmos.Instance.AddGizmo(new WireCubeGizmo(
                rightCheckPosition,
                _collider.bounds.size,
                rightCast ? Color.red : Color.black
            ));
#endif

        return new CheckAroundResult()
        {
            Center = centerCast,
            Front = frontCast,
            Left = leftCast,
            Right = rightCast
        };
    }
}
