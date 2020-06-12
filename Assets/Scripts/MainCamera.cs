using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

    public static Camera instance;

    private void Awake() {
        if(instance) {
            Debug.LogWarning("Found extra MainCamera instance, destroying");
            Destroy(this);
            return;
        }
        instance = GetComponent<Camera>();


    }
}
