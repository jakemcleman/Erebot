using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Door))]
public class DoorEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Door myDoor = (Door)target;

        Rect r1 = EditorGUILayout.BeginHorizontal("Button");
        if(GUI.Button(r1, GUIContent.none))
        {
            myDoor.transform.rotation = Quaternion.Euler(myDoor.openRotation);
        }
        GUILayout.Label("Open Door");
        EditorGUILayout.EndHorizontal();

        Rect r2 = EditorGUILayout.BeginHorizontal("Button");
        if (GUI.Button(r2, GUIContent.none))
        {
            myDoor.transform.rotation = Quaternion.Euler(myDoor.closedRotation);
        }
        GUILayout.Label("Close Door");
        EditorGUILayout.EndHorizontal();

        r1 = EditorGUILayout.BeginHorizontal("Button");
        if (GUI.Button(r1, GUIContent.none))
        {
            myDoor.openRotation = myDoor.transform.rotation.eulerAngles;
        }
        GUILayout.Label("Set open");
        EditorGUILayout.EndHorizontal();

        r2 = EditorGUILayout.BeginHorizontal("Button");
        if (GUI.Button(r2, GUIContent.none))
        {
            myDoor.closedRotation = myDoor.transform.rotation.eulerAngles;
        }
        GUILayout.Label("Set closed");
        EditorGUILayout.EndHorizontal();
    }
}
