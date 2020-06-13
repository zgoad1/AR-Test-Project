using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ResetAllTrackables : MonoBehaviour {

    private void Update() {
        if(Input.touchCount == 2) {
            bool reset = false;
            bool oneBegan = false;
            foreach(Touch touch in Input.touches) {
                if(touch.phase == TouchPhase.Began) {
                    oneBegan = true;
                }
                if(touch.tapCount != 2) {
                    break;
                }
                if(oneBegan) {
                    reset = true;
                }
            }
            if(reset) {
                Debug.Log("Resetting");
                StartCoroutine(EnableDisableManagers(false));
                foreach(ARSession s in FindObjectsOfType<ARSession>()) {
                    s.Reset();
                }
                StartCoroutine(EnableDisableManagers(true, 0.1f));
            }
        }
    }

    private IEnumerator EnableDisableManagers(bool toEnable, float waitTime = 0f) {
        yield return new WaitForSeconds(waitTime);
        foreach(ARPlaneManager m in FindObjectsOfType<ARPlaneManager>()) {
            m.enabled = toEnable;
            if(!toEnable) {
                foreach(ARPlane t in m.trackables) {
                    m.gameObject.SetActive(false);
                }
            }
        }
        foreach(ARTrackedImageManager m in FindObjectsOfType<ARTrackedImageManager>()) {
            m.enabled = toEnable;
            if(!toEnable) {
                foreach(ARTrackedImage t in m.trackables) {
                    m.gameObject.SetActive(false);
                }
            }
        }
    }
}
