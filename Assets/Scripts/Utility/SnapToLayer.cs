using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
public class SnapToLayer : MonoBehaviour {
    [SerializeField] private LayerMask layer;
    [SerializeField] private float offset = 0;

    public void Snap() {
        Ray ray = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(ray, out var hit, 500, layer)) {
            if (hit.collider.gameObject != null) {
                var point = hit.point;
                transform.position = new Vector3(transform.position.x, point.y + offset, transform.position.z);
                return;
            }

            Debug.LogWarning("Can't snap!");
        }
    }
}
