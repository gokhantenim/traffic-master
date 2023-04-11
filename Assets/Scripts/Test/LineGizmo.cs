using System.Collections;
using System.Data.SqlTypes;
using UnityEngine;

public class LineGizmo : TestGizmo
{
    Color gizmoColor;
    Vector3 from = Vector3.zero;
    Vector3 to = Vector3.zero;
    public LineGizmo(Color _gizmoColor, Vector3 _from, Vector3 _to)
    {
        gizmoColor = _gizmoColor;
        from = _from;
        to = _to;
    }
    public void Draw()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawLine(from, to);
    }
}