using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ReferencePointAdder : MonoBehaviour {

    public ARAnchorManager manager;
    public ARRaycastManager raycaster;

    private bool trackingTouch;

    private void Reset() {
        manager = FindObjectOfType<ARAnchorManager>();
        raycaster = FindObjectOfType<ARRaycastManager>();
    }

    private void Update() {
        if(Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began) {
                trackingTouch = true;
            } else if(touch.phase == TouchPhase.Ended && trackingTouch) {
                AddAnchor(touch);
            }
        } else {
            trackingTouch = false;
        }
    }

    private void AddAnchor(Touch touch) {
        //Vector3 position = MainCamera.instance.transform.position + MainCamera.instance.transform.forward;
        //Quaternion rotation = MainCamera.instance.transform.rotation;
        //manager.AddAnchor(new Pose(position, rotation));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if(raycaster.Raycast(touch.position, hits)) {
            Vector3 newEulers = new Vector3(0, Random.Range(0, 360), 0);
            Quaternion newRotation = Quaternion.Euler(newEulers);
            Pose newPose = new Pose(hits[0].pose.position, newRotation);
            ARAnchor anchor = manager.AddAnchor(hits[0].pose);
            Transform duck = anchor.GetComponentInChildren<DuckController>().transform;
            duck.rotation = newRotation;
        }
    }
}
