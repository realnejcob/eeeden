using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    public static InputManager Instance;

    Gamepad gamepad;
    public Vector2 LeftStickRaw { get; private set; }
    public Vector2 RightStickRaw { get; private set; }

    [SerializeField] private Camera cam;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        gamepad = InputSystem.GetDevice<Gamepad>();
    }

    private void Update() {
        LeftStickRaw = gamepad.leftStick.ReadValue();
        RightStickRaw = gamepad.rightStick.ReadValue();
    }

    public Gamepad GetGamepad() {
        return gamepad;
    }

    public Vector3 GetRelativeInputDir(Vector2 input, bool isNormalized = false) {
        var horInput = input.x;
        var verInput = input.y;

        var camForward = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
        var camRight = new Vector3(cam.transform.right.x, 0, cam.transform.right.z);

        var forwardRelative = verInput * camForward;
        var rightRelative = horInput * camRight;

        var inputDir = (forwardRelative + rightRelative);

        if (isNormalized)
            inputDir = inputDir.normalized;

        return inputDir;
    }
}
