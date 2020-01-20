using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoPoint : MonoBehaviour
{
    public Color gizmoColor = new Color(1, 0, 0, 0.5F);
    void OnDrawGizmosSelected()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = gizmoColor;

        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}
