using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameController))]
public class GameControllerEditor : Editor
{
    private int zone = 1;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (!Application.isPlaying) return;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
        zone = EditorGUILayout.IntField("Zone", zone);

        if (GUILayout.Button("Go To Zone"))
            ((GameController)target).DebugSetZone(zone);

        if (GUILayout.Button("Spin To Bomb"))
            ((GameController)target).DebugSpinToBomb();
    }
}
