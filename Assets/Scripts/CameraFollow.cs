using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Transform secondaryTarget;
    public Vector3 offset = new Vector3(0, 2, -10);
    public float smoothTime = 0.25f;
    Vector3 currentVelocity;

    private void FixedUpdate() {
        var targetPosition = target.position;
        if (secondaryTarget != null) {
            targetPosition = (target.position + secondaryTarget.position) / 2;
        }

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition + offset,
            ref currentVelocity,
            smoothTime
            );
    }

    public void SetFollowTarget(Transform newTarget) {
        target = newTarget;
    }

    public void SetSecondaryFollowTarget(Transform newTarget) {
        secondaryTarget = newTarget;
    }
}
