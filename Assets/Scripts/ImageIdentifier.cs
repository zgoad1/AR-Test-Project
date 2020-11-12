using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ImageIdentifier : MonoBehaviour {

    public ARTrackedImageManager manager;

    private void Reset() {
        manager = GetComponent<ARTrackedImageManager>();
    }

    private void Start() {
        manager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs) {
        foreach(ARTrackedImage image in eventArgs.added) {
            //image.GetComponentInChildren<TextMeshPro>().text = image.referenceImage.name;
            try {
                //image.GetComponentInChildren<AddressableLoader>().LoadAsset(image.referenceImage.name, image.transform.position);
                GameObject obj = Resources.Load<GameObject>(image.referenceImage.name);
                obj.transform.position = image.transform.position;
                Debug.Log("SUCCESSFULLY SPAWNED A " + obj.name);
            } catch {
                Debug.LogError("ERROR - image is null, probably!");
            }
        }
    }

}
