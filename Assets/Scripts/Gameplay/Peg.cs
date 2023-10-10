using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peg : MonoBehaviour, IInteractable {
    public Transform Anchor { get { return anchor; }}
    [SerializeField] private Transform anchor;
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject hoverIndicator;

    private float tuneAnimationBaseSpeed = 0.5f;
    private bool canTune = true;

    private void Start() {
        Initialize();
    }

    private void Initialize() {
        hoverIndicator.SetActive(false);
    }

    private void TuneUp() {
        Tune(1);
    }

    private void TuneDown() {
        Tune(-1);
    }

    private void Tune(int steps) {
        if (!canTune)
            return;

        // Tune connected strings

        var degrees = steps * (45 + Random.Range(5, 10));
        TuneAnimation(degrees, Mathf.Abs(steps) * tuneAnimationBaseSpeed);
    }

    private LTDescr TuneAnimation(float degrees, float speed) {
        canTune = false;
        var initRot = model.transform.rotation.eulerAngles;
        var tween = LeanTween.value(gameObject, 0f, 1f, speed).setOnUpdate((float t) => {
            var newY = initRot.y + t * degrees;
            model.transform.rotation = Quaternion.Euler(initRot.x, newY, initRot.z);
        })
            .setEaseOutQuint()
            .setOnComplete(() => {
                canTune = true;
            });

        return tween;
    }

    #region INTERACTIONS

    public bool OnSouth(Interactor interactor) {
        PegConnectionManager.Instance.AddToCurrentPeg(this);
        return true;
    }

    public bool OnEast(Interactor interactor) {
        return false;
    }

    public bool OnLongSouth(Interactor interactor) {
        return false;
    }

    public bool HoverEnable(Interactor interactor) {
        hoverIndicator.SetActive(true);
        return true;
    }

    public bool HoverDisable(Interactor interactor) {
        hoverIndicator.SetActive(false);
        return true;
    }

    public bool OnLeftShoulder(Interactor interactor) {
        return false;
    }

    public bool OnRightShoulder(Interactor interactor) {
        return false;
    }

    public bool OnRightStickTapRight(Interactor interactor) {
        if (!interactor.LeftShoulder.isPressed)
            return false;

        TuneUp();
        return true;
    }

    public bool OnRightStickTapLeft(Interactor interactor) {
        if (!interactor.LeftShoulder.isPressed)
            return false;

        TuneDown();
        return true;
    }
    
    #endregion
}
