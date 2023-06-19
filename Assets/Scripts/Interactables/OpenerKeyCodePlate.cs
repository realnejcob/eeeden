using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenerKeyCodePlate : Opener {
    public KeyCode key;

    public Color lockColor;
    public Color unlockColor;

    private MeshRenderer meshRenderer;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update() {
        CheckForKeyInput();
    }

    private void CheckForKeyInput() {
        if (Input.GetKeyDown(key)) {
            if (IsUnlocked) {
                Lock();
                return;
            }

            Unlock();
        }
    }

    public override void OnUnlockSuccess() {
        meshRenderer.materials[0].color = unlockColor;
    }

    public override void OnLockSuccess() {
        meshRenderer.materials[0].color = lockColor;
    }
}
