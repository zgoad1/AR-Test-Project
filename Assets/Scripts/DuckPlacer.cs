using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DuckPlacer : MonoBehaviour {

    public ARRaycastManager raycaster;
    public GameObject duck;

    private bool touching = false;
    private Transform currentDuck;

    private void Reset() {
        raycaster = FindObjectOfType<ARRaycastManager>();
    }

    void Update() {
        if(Input.touchCount > 0) {
            if(!touching) {
                touching = true;
                currentDuck = Instantiate(duck).transform;
                Vector3 newEulers = currentDuck.localEulerAngles;
                newEulers.y = Random.Range(0, 360);
                currentDuck.localEulerAngles = newEulers;
            }
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if(raycaster.Raycast(Input.GetTouch(0).position, hits)) {
                currentDuck.position = hits[0].pose.position;
            }
        } else {
            touching = false;
        }
    }
}
