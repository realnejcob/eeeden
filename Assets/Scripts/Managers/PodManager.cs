using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodManager : MonoBehaviour {
    public static PodManager Instance;
    [SerializeField] private Shiftable activePod;
    [SerializeField] private CameraFollow cameraFollow;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        SetActivePod(activePod);
    }

    public void SetActivePod(Shiftable newActivePod) {
        if (activePod != null) {
            activePod.DisablePod();
            activePod = null;
        }

        newActivePod.EnablePod();
        cameraFollow.SetFollowTarget(newActivePod.transform);

        activePod = newActivePod;
    }

    public CameraFollow GetCameraFollow() {
        return cameraFollow;
    }

    public Shiftable GetActivePod() {
        return activePod;
    }
}
