using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoSphere : MonoBehaviour
{

    public Color gizmoColor = new Color(1, 0, 0, 0.5F);

    SphereCollider _sphereCollider;

    void OnEnable()
    {

    }
    //draw hit box
    void OnDrawGizmosSelected()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = gizmoColor;

        _sphereCollider = GetComponent<SphereCollider>();

        if (_sphereCollider.enabled)
        {
            Gizmos.DrawSphere(_sphereCollider.center, _sphereCollider.radius);
        }
        else
        {
            Gizmos.DrawSphere(_sphereCollider.center,0);
        }
    }
}
