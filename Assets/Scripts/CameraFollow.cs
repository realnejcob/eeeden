using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField] private Camera cam;
    private float desiredCamSize = 5;
    private float defaultCamSize = 5;
    private float zoomOutCamSize = 6;
    private float currentCamSize;

    private float camSizeZoomTime = 0.25f;
    private float camSizeVel;


    public Transform target;
    private Transform secondaryTarget;
    public Vector3 offset = new Vector3(0, 2, -10);
    public float smoothTime = 0.25f;
    Vector3 currentVelocity;

    private void Start() {
        currentCamSize = defaultCamSize;
        desiredCamSize = currentCamSize;
    }

    private void Update() {
        CheckForInput();
        SetCamSize();
    }

    private void CheckForInput() {
        if (InputManager.Instance.GetGamepad().leftTrigger.value > 0.1f) {
            desiredCamSize = zoomOutCamSize;
        } else {
            desiredCamSize = defaultCamSize;
        }
    }

    private void SetCamSize() {
        currentCamSize = Mathf.SmoothDamp(currentCamSize, desiredCamSize, ref camSizeVel, camSizeZoomTime);
        cam.fieldOfView = currentCamSize;
    }

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
