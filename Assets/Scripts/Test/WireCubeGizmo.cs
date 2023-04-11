using System.Collections;
using System.Data.SqlTypes;
using UnityEngine;

public class WireCubeGizmo : TestGizmo
{
    Color gizmoColor;
    Vector3 center = Vector3.zero;
    Vector3 size = Vector3.zero;
    public WireCubeGizmo(Vector3 _center, Vector3 _size, Color? _gizmoColor = null)
    {
        gizmoColor = _gizmoColor == null ? Color.black : (Color)_gizmoColor;
        center = _center;
        size = _size;
    }
    public void Draw()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(center, size);
    }
}