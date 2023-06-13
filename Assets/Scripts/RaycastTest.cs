using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTest : MonoBehaviour {
    public float radius = 4;
    private float segments = 9;
    private float spread = 70;

    public Vector3 forwardVector;
    public Vector3 rightStickForwardVector;

    private Vector3 offset = Vector3.up;

    private void OnDrawGizmos() {
        var startingPosition = transform.position + offset;

        forwardVector = transform.forward;

        Gizmos.color = Color.yellow;

        var maxAngle = (int)(spread * 0.5f);
        var minAngle = -maxAngle;

        var increment = (int)(spread / segments);

        for (int angle = minAngle; angle <= maxAngle; angle += increment) {
            var rotation = Quaternion.AngleAxis(angle, transform.right) * forwardVector;
            var endPosition = startingPosition + (rotation * radius);
            Gizmos.DrawLine(startingPosition, endPosition);
        }

    }
}
