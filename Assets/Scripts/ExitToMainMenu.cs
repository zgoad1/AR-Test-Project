using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitToMainMenu : MonoBehaviour {

    void Update() {
        if(Application.platform == RuntimePlatform.Android) {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                Debug.Log("Escape pressed!");
                SceneChanger.ChangeScene("Title Screen");
            }
        } else {
            if(Input.GetMouseButtonDown(1)) {
                Debug.Log("Escape pressed! (mouse)");
                SceneChanger.ChangeScene("Title Screen");
            }
        }
    }
}
