using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peg : MonoBehaviour, IInteractable {
    public Transform Anchor { get { return anchor; }}
    [SerializeField] private Transform anchor;
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject hoverIndicator;

    public List<CurveChain> connectedCurveChains { get; private set; } = new List<CurveChain>();

    private float tuneAnimationBaseSpeed = 0.5f;
    private bool canTune = true;

    private void Start() {
        Initialize();
    }

    private void Initialize() {
        hoverIndicator.SetActive(false);
    }

    public void AddCurveChain(CurveChain curveChain) {
        if (connectedCurveChains.Contains(curveChain))
            return;

        connectedCurveChains.Add(curveChain);
    }

    public void RemoveCurveChain(CurveChain curveChain) {
        if (!connectedCurveChains.Contains(curveChain))
            return;

        connectedCurveChains.Remove(curveChain);
    }

    private void TuneUp() {
        Tune(1);
    }

    private void TuneDown() {
        Tune(-1);
    }

    private void Tune(int step) {
        if (!canTune)
            return;

        var degrees = step * 45;
        TuneAnimation(degrees, Mathf.Abs(step) * tuneAnimationBaseSpeed);

        if (connectedCurveChains.Count == 0)
            return;

        foreach (var curveChain in connectedCurveChains) {
            curveChain.Pitch(step);
        }

        for (int i = connectedCurveChains.Count-1; i >= 0; i--) {
            var connectedChain = connectedCurveChains[i];
            if (connectedChain.ExceedsPitchLimit()) {
                connectedCurveChains.RemoveAt(i);
                PegConnectionManager.Instance.RemoveConnectionByCurveChain(connectedChain);
            }
        }

        foreach (var curveChain in connectedCurveChains) {
            curveChain.SetPitchColor();
            curveChain.DebugPing();
        }
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
        PegConnectionManager.Instance.ConfigurePeg(this);
        return true;
    }

    public bool OnEast(Interactor interactor) {
        return false;
    }

    public bool OnLongSouth(Interactor interactor) {
        return false;
    }

    public bool OnHoverEnable(Interactor interactor) {
        PegConnectionManager.Instance.OnPegHoverEnter?.Invoke(this);

        hoverIndicator.SetActive(true);
        return true;
    }

    public bool OnHoverDisable(Interactor interactor) {
        PegConnectionManager.Instance.OnPegHoverExit?.Invoke(this);

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
