using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegAnimator : MonoBehaviour {
    private float rayStartOffset = 1;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Transform leftLegTarget;
    [SerializeField] private Transform rightLegTarget;

    private void FixedUpdate() {
        leftLegTarget.position = new Vector3(leftLegTarget.position.x, GetGroundContact(leftLegTarget).y, leftLegTarget.position.z);
        rightLegTarget.position = new Vector3(rightLegTarget.position.x, GetGroundContact(rightLegTarget).y, rightLegTarget.position.z);
    }

    private Vector3 GetGroundContact(Transform target) {
        var contactPoint = target.position;

        var ray = new Ray(target.position + (Vector3.up * rayStartOffset), Vector3.down);
        if (Physics.Raycast(ray, out var hit, float.MaxValue, groundLayerMask)) {
            contactPoint = hit.point;
        }

        return contactPoint;
    }
}
