using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class MusicManager : MonoBehaviour {
    public AudioSource music;
    // need a different track for reversed bc WebGL does not support playing music backwards
    public AudioSource reversedMusic;
    public float normalPitch;
    public float pausedPitch;
    public float pitchChangeDuration;
    public float startOffset;
    public float volumeLevel;
    private bool fadingIn;
    public bool fadingOut;
    private bool isFirstLoad = true;
    private float fadingOutTime;
    private float oldVolume;

    private static Scene scene;
    private static MusicManager _instance;
    public static MusicManager instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<MusicManager>();

                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    // maintain one MusicManager when level changes.
    void Awake() {
        if (_instance == null) {
            _instance = this;
            this.Initialize();
            DontDestroyOnLoad(this);
        }
        else {
            if (this != _instance) {
                _instance.Initialize();
                Destroy(this.gameObject);
            }
        }
    }

    void Initialize() {
        TimeEventManager.OnPause += OnPauseReverse;
        TimeEventManager.OnReverse += OnPauseReverse;
        if (isFirstLoad) {
            fadingIn = true;
            music.Play();
            music.time = startOffset;
            music.volume = 0;
            reversedMusic.Pause();
            reversedMusic.volume = volumeLevel;
            oldVolume = music.volume;
            isFirstLoad = false;
        }
        else {
            OnPauseReverse();
        }
    }

    void Update() {
        if (fadingIn) {
            if (Time.timeSinceLevelLoad / Time.timeScale < 1) {
                music.volume = Mathf.Lerp(oldVolume, volumeLevel, Time.timeSinceLevelLoad / Time.timeScale);
            }
            else {
                music.volume = volumeLevel;
                fadingIn = false;
            }
        }
        if (fadingOut) {
            fadingOutTime += Time.deltaTime;
            if (fadingOutTime / Time.timeScale < 1) {
                music.volume = Mathf.Lerp(volumeLevel, 0.2f, fadingOutTime / Time.timeScale);
            }
            else {
                music.volume = 0.2f;
                fadingOut = false;
                fadingOutTime = 0;
            }
        }
    }

    void OnPauseReverse() {
        if (!TimeEventManager.isPaused) {
            if (TimeEventManager.isReversed) {
                SwitchToReverse(normalPitch);
            }
            else {
                SwitchToForward(normalPitch);
            }
        }
        else {
            if (TimeEventManager.isReversed) {
                SwitchToReverse(pausedPitch);
            }
            else {
                SwitchToForward(pausedPitch);
            }
        }
    }

    void SwitchToForward(float pitch) {
        if (reversedMusic.isPlaying) {
            reversedMusic.Pause();
            music.Play();
            music.time = reversedMusic.clip.length - reversedMusic.time;
        }
        music.pitch = pitch;
    }

    void SwitchToReverse(float pitch) {
        if (music.isPlaying) {
            music.Pause();
            reversedMusic.Play();
            reversedMusic.time = music.clip.length - music.time;
        }
        reversedMusic.pitch = pitch;
    }
}