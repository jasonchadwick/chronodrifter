using System.Collections.Generic;
using UnityEngine;

class MainMenuMusic : MonoBehaviour {
    public AudioSource music;
    public float fadeInTime;
    private bool fadingIn;

    private static MainMenuMusic _instance;

    public static MainMenuMusic instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<MainMenuMusic>();

                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            if (this != _instance) {
                Destroy(this.gameObject);
            }
        }
    }

    void Update() {
        if (fadingIn) {
            if (Time.timeSinceLevelLoad < fadeInTime) {
                music.volume = Mathf.Lerp(0, 1, Time.time / fadeInTime);
            }
            else {
                music.volume = 1;
                fadingIn = false;
            }
        }
    }

    void Start() {
        if (!music.isPlaying) {
            fadingIn = true;
            music.volume = 0;
            music.Play();
        }
    }
}