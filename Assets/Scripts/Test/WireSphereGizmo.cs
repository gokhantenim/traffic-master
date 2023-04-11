using System.Collections;
using System.Data.SqlTypes;
using UnityEngine;

public class WireSphereGizmo : TestGizmo
{
    Color gizmoColor;
    float radius = 2;
    Vector3 center = Vector3.zero;
    public WireSphereGizmo(Color _gizmoColor, Vector3 _center, float _radius)
    {
        gizmoColor = _gizmoColor;
        radius = _radius;
        center = _center;
    }
    public void Draw()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(center, radius);
    }
}