using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class Interactor : MonoBehaviour {
    public bool CanInteract { get; set; } = true;
    public ButtonControl SouthButton { get; private set; }
    public ButtonControl EastButton { get; private set; }
    public ButtonControl LeftShoulder { get; private set; }
    public ButtonControl RightShoulder { get; private set; }
    public StickControl RightStick { get; private set; }

    [SerializeField] private Transform overlapPoint;
    [SerializeField] private float overlapRadius;
    [SerializeField] private LayerMask mask;

    private bool canRightStickTap = true;
    private float rightStickTapThreshold = 0.9f;

    public int overlappedCount = 0;
    private List<IInteractable> overlappedInteractables = new List<IInteractable>();

    private InputManager inputManager;

    private void Start() {
        inputManager = InputManager.Instance;

        SouthButton = inputManager.GetGamepad().buttonSouth;
        EastButton = inputManager.GetGamepad().buttonEast;
        LeftShoulder = inputManager.GetGamepad().leftShoulder;
        RightShoulder = inputManager.GetGamepad().rightShoulder;

        RightStick = inputManager.GetGamepad().rightStick;
    }

    private void Update() {
        CheckForInteractables();

        if (!CanInteract)
            return;

        CheckInput();
    }

    private void CheckForInteractables() {
        if (overlappedCount > 0) {
            overlappedInteractables[0].HoverDisable(this);
        }

        overlappedInteractables.Clear();

        var colliders = Physics.OverlapSphere(overlapPoint.position, overlapRadius, mask).ToList();
        foreach (var col in colliders) {
            if (col.isTrigger) {
                var curInteractable = col.GetComponent<IInteractable>();
                overlappedInteractables.Add(curInteractable);
            }
        }

        overlappedCount = overlappedInteractables.Count;

        if (overlappedCount > 0) {
            overlappedInteractables[0].HoverEnable(this);
        }
    }

    private void CheckInput() {
        if (!CanInteract)
            return;

        if (!HasInteractable())
            return;

        SouthButtonCheck();
        EastButtonCheck();
        LeftShoulderButtonCheck();
        RightShoulderButtonCheck();

        RightStickTapCheck();
    }

    private void SouthButtonCheck() {
        if (SouthButton.wasPressedThisFrame) {
            var hasInteracted = overlappedInteractables[0].OnSouth(this);
        }
    }

    private void EastButtonCheck() {
        if (EastButton.wasPressedThisFrame) {
            var hasInteracted = overlappedInteractables[0].OnEast(this);
        }
    }

    private void LeftShoulderButtonCheck() {
        if (LeftShoulder.wasPressedThisFrame) {
            var hasInteracted = overlappedInteractables[0].OnLeftShoulder(this);
        }
    }

    private void RightShoulderButtonCheck() {
        if (RightShoulder.wasPressedThisFrame) {
            var hasInteracted = overlappedInteractables[0].OnRightShoulder(this);
        }
    }

    private void RightStickTapCheck() {
        if (inputManager.IsRightStickInDeadZone()) {
            canRightStickTap = true;
        }

        if (!canRightStickTap)
            return;

        var rightStickX = RightStick.ReadValue().x;

        if (rightStickX >= rightStickTapThreshold) {
            canRightStickTap = false;
            var hasInteracted = overlappedInteractables[0].OnRightStickTapRight(this);
        } else if(rightStickX <= -rightStickTapThreshold) {
            canRightStickTap = false;
            var hasInteracted = overlappedInteractables[0].OnRightStickTapLeft(this);
        }
    }

    private bool HasInteractable() {
        if (overlappedInteractables.Count > 0) {
            return true;
        }

        return false;
    }

    private void OnDrawGizmos() {
        if (overlapPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(overlapPoint.position, overlapRadius);
    }
}
