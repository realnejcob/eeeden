using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Interactor : MonoBehaviour {
    public bool CanInteract { get; set; } = true;
    [SerializeField] private Transform overlapPoint;
    [SerializeField] private float overlapRadius;
    [SerializeField] private LayerMask mask;

    public int overlappedCount = 0;
    private List<IInteractable> overlappedInteractables = new List<IInteractable>();

    private void Update() {
        CheckForInteractables();
        CheckForInput();

    }

    private void CheckForInteractables() {
        overlappedInteractables.Clear();
        var colliders = Physics.OverlapSphere(overlapPoint.position, overlapRadius, mask).ToList();
        foreach (var col in colliders) {
            if (col.isTrigger) {
                var curInteractable = col.GetComponent<IInteractable>();
                overlappedInteractables.Add(curInteractable);
            }
        }

        overlappedCount = overlappedInteractables.Count;
    }

    private void CheckForInput() {
        if (InputManager.Instance.GetGamepad().buttonSouth.wasPressedThisFrame) {
            if (overlappedInteractables.Count > 0) {
                overlappedInteractables[0].TryInteract(this);
            }
        }
    }

    private void OnDrawGizmos() {
        if (overlapPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(overlapPoint.position, overlapRadius);
    }
}
