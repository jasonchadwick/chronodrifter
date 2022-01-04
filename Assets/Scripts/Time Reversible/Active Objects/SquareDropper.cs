using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

class SquareDropper : ActivatedObject<DefaultState> {
    public DespawningSquare squarePrefab;
    public float force;
    public float spawnInterval = 5.0f;
    public float fadeTime = 1.0f;
    
    public float startDelay;
    public float delay;
    public AudioSource dropSound;
    private DespawningSquare square;
    private float forwardTimeSinceLevelLoad = 0.0f;
    private bool hasSpawnedSquare = false;

    public override void ChildStart() {
        GetComponentInChildren<AreaEffector2D>().forceMagnitude = force;
        TimeEventManager.OnPause += UpdateOnPause;
    }

    public override void ChildFixedUpdate() {
        if (IsActive()) {
            if (!TimeEventManager.isReversed) {
                if (!hasSpawnedSquare) {
                    forwardTimeSinceLevelLoad += Time.fixedDeltaTime / TimeEventManager.curSlowFactor;

                    if (forwardTimeSinceLevelLoad > startDelay) {
                        dropSound.Play();
                        square = Instantiate(squarePrefab, transform.position, transform.rotation);
                        square.lifetime = spawnInterval;

                        hasSpawnedSquare = true;
                    }
                }
                else if (square.elapsedLifetime >= spawnInterval + delay) {
                    dropSound.Play();
                    square.Respawn();
                }
            }
            else if (!hasSpawnedSquare) {
                forwardTimeSinceLevelLoad -= Time.fixedDeltaTime / TimeEventManager.curSlowFactor;
            }
        }
    }

    void UpdateOnPause() {
        if (!float.IsPositiveInfinity(TimeEventManager.slowFactor)) {
            if (TimeEventManager.isPaused) {
                GetComponentInChildren<AreaEffector2D>().forceMagnitude /= TimeEventManager.slowFactor;
            }
            else {
                GetComponentInChildren<AreaEffector2D>().forceMagnitude *= TimeEventManager.slowFactor;
            }
        }
    }
}