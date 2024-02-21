using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
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
#endif