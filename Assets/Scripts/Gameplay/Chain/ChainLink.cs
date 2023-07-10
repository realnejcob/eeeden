using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLink : MonoBehaviour {
    public bool IsLocked { get; set; } = false;

    public ChainLinkSpring chainLinkSpring;
    private Vector3 initialPosition;

    public void Initialize(float stiffness, float decay) {
        chainLinkSpring = new ChainLinkSpring(stiffness, decay);
        initialPosition = transform.position;
    }

    private void Update() {
        if (IsLocked)
            return;

        chainLinkSpring.UpdateElement();
        transform.position = initialPosition + (Vector3.up * chainLinkSpring.height);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 0.10f);
    }
}
