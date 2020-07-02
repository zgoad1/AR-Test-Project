using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePersistent : MonoBehaviour {
    private void Start() {
        DontDestroyOnLoad(gameObject);
    }
}
