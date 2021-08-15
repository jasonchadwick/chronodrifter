using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

class LightColor : MonoBehaviour {
    public Light2D lightComponent;
    public Color forwardColor = Color.white;
    public Color reverseColor = Color.blue;
    public Color pauseColor = Color.green;
    private Color baseColor;
    public float colorLerp = 0.2f;

    void Start() {
        lightComponent = GetComponent<Light2D>();
        baseColor = lightComponent.color;
        TimeEventManager.OnPause += UpdateOnPause;
        TimeEventManager.OnReverse += UpdateOnReverse;
    }

    void LerpForward() {
        lightComponent.color = Color.Lerp(baseColor, forwardColor, colorLerp);
    }

    void LerpReverse() {
        lightComponent.color = Color.Lerp(baseColor, reverseColor, colorLerp);
    }

    void LerpPause() {
        lightComponent.color = Color.Lerp(baseColor, pauseColor, colorLerp);
    }

    void LerpReversePause() {
        Color halfway = Color.Lerp(pauseColor, reverseColor, 0.5f);
        lightComponent.color = Color.Lerp(baseColor, halfway, colorLerp);
    }

    void UpdateOnPause() {
        if (TimeEventManager.isPaused && TimeEventManager.isReversed) {
            LerpReversePause();
        }
        else if (TimeEventManager.isPaused) {
            LerpPause();
        }
        else if (TimeEventManager.isReversed) {
            LerpReverse();
        }
        else {
            LerpForward();
        }
    }

    void UpdateOnReverse() {
        if (TimeEventManager.isPaused) {
            if (TimeEventManager.isReversed) {
                LerpReversePause();
            }
            else {
                LerpPause();
            }
        }
        else {
            if (TimeEventManager.isReversed) {
                LerpReverse();
            }
            else {
                LerpForward();
            }
        }
    }
}