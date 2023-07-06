using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftDisplay : MonoBehaviour {
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform area;
    [SerializeField] private Transform sphere;
    private float radius;

    public void CenterConnectionPoints() {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
    }

    public void SetConnectionEndPoint(Vector3 connectionEndDir) {
        lineRenderer.SetPosition(0, transform.position);
        var dir = new Vector3(connectionEndDir.x * radius, 0, connectionEndDir.z * radius);
        var newEnd = transform.position + dir;
        lineRenderer.SetPosition(1, newEnd);
    }

    public void SetConnectionEndPointAbsolute(Vector3 target) {
        lineRenderer.SetPosition(0, transform.position);
        var newEnd = target + (Vector3.up);
        lineRenderer.SetPosition(1, newEnd);
    }

    public void SetAreaRadius(float newRadius) {
        radius = newRadius;
        area.localScale = new Vector3(radius * 2, radius * 2, 1);
        sphere.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
    }
}
