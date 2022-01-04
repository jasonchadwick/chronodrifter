using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

// an object with a distinct lifetime
public class DespawningSquare : TimeReversibleObject<DespawningSquareState> {
    // from TimeReversibleRigidbody
    protected Rigidbody2D rb2D;
    private bool isKinematic;
    private Vector2 pausedVelocity;
    private float pausedAngularVelocity;
    private bool pausedReverse;

    public bool restoringForce = true;
    public float restoringLerp = 10.0f;

    private Light2D squareLight;
    private SpriteRenderer rendererObject;
    private Collider2D boxCollider;
    private ShadowCaster2D shadows;
    private Vector2 spawnPos = new Vector2(0, 0);
    private float spawnAngle = 0.0f;
    private Color spawnColor;
    private float spawnIntensity;
    private Color endColor;

    public float elapsedLifetime;
    public float lifetime;
    public float fadeTime;

    public override void ChildStart()
    {
        // from TimeReversibleRigidbody
        rb2D = GetComponent<Rigidbody2D>();
        isKinematic = rb2D.isKinematic;
        TimeEventManager.OnPause += UpdateOnPause;
        TimeEventManager.OnReverse += UpdateOnReverse;

        squareLight = GetComponentInChildren<Light2D>();
        rendererObject = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<Collider2D>();
        shadows = GetComponent<ShadowCaster2D>();
        spawnPos = Utils.Vector3to2(transform.position);
        spawnColor = rendererObject.color;
        spawnIntensity = squareLight.intensity;
        endColor = new Color(spawnColor.r, spawnColor.g, spawnColor.b, 0);
    }

    public void Respawn(Vector2? pos = null, float angle = 720.0f) {
        if (pos == null) {
            pos = spawnPos;
        }
        if (angle == 720.0f) {
            angle = spawnAngle;
        }
        elapsedLifetime = 0;
        transform.position = (Vector3) pos;
        rb2D.velocity = Vector2.zero;
        rb2D.angularVelocity = 0.0f;
        transform.eulerAngles = new Vector3(0, 0, angle);
        rendererObject.color = spawnColor;
        squareLight.intensity = spawnIntensity;
        boxCollider.enabled = true;
        shadows.castsShadows = true;
    }

    public override DespawningSquareState GetCurrentState()
    {
        return new DespawningSquareState(transform.position, transform.eulerAngles.z, rb2D.velocity, rb2D.angularVelocity, rendererObject.color, squareLight.intensity, elapsedLifetime, shadows.castsShadows);
    }

    // GetStateDifference same as parent

    public override void UpdateObjectState(DespawningSquareState state)
    {
        if (restoringForce && (Utils.Vector3to2(transform.position) - state.position).magnitude < 2) {
            transform.position = Vector3.Lerp(transform.position, state.position, restoringLerp*Time.fixedDeltaTime);
            transform.eulerAngles = new Vector3(0, 0, Utils.AngleLerp(transform.eulerAngles.z, state.angle, restoringLerp*Time.fixedDeltaTime));
            rb2D.velocity = Vector3.Lerp(rb2D.velocity, -state.velocity, restoringLerp*Time.fixedDeltaTime);
            rb2D.angularVelocity = Mathf.Lerp(rb2D.angularVelocity, -state.angularVelocity, restoringLerp*Time.fixedDeltaTime);
        }
        else {
            transform.position = state.position;
            transform.eulerAngles = new Vector3(0, 0, state.angle);
            rb2D.velocity = -state.velocity;
            rb2D.angularVelocity = -state.angularVelocity;
        }

        rendererObject.color = state.color;
        squareLight.intensity = state.intensity;
        elapsedLifetime = state.lifetime;
        boxCollider.enabled = state.shadowsEnabled;
        shadows.castsShadows = state.shadowsEnabled;
    }

