using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

class SquareDropper : ButtonActivatedObject {
    public GameObject squarePrefab;
    public float initVelocity;
    public float spawnInterval = 5.0f;
    public float fadeTime = 1.0f;
    public float accelTime = 0.5f;
    public float startDelay = 0;
    public float delay;
    public AudioSource dropSound;
    private GameObject square;
    private SpriteRenderer squareRenderer;
    private Color oldColor;
    private Color goalColor;
    private Light2D squareLight;
    private float oldIntensity;
    private float reverseLerp;
    private float lastAchievedIntensity;
    private Color lastAchievedColor;
    private float squareLifetime;

    void Start() {
        square = null;
        squareLifetime = spawnInterval - startDelay;
    }

    void FixedUpdate() {
        if (isActive && !TimeEventManager.isPaused) {
            if (TimeEventManager.isReversed) {
                if (!square.GetComponent<TimeReversibleObject>().isFullyReversed) {
                    squareLifetime -= Time.deltaTime;
                }
            }
            else if (!TimeEventManager.isReversed) {
                if (squareLifetime >= spawnInterval + delay) {
                    if (Time.timeSinceLevelLoad > 0.05) {
                        dropSound.Play();
                    }
                    if (square != null) {
                        Destroy(square);
                    }
                    square = Instantiate(squarePrefab, transform.position, transform.rotation);
                    squareLifetime = 0;
                    
                    square.GetComponent<TimeReversibleObject>().restoringForce = true;
                    squareRenderer = square.GetComponent<SpriteRenderer>();
                    oldColor = squareRenderer.color;
                    goalColor = new Color(oldColor.r, oldColor.g, oldColor.b, 0);
                    squareLight = square.GetComponentInChildren<Light2D>();
                    oldIntensity = squareLight.intensity;
                }
                else if (squareLifetime < accelTime && square != null) {
                    square.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0, -initVelocity));
                }
                squareLifetime += Time.fixedDeltaTime;
            }
            if (squareLifetime >= spawnInterval - fadeTime && square != null) {
                float lerp = 1 - (spawnInterval - squareLifetime) / fadeTime;
                squareRenderer.color = Color.Lerp(oldColor, goalColor, lerp);
                squareLight.intensity = Mathf.Lerp(oldIntensity, 0, lerp);
            }
        }
        else if (!isActive && !TimeEventManager.isPaused) {
            squareLifetime = (squareLifetime + Time.deltaTime) % spawnInterval;
        }
    }
}