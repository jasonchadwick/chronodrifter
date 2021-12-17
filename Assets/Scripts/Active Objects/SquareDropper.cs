using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

//TODO: give dropper an "acceleration field" inside of it
class SquareDropper : ActivatedObject {
    public DespawningSquare squarePrefab;
    public float force;
    public float spawnInterval = 5.0f;
    public float fadeTime = 1.0f;
    public float accelTime = 0.5f;
    
    // currently not working
    public float startDelay = 0;
    public float delay;
    public AudioSource dropSound;
    private DespawningSquare square;
    private float forwardTimeSinceLevelLoad = 0.0f;
    private bool hasSpawnedSquare = false;

    public override void ChildStart() {
        GetComponentInChildren<AreaEffector2D>().forceMagnitude = force;
    }

    public override void ChildFixedUpdate() {
        if (isActive && !TimeEventManager.isPaused) {
            if (!TimeEventManager.isReversed) {
                if (!hasSpawnedSquare) {
                    forwardTimeSinceLevelLoad += Time.fixedDeltaTime;

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
                forwardTimeSinceLevelLoad -= Time.fixedDeltaTime;
            }
        }
    }
}