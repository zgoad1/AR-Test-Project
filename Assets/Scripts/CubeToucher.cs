using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CubeToucher : MonoBehaviour {

    public ARRaycastManager raycaster;

    private void Reset() {
        raycaster = FindObjectOfType<ARRaycastManager>();
    }

    void Update() {
        if(Input.touchCount > 0) {
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if(raycaster.Raycast(Input.GetTouch(0).position, hits)) {
                transform.position = hits[0].pose.position;
            }
        } else {
            //Debug.LogWarning("No touch!");
        }

        //Input.gyro.enabled = true;
        //if(Input.touchCount > 0) { 
        //    Ray ray = MainCamera.instance.ScreenPointToRay(Input.GetTouch(0).position);
        //    RaycastHit hit;
        //    if(Physics.Raycast(ray, out hit)) {
        //        transform.position = hit.point;
        //    }
        //} else {
        //    //Debug.LogWarning("No touch!");
        //}
        //Debug.Log("Camera enabled: " + MainCamera.instance.GetComponent<ARCameraBackground>().backgroundRenderingEnabled);

    }
}
