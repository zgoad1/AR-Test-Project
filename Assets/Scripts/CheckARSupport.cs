using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CheckARSupport : MonoBehaviour {

    [SerializeField] ARSession m_Session;

    private void Reset() {
        m_Session = FindObjectOfType<ARSession>();
    }

    IEnumerator Start() {
        if(ARSession.state == ARSessionState.None || ARSession.state == ARSessionState.CheckingAvailability) {
            yield return ARSession.CheckAvailability();
        }

        switch(ARSession.state) {
            case ARSessionState.Unsupported:
                Debug.LogError("This device is unsupported!");
                break;
            case ARSessionState.Installing:
                Debug.LogError("AR is still installing!");
                break;
            case ARSessionState.NeedsInstall:
                Debug.LogError("AR needs to be installed!");
                break;
            case ARSessionState.Ready:
                // Start the AR session
                Debug.Log("This device is supported!");
                m_Session.enabled = true;
                break;
            default:
                Debug.LogError("Unknown error\ncase: " + ARSession.state);
                break;
        }
    }

}
