using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

// an object with a distinct lifetime
public class DespawningSquare : TimeReversibleRigidbody {
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
        base.ChildStart();
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
        shadows.enabled = true;
    }

    public override State GetCurrentState()
    {
        return new DespawningSquareState(transform, rb2D, rendererObject.color, squareLight.intensity, elapsedLifetime, shadows.enabled);
    }

    // GetStateDifference same as parent

    public override void UpdateObjectState(State s)
    {
        base.UpdateObjectState(s);
        DespawningSquareState state = (DespawningSquareState) s;
        rendererObject.color = state.color;
        squareLight.intensity = state.intensity;
        elapsedLifetime = state.lifetime;
        shadows.enabled = state.shadowsEnabled;
    }

    public override void ChildFixedUpdate() {
        if (!TimeEventManager.isPaused && !TimeEventManager.isReversed) {
            elapsedLifetime += Time.fixedDeltaTime;
            if (lifetime - elapsedLifetime <= fadeTime && elapsedLifetime < lifetime) {
                float lerp = 1 - (lifetime - elapsedLifetime) / fadeTime;
                rendererObject.color = Color.Lerp(spawnColor, endColor, lerp);
                squareLight.intensity = Mathf.Lerp(spawnIntensity, 0, lerp);
            }
            else if (elapsedLifetime >= lifetime) {
                rendererObject.color = endColor;
                squareLight.intensity = 0;
                boxCollider.enabled = false;
                shadows.enabled = false;
            }
        }
    }
}

public class DespawningSquareState : Rigidbody2DState {
    public Color color;
    public float intensity;
    public float lifetime;
    public bool shadowsEnabled;

    public DespawningSquareState(Transform transform, Rigidbody2D rb, Color c, float i, float lt, bool s) : base(transform, rb) {
        color = c;
        intensity = i;
        lifetime = lt;
        shadowsEnabled = s;
    }
}