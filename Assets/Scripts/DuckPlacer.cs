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
        if(Input.touchCount == 1) {
            if(!touching) {
                touching = true;
                currentDuck = Resources.Load<GameObject>("test_absol_1").transform;//Instantiate(duck).transform;
                Debug.Log($"Attempted to load: {"test_absol_1"}, got: {currentDuck}");
                Vector3 newEulers = currentDuck.localEulerAngles;
                newEulers.y = Random.Range(0, 360);
                currentDuck.localEulerAngles = newEulers;
            }
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if(raycaster.Raycast(Input.GetTouch(0).position, hits)) {
                Vector3 newForward = hits[0].pose.position - currentDuck.position;
                newForward.y = 0;
                currentDuck.forward = Vector3.Slerp(currentDuck.forward, newForward, newForward.sqrMagnitude * 300);
                currentDuck.position = hits[0].pose.position;
                Debug.Log("WE DID IT, WE HIT THE RAY AND SAPWNESND THI THINGS? AT: " + currentDuck.position);
            } else {
                Debug.Log("RAYCAST AINT HIT DUFFIN");
            }
        } else {
            touching = false;
        }
    }
}
