using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class CardIdentifier : MonoBehaviour {

    public ARTrackedImageManager manager;

    private void Reset() {
        manager = GetComponent<ARTrackedImageManager>();
    }

    private void Start() {
        manager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs) {
        foreach(ARTrackedImage image in eventArgs.added) {
            image.GetComponentInChildren<PokemonSpawner>().Spawn(image.name);
        }
    }

}
