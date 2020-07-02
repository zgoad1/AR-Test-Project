using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugInfo : MonoBehaviour {

    public TextMeshProUGUI tmp;

    private float frameRateSum = 0;
    private int frameRateSampleCount = 0;

    private void Update() {
        float thisFrameRate = 1f / Time.deltaTime;
        frameRateSum += thisFrameRate;
        frameRateSampleCount++;
        float avg = frameRateSum / frameRateSampleCount;
        tmp.text = "FPS: " + thisFrameRate + "\nAvg: " + avg;
    }
}
