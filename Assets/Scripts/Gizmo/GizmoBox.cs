using UnityEngine;
using System.Collections;

public class GizmoBox : MonoBehaviour
{

    public Color gizmoColor = new Color(1, 0, 0, 0.5F);

    BoxCollider _boxCollider;

    void OnEnable()
    {
       
    }
    //draw hit box
    void OnDrawGizmosSelected()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = gizmoColor;

        _boxCollider = GetComponent<BoxCollider>();

        if (_boxCollider.enabled)
        {
            Gizmos.DrawCube(_boxCollider.center, _boxCollider.size);
        }
        else
        {
            Gizmos.DrawCube(Vector3.zero, Vector3.zero);
        }


    }
}


