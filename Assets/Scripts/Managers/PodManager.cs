using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodManager : MonoBehaviour {
    public static PodManager Instance;
    [SerializeField] private PodBase activePod;
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

    public void SetActivePod(PodBase newActivePod) {
        if (activePod != null) {
            activePod.DisablePod();
            activePod = null;
        }

        newActivePod.EnablePod();
        cameraFollow.SetFollowTarget(newActivePod.transform);

        activePod = newActivePod;
    }
}
