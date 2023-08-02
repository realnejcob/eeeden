using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peg : MonoBehaviour, IInteractable {
    public bool IsInUse { get; private set; } = false;
    public Transform Anchor { get { return anchor; }}
    [SerializeField] private Transform anchor;
    [SerializeField] private List<GameObject> inUseIndicators = new List<GameObject>();
    private GameObject currentInUseIndicator;

    public void SetInUse() {
        if (IsInUse)
            return;

        IsInUse = true;
        currentInUseIndicator = GetRandomInUseIndicator();
        currentInUseIndicator.SetActive(true);
    }

    public void SetNotInUse() {
        if (!IsInUse)
            return;

        IsInUse = false;
        currentInUseIndicator.SetActive(false);
        currentInUseIndicator = null;
    }

    private GameObject GetRandomInUseIndicator() {
        var randomIdx = Random.Range(0, inUseIndicators.Count);
        return inUseIndicators[randomIdx];
    }

    public bool TryInteract(Interactor interactor) {
        if (!IsInUse) {
            SetInUse();
            PegConnectionManager.Instance.ConfigurePeg(this);
        } else {
            return false;
            SetNotInUse();
        }

        return true;
    }
}
