using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ImageIdentifier : MonoBehaviour {

    public ARTrackedImageManager manager;

    public Camera cam;

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
                //image.GetComponentInChildren<AddressableLoader>().LoadAsset(image.referenceImage.name, image.transform.position, Quaternion.identity);
                GameObject obj = Resources.Load<GameObject>(image.referenceImage.name);
                Debug.Log($"Attempted to load: {image.referenceImage.name}, got: {obj}");
                obj.transform.position = cam.transform.position + cam.transform.forward * 0.2f;//image.transform.position;
                //obj.transform.parent = image.GetComponentInChildren<AddressableLoader>().transform;
                //obj.transform.localPosition = Vector3.zero;
                Debug.Log("SUCCESSFULLY SPAWNED A " + obj.name);
            } catch {
                Debug.LogError("ERROR - image is null, probably!");
            }
        }
    }

}
