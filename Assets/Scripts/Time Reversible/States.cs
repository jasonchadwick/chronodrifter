using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

// child classes of TimeReversibleObject will use a specific State structure.
public interface State {
    // Number of FixedUpdate iters for which the object has been in this State.
    int numIntervals {
        get;
        set;
    }

    public void Increment();
    public void Decrement();
}

// default for when no time reversibility is needed
public struct DefaultState : State {
    public int numIntervals {get; set;}
    public void Increment() {
        numIntervals++;
    }
    public void Decrement() {
        numIntervals--;
    }
}

// used by TimeGhost and MovingPillar
public struct PositionState : State {
    public Vector2 position;

    public PositionState(Vector2 p) {
        numIntervals = 1;
        position = p;
    }

    public int numIntervals {get; set;}
    public void Increment() {
        numIntervals++;
    }
    public void Decrement() {
        numIntervals--;
    }
}

public struct PillarState : State {
    public Vector2 position;
    public Vector2 velocity;
    public float intensity;

    public PillarState(Vector2 p, Vector2 v, float i) {
        numIntervals = 1;
        position = p;
        velocity = v;
        intensity = i;
    }

    public int numIntervals {get; set;}
    public void Increment() {
        numIntervals++;
    }
    public void Decrement() {
        numIntervals--;
    }
}

// used by TimeReversibleRigidbody
public struct Rigidbody2DState : State {
    public Vector2 position;

    // third Euler angle (the only one we need for 2D)
    public float angle;
    public Vector2 velocity;
    public float angularVelocity;

    public Rigidbody2DState (Vector2 pos, float a, Vector2 v, float w) {
        numIntervals = 1;
        position = pos;
        angle = a;
        velocity = v;
        angularVelocity = w;
    }

    public int numIntervals {get; set;}
    public void Increment() {
        numIntervals++;
    }
    public void Decrement() {
        numIntervals--;
    }
}

// used by DespawningSquare
public struct DespawningSquareState : State {
    public Vector2 position;

    // third Euler angle (the only one we need for 2D)
    public float angle;
    public Vector2 velocity;
    public float angularVelocity;
    public Color color;
    public float intensity;
    public float lifetime;
    public bool shadowsEnabled;

    public DespawningSquareState(Vector2 pos, float a, Vector2 v, float w, Color c, float i, float lt, bool s) {
        numIntervals = 1;
        position = pos;
        angle = a;
        velocity = v;
        angularVelocity = w;
        color = c;
        intensity = i;
        lifetime = lt;
        shadowsEnabled = s;
    }

    public int numIntervals {get; set;}
    public void Increment() {
        numIntervals++;
    }
    public void Decrement() {
        numIntervals--;
    }
}

// used by PressurePlate
public struct FloatState : State {
    public float f;

    public FloatState(float num) {
        numIntervals = 1;
        f = num;
    }

    public int numIntervals {get; set;}
    public void Increment() {
        numIntervals++;
    }
    public void Decrement() {
        numIntervals--;
    }
}