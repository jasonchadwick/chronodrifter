using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

//TODO: give dropper an "acceleration field" inside of it
class SquareDropper : ActivatedObject {
    public GameObject squarePrefab;
    public float force;
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

    public override void ChildStart() {
        square = null;
        squareRenderer = null;
        squareLight = null;
        squareLifetime = spawnInterval - startDelay;
        GetComponentInChildren<AreaEffector2D>().forceMagnitude = force;
    }

    public override void ChildFixedUpdate() {
        if (isActive && !TimeEventManager.isPaused) {
            if (TimeEventManager.isReversed) {
                if (squareLifetime > 0) {
                    squareLifetime -= Time.fixedDeltaTime;
                }
                else {
                    squareLifetime = 0;
                }
            }
            else if (!TimeEventManager.isReversed) {
                if (squareLifetime >= spawnInterval + delay) {
                    if (Time.timeSinceLevelLoad > 0.05) {
                        dropSound.Play();
                    }
                    if (square != null && squareRenderer != null && squareLight != null) {
                        square.transform.position = transform.position;
                        square.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        square.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
                        square.transform.rotation = transform.rotation;
                        squareRenderer.color = oldColor;
                        squareLight.intensity = oldIntensity;
                    }
                    else {
                        square = Instantiate(squarePrefab, transform.position, transform.rotation);
                        squareRenderer = square.GetComponent<SpriteRenderer>();
                        oldColor = squareRenderer.color;
                        goalColor = new Color(oldColor.r, oldColor.g, oldColor.b, 0);
                        square.GetComponent<TimeReversibleRigidbody>().restoringForce = true;
                        squareLight = square.GetComponentInChildren<Light2D>();
                        oldIntensity = squareLight.intensity;
                    }
                    squareLifetime = 0;
                }
                else if (squareLifetime < accelTime && square != null) {
                    //square.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0, -initVelocity));
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