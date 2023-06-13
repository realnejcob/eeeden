using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionIndicator : MonoBehaviour {
    private void Update() {
        var rightStick = InputManager.Instance.RightStickRaw;
        var relativeRightStick = InputManager.Instance.GetRelativeInputDir(rightStick);

        var rotation = Quaternion.LookRotation(transform.parent.forward);

        if (relativeRightStick.magnitude > 0) {
            rotation = Quaternion.LookRotation(relativeRightStick);
        }

        transform.rotation = rotation;
    }
}
