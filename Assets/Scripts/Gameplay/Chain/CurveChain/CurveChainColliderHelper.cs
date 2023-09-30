using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CurveChainColliderHelper : MonoBehaviour {
    public bool IsTouching { get; private set; }
    public bool IsOutOfBounds { get; private set; }

    private string comparedTag = "StringDisplacementCollider";
    private BoxCollider boxCollider;

    public Vector3 collisionPoint;
    public Vector3 enterDirection;

    private Collider connectedCollider;

    public Action OnExitSameSide;
    public Action OnExitOppositeSide;

    private void Awake() {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other) {
        if (IsOutOfBounds)
            return;

        if (other.CompareTag(comparedTag)) {
            IsTouching = true;

            connectedCollider = other;

            enterDirection = GetDirection(connectedCollider);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (connectedCollider!= null) {
            collisionPoint = GetCollisionPoint(connectedCollider);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (connectedCollider != null) {
            var exitDirection = GetDirection(connectedCollider);

            if (GetIsExitingOppositeSide(enterDirection, exitDirection)) {
                OnExitOppositeSide?.Invoke();

                IsOutOfBounds = true;
            } else {
                OnExitSameSide?.Invoke();

                IsTouching = false;
                IsOutOfBounds = false;

                collisionPoint = Vector3.zero;
                enterDirection = Vector3.zero;
                connectedCollider = null;
            }
        }
    }

    private Vector3 GetDirection(Collider _connectedCollider) {
        var direction = GetCollisionPoint(_connectedCollider) - _connectedCollider.transform.position;
        return direction;
    }

    private Vector3 GetCollisionPoint(Collider other) {
        var point = boxCollider.ClosestPoint(other.transform.position);
        return point;
    }

    private bool GetIsExitingOppositeSide(Vector3 enterDir, Vector3 exitDir) {
        if (Vector3.Dot(enterDir, exitDir) < 0) {
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
