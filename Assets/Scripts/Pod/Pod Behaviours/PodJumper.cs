using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GroundRider))]

public class PodJumper : PodBase {
    [Header("Jumper Pod Variables:")]
    [SerializeField] private float jumpHeight = 5;
    [SerializeField] private float jumpTimePerUnit = 0.2f;
    [SerializeField] private AnimationCurve jumpCurve;

    private LTDescr jumpTween = null;

    private void Update() {
        if (IsControlling) {
            if (InputManager.Instance.GetGamepad().aButton.isPressed) {
                if (!IsJumpRunning()) {
                    Jump();
                }
            }
        }
    }

    private bool IsJumpRunning() {
        if (jumpTween == null)
            return false;

        if (LeanTween.isTweening(jumpTween.id))
            return true;

        return false;
    }

    private void Jump() {
        jumpTween = LeanTween.value(gameObject, 0, 1, jumpTimePerUnit * jumpHeight).setOnUpdate((float t) => {
            var newHeight = Mathf.LerpUnclamped(enabledHeight, jumpHeight, jumpCurve.Evaluate(t));
            groundRider.SetHeight(newHeight);
        });
    }
}
