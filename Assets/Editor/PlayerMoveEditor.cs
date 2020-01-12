using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerMove))]
[CanEditMultipleObjects]
public class PlayerMoveEditor : Editor
{
    PlayerMove thisMove;
    SerializedProperty groundHeight,gravity,stopMovementVelocity,normalRunSpeed;
    SerializedProperty rotateSpeed, smoothRotation, speedSmoothing;
    SerializedProperty lerpable;

    void OnEnable()
    {
        groundHeight = serializedObject.FindProperty("groundHeight");
        gravity = serializedObject.FindProperty("gravity");
        stopMovementVelocity = serializedObject.FindProperty("stopMovementVelocity");
        normalRunSpeed = serializedObject.FindProperty("normalRunSpeed");

        rotateSpeed = serializedObject.FindProperty("rotateSpeed");
        smoothRotation = serializedObject.FindProperty("smoothRotation");
        speedSmoothing = serializedObject.FindProperty("speedSmoothing");
        lerpable = serializedObject.FindProperty("lerpable");
        thisMove = (PlayerMove)target;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(groundHeight);
        EditorGUILayout.PropertyField(gravity);
        EditorGUILayout.PropertyField(stopMovementVelocity);
        EditorGUILayout.PropertyField(normalRunSpeed);

        EditorGUILayout.PropertyField(lerpable);

        if (lerpable.boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(rotateSpeed);
            EditorGUILayout.PropertyField(smoothRotation, new GUIContent("smoothRotation"));
            EditorGUILayout.PropertyField(speedSmoothing, new GUIContent("speedSmoothing"));
            EditorGUI.indentLevel--;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
