using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peg : MonoBehaviour, IInteractable {
    public Transform Anchor { get { return anchor; }}
    [SerializeField] private Transform anchor;
    [SerializeField] private GameObject hoverIndicator;

    private void Start() {
        Initialize();
    }

    private void Initialize() {
        hoverIndicator.SetActive(false);
    }

    public bool PressInteract(Interactor interactor) {
        PegConnectionManager.Instance.AddToCurrentPeg(this);
        return true;
    }

    public bool PressInteractAlt(Interactor interactor) {
        return false;
    }

    public bool LongPressInteract(Interactor interactor) {
        return false;
    }

    public bool HoverEnable(Interactor interactor) {
        hoverIndicator.SetActive(true);
        return false;
    }

    public bool HoverDisable(Interactor interactor) {
        hoverIndicator.SetActive(false);
        return false;
    }
}
