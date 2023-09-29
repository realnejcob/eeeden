using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveChainColliderHelper : MonoBehaviour {
    public bool IsTouching { get;private set; }
    private string comparedTag = "StringDisplacementCollider";
    private BoxCollider boxCollider;

    public Vector3 collisionPoint;
    public Vector3 enterDirection;

    public bool isOutOfBounds = false;

    private void Awake() {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(comparedTag)) {
            IsTouching = true;

            var hit = GetCollisionPoint(other);
            enterDirection = (hit - other.transform.position);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag(comparedTag)) {
            collisionPoint = GetCollisionPoint(other);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag(comparedTag)) {
            IsTouching = false;

            var hit = GetCollisionPoint(other);
            var exitDirection = hit - other.transform.position;


            print(enterDirection);
            print(exitDirection);

            if (GetIsExitingOppositeSide(enterDirection, exitDirection)) {
                PegConnectionManager.Instance.DeconfigurePeg(transform.parent.parent.GetComponent<Peg>());
            }

            collisionPoint = Vector3.zero;
            enterDirection = Vector3.zero;
        }
    }

    private Vector3 GetCollisionPoint(Collider other) {
        var point = boxCollider.ClosestPoint(other.transform.position);
        return point;
    }

    private bool GetIsExitingOppositeSide(Vector3 enterDir, Vector3 exitDir) {
        if (Vector3.Dot(enterDir,exitDir) < 0) {
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
