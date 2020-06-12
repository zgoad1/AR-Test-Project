using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckThrower : MonoBehaviour {

    public GameObject duck;

    private void Update() {
        foreach(Touch touch in Input.touches) {
            if(touch.phase == TouchPhase.Began) {
                ThrowDuck();
            }
        }
    }

    private void ThrowDuck() {
        Rigidbody newDuck = Instantiate(duck).GetComponent<Rigidbody>();
        newDuck.transform.position = MainCamera.instance.transform.position - MainCamera.instance.transform.up * 0.25f;
        newDuck.transform.forward = MainCamera.instance.transform.forward;
        Vector3 forceDirection = MainCamera.instance.transform.forward;
        forceDirection.y += 0.25f;
        newDuck.AddForce(forceDirection * 10, ForceMode.Impulse);
    }
}
