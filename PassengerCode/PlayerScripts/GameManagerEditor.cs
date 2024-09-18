using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        if (Application.isPlaying)
        {
            GUILayout.Space(10f);
            GUILayout.Label("Current Game State:");
            GUILayout.Space(5f);
            GUILayout.Label(GameManager.GM.State.ToString().ToUpper());
            GUILayout.Space(10f);
            GUILayout.Label("Game States");

            if (GUILayout.Button("Gameplay"))
            {
                ((GameManager)target).Gameplay();
            }
            if (GUILayout.Button("Hiding"))
            {
                ((GameManager)target).Hiding();
            }
            if (GUILayout.Button("Cutscene"))
            {
                ((GameManager)target).Cutscene();
            }

            GUILayout.Label("Other");
            if (GUILayout.Button("Transition"))
            {
                ((GameManager)target).Transition();
            }

        }
    }
    
}
#endif