    public override void ChildFixedUpdate() {
        if (!TimeEventManager.isReversed) {
            elapsedLifetime += Time.fixedDeltaTime / TimeEventManager.curSlowFactor;
            if (lifetime - elapsedLifetime <= fadeTime && elapsedLifetime < lifetime) {
                float lerp = 1 - (lifetime - elapsedLifetime) / fadeTime;
                rendererObject.color = Color.Lerp(spawnColor, endColor, lerp);
                squareLight.intensity = Mathf.Lerp(spawnIntensity, 0, lerp);
            }
            else if (elapsedLifetime >= lifetime) {
                rendererObject.color = endColor;
                squareLight.intensity = 0;
                boxCollider.enabled = false;
                shadows.castsShadows = false;
            }
        }
    }

    public override float GetStateDifference(DespawningSquareState state1, DespawningSquareState state2) {

        return (state1.position - state2.position).magnitude
             + (state1.velocity - state2.velocity).magnitude
             + Mathf.Abs(state1.angle - state2.angle) * Mathf.Deg2Rad
             + Mathf.Pow(Mathf.Abs(state1.lifetime - state2.lifetime), 4);
    }

    public override DespawningSquareState StateLerp(DespawningSquareState state1, DespawningSquareState state2, float f) {
        Vector2 newPos = Vector2.Lerp(state1.position, state2.position, f);
        float newA = Mathf.LerpAngle(state1.angle, state2.angle, f);
        Vector2 newV = Vector2.Lerp(state1.velocity, state2.velocity, f);
        float newW = Mathf.Lerp(state1.angularVelocity, state2.angularVelocity, f);
        Color newC = Color.Lerp(state1.color, state2.color, f);
        float newI = Mathf.Lerp(state1.intensity, state2.intensity, f);
        float newL = Mathf.Lerp(state1.lifetime, state2.lifetime, f);
        bool newS = f < 0.5 ? state1.shadowsEnabled : state2.shadowsEnabled;

        return new DespawningSquareState(newPos, newA, newV, newW,
                                         newC, newI, newL, newS);
    }

    public override DespawningSquareState SlowDownState(DespawningSquareState state)
    {
        return new DespawningSquareState(state.position, state.angle, state.velocity / TimeEventManager.slowFactor, state.angularVelocity / TimeEventManager.slowFactor, state.color, state.intensity, state.lifetime, state.shadowsEnabled);
    }

    public override DespawningSquareState SpeedUpState(DespawningSquareState state)
    {
        return new DespawningSquareState(state.position, state.angle, state.velocity * TimeEventManager.slowFactor, state.angularVelocity * TimeEventManager.slowFactor, state.color, state.intensity, state.lifetime, state.shadowsEnabled);
    }

    void UpdateOnPause() {
        if (TimeEventManager.isPaused) {
            if (float.IsPositiveInfinity(TimeEventManager.slowFactor)) {
                rb2D.isKinematic = true;

                pausedVelocity = rb2D.velocity;
                pausedAngularVelocity = rb2D.angularVelocity;

                rb2D.velocity = new Vector2(0, 0);
                rb2D.angularVelocity = 0.0f;
            }
            else {
                rb2D.mass *= TimeEventManager.slowFactor;
                rb2D.velocity /= TimeEventManager.slowFactor;
                rb2D.angularVelocity /= TimeEventManager.slowFactor;
                rb2D.gravityScale /= TimeEventManager.slowFactor * TimeEventManager.slowFactor;
            }            
        }
        else {
            if (float.IsPositiveInfinity(TimeEventManager.slowFactor)) {

                rb2D.isKinematic = false;
                // restore velocity values
                rb2D.velocity = pausedVelocity;
                rb2D.angularVelocity = pausedAngularVelocity;
            }
            else {
                rb2D.mass /= TimeEventManager.slowFactor;
                rb2D.velocity *= TimeEventManager.slowFactor;
                rb2D.angularVelocity *= TimeEventManager.slowFactor;
                rb2D.gravityScale *= TimeEventManager.slowFactor * TimeEventManager.slowFactor;
            }
        }
    }

    void UpdateOnReverse() {
        rb2D.velocity *= -1;
        rb2D.angularVelocity *= -1;
        pausedVelocity *= -1;
        pausedAngularVelocity *= -1;
    }

    void OnDisable() {
        TimeEventManager.OnPause -= UpdateOnPause;
        TimeEventManager.OnReverse -= UpdateOnReverse;
    }
}