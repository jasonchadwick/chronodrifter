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

    public override void ChildStart() {
        square = Instantiate(squarePrefab, transform.position, transform.rotation);
        square.respawns = true;
        square.lifetime = spawnInterval;
        GetComponentInChildren<AreaEffector2D>().forceMagnitude = force;
    }

    public override void ChildFixedUpdate() {
        if (isActive && !TimeEventManager.isPaused) {
            if (!TimeEventManager.isReversed) {
                if (square.elapsedLifetime >= spawnInterval + delay) {
                    if (Time.timeSinceLevelLoad > 0.05) {
                        dropSound.Play();
                    }
                    square.Respawn();
                }
            }
        }
    }
}