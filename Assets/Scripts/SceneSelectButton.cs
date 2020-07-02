using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelectButton : MonoBehaviour {

    public string scene;

    public void OnClick() {
        SceneChanger.ChangeScene(scene);
    }
}
