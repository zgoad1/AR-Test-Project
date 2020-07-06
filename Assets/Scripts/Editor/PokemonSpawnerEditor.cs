using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PokemonSpawner))]
public class PokemonSpawnerEditor : Editor {

    private SerializedProperty names, prefabs;
    private PokemonSpawner spawner;

    private void OnEnable() {
        spawner = (PokemonSpawner)target;
        names = serializedObject.FindProperty("names");
        prefabs = serializedObject.FindProperty("prefabs");
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if(spawner.names.Length != spawner.prefabs.Length) {
            ShowWarning();
            return;
        }
        for(int i = 0; i < spawner.names.Length; i++) {
            if(spawner.names[i].Length == 0 || spawner.prefabs[i] == null) {
                ShowWarning();
                return;
            }
        }
    }

    private void ShowWarning() {
        EditorGUILayout.LabelField("Names and Prefabs lengths do not match, or they contain null elements!");
    }
}
