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
            Debug.Log("Found an image! " + image.referenceImage.name);
            if(PokemonSpawner.ActiveCard == null) {
                image.GetComponentInChildren<PokemonSpawner>().Activate(image.referenceImage.name);
            }
        }
        //foreach(ARTrackedImage image in manager.trackables) {
        //    Debug.Log("Image at " + image.transform.position);
        //}
    }

}
