using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerMove))]
[CanEditMultipleObjects]
public class PlayerMoveEditor : Editor
{
    PlayerMove thisMove;

    SerializedProperty rollDuration;
    SerializedProperty groundHeight,gravity,stopMovementVelocity;
    SerializedProperty rotateSpeed, smoothRotation, speedSmoothing;
    SerializedProperty lerpable, rawInput;

    SerializedProperty groundLayer, obsLayer;
    void OnEnable()
    {
        rollDuration = serializedObject.FindProperty("rollDuration");

        groundLayer = serializedObject.FindProperty("groundLayer");
        obsLayer = serializedObject.FindProperty("obsLayer");

        groundHeight = serializedObject.FindProperty("groundHeight");
        gravity = serializedObject.FindProperty("gravity");
        stopMovementVelocity = serializedObject.FindProperty("stopMovementVelocity");

        rotateSpeed = serializedObject.FindProperty("rotateSpeed");
        smoothRotation = serializedObject.FindProperty("smoothRotation");
        speedSmoothing = serializedObject.FindProperty("speedSmoothing");
        lerpable = serializedObject.FindProperty("lerpable");
        rawInput = serializedObject.FindProperty("rawInput");
        thisMove = (PlayerMove)target;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(rollDuration);

        EditorGUILayout.PropertyField(groundLayer);
        EditorGUILayout.PropertyField(obsLayer);

        EditorGUILayout.PropertyField(groundHeight);
        EditorGUILayout.PropertyField(gravity);
        EditorGUILayout.PropertyField(stopMovementVelocity);

        EditorGUILayout.PropertyField(lerpable);
        EditorGUILayout.PropertyField(rawInput);

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
