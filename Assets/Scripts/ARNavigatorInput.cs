using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARNavigatorInput : MonoBehaviour {

    public ARRaycastManager raycaster;
    public GameObject pointer;

    private void Reset() {
        raycaster = FindObjectOfType<ARRaycastManager>();
    }

    protected void Awake() {
        if(raycaster == null) {
            raycaster = FindObjectOfType<ARRaycastManager>();
        }
    }

    void Update() {
        if(Input.touchCount > 0) {

            // Do a physics raycast to check if we touched a card
            RaycastHit hit;
            Ray ray = MainCamera.instance.ScreenPointToRay(Input.GetTouch(0).position);
            if(Physics.Raycast(ray, out hit)) {
                PokemonSpawner spawner;
                if(!ARNavigator.exiting && hit.collider.TryGetComponent(out spawner)) {
                    if(Input.GetTouch(0).phase == TouchPhase.Began) {
                        //Debug.Log("Raycast hit Spawner");
                        spawner.OnTouch();
                    }
                    return;
                }
            }

            // Raycast against AR planes and set the destination
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if(raycaster.Raycast(Input.GetTouch(0).position, hits)) {
                //Debug.Log("Raycast hit Plane");
                if(!ARNavigator.exiting) {
                    ARNavigator.Destination = hits[0].pose.position;
                }
            }
        }
    }
}
