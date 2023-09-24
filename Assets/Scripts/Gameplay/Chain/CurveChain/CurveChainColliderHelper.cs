using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveChainColliderHelper : MonoBehaviour {
    public bool IsTouching { get;private set; }
    private string comparedTag = "StringDisplacementCollider";
    private Collider boxCollider;

    public Vector3 collisionPoint;
    public Vector3 enterDirection;

    private void Awake() {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(comparedTag)) {
            IsTouching = true;

            var hit = boxCollider.ClosestPoint(other.transform.position);
            enterDirection = (hit - other.transform.position) * 1.25f;
        }

    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag(comparedTag)) {
            collisionPoint = boxCollider.ClosestPoint(other.transform.position);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag(comparedTag)) {
            IsTouching = false;

            var hit = boxCollider.ClosestPoint(other.transform.position);
            var exitDirection = hit - other.transform.position;


            if (CheckWillBreak(enterDirection, exitDirection)) {
                PegConnectionManager.Instance.DeconfigurePeg(transform.parent.parent.GetComponent<Peg>());
            }

            collisionPoint = Vector3.zero;
            enterDirection = Vector3.zero;
        }
    }

    private bool CheckWillBreak(Vector3 enterDir, Vector3 exitDir) {
        if (enterDir.x > 0 && exitDir.x < 0 || enterDir.x < 0 && exitDir.x > 0) {
            return true;
        }

        return false;
    }

    private void OnDrawGizmos() {
        if (collisionPoint == Vector3.zero)
            return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(collisionPoint, 0.1f);
    }
}
