using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLinkSpring {
    public float velocity;
    public float height;
    public float force;
    public float stiffness; //0.5f
    public float decay; //0.2f

    public ChainLinkSpring(float stiffness_, float decay_) { 
        stiffness = stiffness_;
        decay = decay_;
    }

    public void UpdateElement() {
        force = -stiffness * height;
        velocity += force;
        velocity *= decay;
        height += velocity;
    }

    public void SetForce(float force_) { 
        velocity = force_;
    }
}
