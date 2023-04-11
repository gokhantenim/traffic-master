using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGizmos : AbstractSingleton<TestGizmos>
{
    List<TestGizmo> testGizmos = new List<TestGizmo>();

    void OnDrawGizmos()
    {
        foreach (TestGizmo testGizmo in testGizmos)
        {
            testGizmo.Draw();
        }
    }

    public void Clear()
    {
        testGizmos.Clear();
    }

    public void AddGizmo(TestGizmo gizmo)
    {
        testGizmos.Add(gizmo);
    }
}