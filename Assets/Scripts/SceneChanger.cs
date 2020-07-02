using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    public static SceneChanger instance;

    public Animator blackFade;
    [HideInInspector] public static string nextScene;



    private void Reset() {
        blackFade = GetComponent<Animator>();
    }

    private void Awake() {
        if(instance) {
            Debug.LogWarning("Found extra SceneChanger, destroying");
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void OnDestroy() {
        if(instance == this) instance = null;
    }

    public static void ChangeScene(string newScene) {
        nextScene = newScene;
        instance.blackFade.SetBool("Visible", true);
    }

    public void OnFadeToBlack() {
        Menu.ClearMenuStack();
        SceneManager.LoadScene(nextScene);
    }
}
