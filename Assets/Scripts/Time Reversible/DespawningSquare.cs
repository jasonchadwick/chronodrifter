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
        return new DespawningSquareState(transform.position, transform.eulerAngles.z, rb2D.velocity, rb2D.angularVelocity, rendererObject.color, squareLight.intensity, elapsedLifetime, shadows.enabled);
    }

    // GetStateDifference same as parent

    public override void UpdateObjectState(State s)
    {
        base.UpdateObjectState(s);
        DespawningSquareState state = s as DespawningSquareState;
        rendererObject.color = state.color;
        squareLight.intensity = state.intensity;
        elapsedLifetime = state.lifetime;
        boxCollider.enabled = state.shadowsEnabled;
        shadows.enabled = state.shadowsEnabled;
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
                shadows.enabled = false;
            }
        }
    }

    public override float GetStateDifference(State s1, State s2) {
        DespawningSquareState state1 = s1 as DespawningSquareState;
        DespawningSquareState state2 = s2 as DespawningSquareState;

        return base.GetStateDifference(s1, s2) + 
               Mathf.Pow(Mathf.Abs(state1.lifetime - state2.lifetime), 4);
    }

    public override State StateLerp(State s1, State s2, float f)
    {
        Rigidbody2DState rb2Dlerp = base.StateLerp(s1, s2, f) as Rigidbody2DState;

        DespawningSquareState state1 = s1 as DespawningSquareState;
        DespawningSquareState state2 = s2 as DespawningSquareState;

        Color newC = Color.Lerp(state1.color, state2.color, f);
        float newI = Mathf.Lerp(state1.intensity, state2.intensity, f);
        float newL = Mathf.Lerp(state1.lifetime, state2.lifetime, f);
        bool newS = f < 0.5 ? state1.shadowsEnabled : state2.shadowsEnabled;

        return new DespawningSquareState(rb2Dlerp.position, rb2Dlerp.angle, rb2Dlerp.velocity, rb2Dlerp.angularVelocity,
                                         newC, newI, newL, newS);
    }

    public override State SlowDownState(State s)
    {
        DespawningSquareState state = s as DespawningSquareState;
        Rigidbody2DState s1 = base.SlowDownState(s) as Rigidbody2DState;
        return new DespawningSquareState(s1.position, s1.angle, s1.velocity, s1.angularVelocity, state.color, state.intensity, state.lifetime, state.shadowsEnabled);
    }

    public override State SpeedUpState(State s)
    {
        DespawningSquareState state = s as DespawningSquareState;
        Rigidbody2DState s1 = base.SpeedUpState(s) as Rigidbody2DState;
        return new DespawningSquareState(s1.position, s1.angle, s1.velocity, s1.angularVelocity, state.color, state.intensity, state.lifetime, state.shadowsEnabled);
    }
}

public class DespawningSquareState : Rigidbody2DState {
    public Color color;
    public float intensity;
    public float lifetime;
    public bool shadowsEnabled;

    public DespawningSquareState(Vector2 pos, float a, Vector2 v, float w, Color c, float i, float lt, bool s) : base(pos, a, v, w) {
        color = c;
        intensity = i;
        lifetime = lt;
        shadowsEnabled = s;
    }
}