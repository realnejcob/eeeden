using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ChainPlacementIndicator : MonoBehaviour {
    [SerializeField] private PegConnectionManager pegConnectionManager;

    private LineRenderer lineRenderer;

    public Vector3 currentStartPosition;
    public Vector3 currentEndPosition;

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        ConfigureEvents();
    }

    private void Update() {
        if (!CanShow()) {
            if (lineRenderer.enabled) {
                lineRenderer.enabled = false;
            }
        } else {
            if (!lineRenderer.enabled) {
                lineRenderer.enabled = true;
            }
        }
    }

    private void ConfigureEvents() {
        pegConnectionManager.OnConfigureStartingPeg += SetStartPosition;
        pegConnectionManager.OnPegHoverEnter += SetEndPosition;
        pegConnectionManager.OnPegHoverExit += ResetEndPosition;
        pegConnectionManager.OnResetCurrentConnections += ResetPositions;
    }

    private bool CanShow() {
        if (currentStartPosition == Vector3.zero || currentEndPosition == Vector3.zero)
            return false;

        return true;
    }

    private void SetStartPosition(Peg newPeg) {
        currentStartPosition = newPeg.Anchor.position;
        lineRenderer.SetPosition(0, currentStartPosition);
    }

    private void SetEndPosition(Peg newPeg) {
        currentEndPosition = newPeg.Anchor.position;
        lineRenderer.SetPosition(1, currentEndPosition);
    }

    private void ResetStartPosition(Peg newPeg) {
        currentStartPosition = Vector3.zero;
        lineRenderer.SetPosition(0, currentStartPosition);
    }

    private void ResetEndPosition(Peg newPeg) {
        currentEndPosition = currentStartPosition;
        lineRenderer.SetPosition(1, currentStartPosition);
    }

    private void ResetPositions() {
        ResetStartPosition(null);
        ResetEndPosition(null);
    }
}
