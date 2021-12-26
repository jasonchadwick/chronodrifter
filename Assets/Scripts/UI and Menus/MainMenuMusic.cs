using System.Collections.Generic;
using UnityEngine;

class MainMenuMusic : MonoBehaviour {
    public AudioSource music;
    public float fadeInTime;
    public float volume;
    private bool fadingIn = true;

    private static MainMenuMusic _instance;

    public static MainMenuMusic instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<MainMenuMusic>();
                _instance.Initialize();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    void Awake() {
        if (_instance == null) {
            _instance = this;
            Initialize();
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
            if (Time.time < fadeInTime) {
                music.volume = Mathf.Lerp(0, volume, Time.time / fadeInTime);
            }
            else {
                music.volume = volume;
                fadingIn = false;
            }
        }
    }

    void Initialize() {
        fadingIn = true;
        music.volume = 0;
        music.Play();
    }

    void Start() {
        if (!music.isPlaying) {
            fadingIn = true;
        }
    }
}