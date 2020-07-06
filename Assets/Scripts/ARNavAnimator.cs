using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This was needed because functions called by an Animator have to be on the same
// object or a child of that object
public class ARNavAnimator : MonoBehaviour {

    public ARNavigator myNav;

    public void OnExitAnimFinish() {
        Debug.Log("ARNavAnimator - ONEXITANIMFINISH CALLED");
        myNav.OnExitAnimFinish();
    }
}
