using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GroundRider : MonoBehaviour {
    [SerializeField] private float rideHeight = 0.525f;
    private float slopeDownForce;
    [SerializeField] private float rideSpringStrength = 250;
    [SerializeField] private float rideSpringDamper = 1;

    [SerializeField] private float rayLength = 5;
    private LayerMask isGroundMask;

    private Rigidbody rb;

    [SerializeField] private float maxSlopeDownForce = 0.1f;
    private float slopeAngle;
    private bool isMoving = false;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        isGroundMask = LayerMask.GetMask("Ground");
    }

    private void FixedUpdate() {
        GroundRide();
        AddSlopeDownForce();
    }

    private void GroundRide() {
        var rayOrigin = transform.position + Vector3.up;
        var ray = new Ray(rayOrigin, Vector3.down);

        if (Physics.Raycast(ray, out var hit, rayLength, isGroundMask)) {
            slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

            var vel = rb.velocity;
            var rayDir = transform.TransformDirection(Vector3.down);

            var otherVel = Vector3.zero;

            var rayDirVel = Vector3.Dot(rayDir, vel);
            var otherDirVel = Vector3.Dot(rayDir, otherVel);
            var relVel = rayDirVel - otherDirVel;

            float x = hit.distance - rideHeight + slopeDownForce;
            float springForce = (x * rideSpringStrength) - (relVel * rideSpringDamper);

            rb.AddForce(rayDir * springForce);
        }
    }

    public void SetHeight(float newHeight) {
        rideHeight = newHeight;
    }

    private void AddSlopeDownForce() {
        if (rb.velocity.x + rb.velocity.z == 0) {
            isMoving = false;
        } else {
            isMoving = true;
        }

        if (rb.velocity.y < 0)
            slopeAngle *= -1;

        if (isMoving && slopeAngle < 0) {
            slopeDownForce = maxSlopeDownForce;
        } else {
            slopeDownForce = 0;
        }
    }
}
