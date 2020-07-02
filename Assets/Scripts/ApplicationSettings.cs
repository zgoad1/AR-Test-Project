using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class ApplicationSettings : MonoBehaviour {

    public int targetFrameRate = 60;

    void Start() {
        Application.targetFrameRate = targetFrameRate;
        FindObjectOfType<ARSession>().matchFrameRateRequested = false;
        if(SceneManager.GetActiveScene().buildIndex == 0) {
            StartCoroutine(WHYWHYWHY());
        }
    }

    private IEnumerator WHYWHYWHY() {
        yield return new WaitForSeconds(.1f);
        SceneManager.LoadScene(1);
    }
}
