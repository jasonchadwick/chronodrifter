using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

class SquareDropper : MonoBehaviour {
    public GameObject squarePrefab;
    public float spawnInterval = 5.0f;
    public float fadeTime = 1.0f;
    private GameObject square;
    private float nextSpawnTime;
    private SpriteRenderer squareRenderer;
    private Color oldColor;
    private Color goalColor;
    private Light2D squareLight;
    private float oldIntensity;
    private float reverseLerp;
    private float lastAchievedIntensity;
    private Color lastAchievedColor;

    void Start() {
        nextSpawnTime = 0;
    }

    void Update() {
        if (!TimeEventManager.isPaused && !TimeEventManager.isReversed) {
            if (Time.time >= nextSpawnTime) {
                Destroy(square);
                Debug.Log(square);
                Debug.Log(Time.time);
                square = Instantiate(squarePrefab, transform.position, Quaternion.identity);
                nextSpawnTime = Time.time + spawnInterval;
                
                squareRenderer = square.GetComponent<SpriteRenderer>();
                oldColor = squareRenderer.color;
                goalColor = new Color(oldColor.r, oldColor.g, oldColor.b, 0);
                squareLight = square.GetComponentInChildren<Light2D>();
                oldIntensity = squareLight.intensity;
            }
            else if (Time.time >= nextSpawnTime - fadeTime && square != null) {
                float lerp = 1 - ((nextSpawnTime - Time.time) / fadeTime);
                squareRenderer.color = Color.Lerp(oldColor, goalColor, lerp);
                lastAchievedColor = squareRenderer.color;
                squareLight.intensity = Mathf.Lerp(oldIntensity, 0, lerp);
                lastAchievedIntensity = squareLight.intensity;
            }
        }
        else if (TimeEventManager.isPaused) {
            nextSpawnTime += Time.deltaTime;
        }
        else if (TimeEventManager.isReversed) {
            if (squareRenderer.color.a <= 0.99) {
                reverseLerp += Time.deltaTime / fadeTime;
                squareRenderer.color = Color.Lerp(lastAchievedColor, oldColor, reverseLerp);
                squareLight.intensity = Mathf.Lerp(lastAchievedIntensity, oldIntensity, reverseLerp);
            }
            else {
                reverseLerp = 0;
            }
            if ((transform.position - square.transform.position).magnitude > 0.01) {
                // account for time flying up plus time to fall back down
                nextSpawnTime += 2*Time.deltaTime;
            }
            else {
                // just account for time sitting at spawn
                nextSpawnTime += Time.deltaTime;
            }
        }
    }
}