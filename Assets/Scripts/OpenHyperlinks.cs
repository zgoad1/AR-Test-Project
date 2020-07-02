using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpenHyperlinks : MonoBehaviour {

    public TextMeshProUGUI link;

    private void Reset() {
        link = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnClick() {
        Application.OpenURL(link.text);
    }
}
