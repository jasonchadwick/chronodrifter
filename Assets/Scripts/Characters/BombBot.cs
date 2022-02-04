using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/* Rolls towards player once spotted, then explodes.
*/

class BombBot : Character {
    private GameObject target;
    private Rigidbody2D rb2D;
    private Transform eyeTransform;
    private ParticleSystem explosion;
    private bool isExploding;
    private Light2D eyeLight;

    public float rollForce;
    public float explodeDistance;
    public float killDistance;
    public float explodeTime;

    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        eyeTransform = transform.GetChild(0);
        explosion = GetComponentInChildren<ParticleSystem>();
        eyeLight = GetComponentInChildren<Light2D>();
    }

    void FixedUpdate() {
        if (target == null) {
            target = GameObject.FindWithTag("Player");
        }
        else if (isExploding) {
            if (explosion.isStopped) {
                Kill();
            }
        }
        else {
            Vector3 direction = target.transform.position - transform.position;
            // TODO: if player is visible, bright red light. else dim red light.
            if (direction.magnitude < explodeDistance) {
                Explode();
            }
            
            rb2D.AddForce(new Vector3(direction.normalized.x * rollForce, 0, 0));
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            eyeTransform.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    void Explode() {
        rb2D.velocity = Vector3.zero;
        isExploding = true;
        explosion.Play();
        if (target != null) {
            target.GetComponent<Player>().Kill();
        }
        Destroy(GetComponent<SpriteRenderer>());
        Destroy(eyeTransform.gameObject);
    }
}