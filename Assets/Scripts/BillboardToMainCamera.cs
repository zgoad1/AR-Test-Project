using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardToMainCamera : MonoBehaviour {

    private void Update() {
        transform.LookAt(MainCamera.instance.transform);
    }
}
