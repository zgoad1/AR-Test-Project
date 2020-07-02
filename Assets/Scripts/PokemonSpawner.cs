using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonSpawner : MonoBehaviour {

    public string[] names;
    public GameObject[] prefabs;

    private static Dictionary<string, GameObject> recognizedPokemon;

    private void OnEnable() {
        if(recognizedPokemon == null) {
            recognizedPokemon = new Dictionary<string, GameObject>();
            try {
                int i;
                for(i = 0; i < names.Length; i++) {
                    recognizedPokemon.Add(names[i], prefabs[i]);
                }
                if(i < prefabs.Length) {
                    Debug.LogError("Prefabs list longer than names list", gameObject);
                }
            } catch {
                Debug.LogError("Adding keys and values to dictionary failed. Are the arrays the same length?");
            }
        }
    }

    public void Spawn(string name) {
        try {
            Debug.Log("Attempting to spawn " + name);
            Transform newPoke = Instantiate(recognizedPokemon[name], transform).transform;
        } catch {
            Debug.LogError("Card has invalid name: " + name);
        }
    }
}
