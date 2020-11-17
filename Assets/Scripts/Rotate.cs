using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public Vector3 angularVelocity = Vector3.zero;
    public bool randomize = false;

    // Update is called once per frame
    void Update() {

        if(randomize) {
            angularVelocity.x = Mathf.Clamp(angularVelocity.x + Random.Range(-0.5f, 0.5f), -5, 5);
            angularVelocity.y = Mathf.Clamp(angularVelocity.y + Random.Range(-0.5f, 0.5f), -5, 5);
            angularVelocity.z = Mathf.Clamp(angularVelocity.z + Random.Range(-0.5f, 0.5f), -5, 5);
        }

        transform.Rotate(angularVelocity * 60 * Time.deltaTime);
    }
}
