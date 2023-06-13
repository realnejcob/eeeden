using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKLegSolver : MonoBehaviour {
    [SerializeField] private  float stepDistance = 1f;
    private float rayStartOffset = 1f;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Transform body;
    private Vector3 currentPosition;
    private Vector3 oldPosition;
    private Vector3 newPosition;
    private float lerp = 0;
    [SerializeField] private AnimationCurve stepAnimationCurve;
    [SerializeField] private float stepHeight = 0.1f;
    [SerializeField] private float footSpacing = 1;
    [SerializeField] private float speed = 1;

    private void Awake() {
        oldPosition = transform.position;
        newPosition = transform.position;
        currentPosition = transform.position;
    }

    private void Update() {
        var ray = new Ray(body.position + (body.right * footSpacing) + (body.forward * stepDistance) + (Vector3.up * rayStartOffset), Vector3.down);
        if (Physics.Raycast(ray, out var hit, float.MaxValue, groundLayerMask)) {
            if (Vector3.Distance(newPosition, hit.point) > stepDistance * 2) {
                lerp = 0;
                newPosition = hit.point;
            }
        }

        if (lerp < 1) {
            Vector3 footPosition = Vector3.Lerp(oldPosition, newPosition, stepAnimationCurve.Evaluate(lerp));
            footPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = footPosition;
            lerp += Time.deltaTime * speed;
        } else {
            oldPosition = newPosition;
        }

        transform.position = currentPosition;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.1f);
    }
}
