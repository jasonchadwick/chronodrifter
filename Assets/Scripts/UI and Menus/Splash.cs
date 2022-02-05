using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class Splash : MonoBehaviour {
    public float fadeInTime;
    public float stayTime;
    public float fadeOutTime;
    public SpriteRenderer logo;
    private bool fadingIn;
    private bool fadingOut;

    public void Start() {
        fadingIn = true;
        fadingOut = false;
    }

    public void FixedUpdate() {
        if (fadingIn) {
            if (Time.timeSinceLevelLoad < fadeInTime) {
                logo.color = Color.Lerp(Color.black, Color.white, Time.timeSinceLevelLoad / fadeInTime);
            }
            else {
                fadingIn = false;
            }
        }
        else if (fadingOut) {
            if (Time.timeSinceLevelLoad < fadeInTime + stayTime + fadeOutTime) {
                logo.color = Color.Lerp(Color.white, Color.black, (Time.timeSinceLevelLoad - fadeInTime - stayTime) / fadeOutTime);
            }
            else {
                SceneManager.LoadScene("MainMenu");
            }
        }
        else {
            if (Time.timeSinceLevelLoad < fadeInTime + stayTime) {
                logo.color = Color.white;
            }
            else {
                fadingOut = true;
            }
        }
    }
}