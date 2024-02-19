using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[UnityEditor.DrawGizmo(UnityEditor.GizmoType.Pickable)]
public class JointEditor : MonoBehaviour
{
    public Color JointColor = Color.white;
    private void OnDrawGizmos()
    {
        Gizmos.color = JointColor;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
