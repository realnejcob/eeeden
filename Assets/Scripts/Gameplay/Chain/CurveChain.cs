using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveChain : ChainBase {
    [Header("Spring Settings:")]
    [SerializeField] private CurveChainPreset preset;

    private List<Vector3> initPositions = new List<Vector3>();

    private float resonanceTime = 0;
    private float amount = 0;

    private LTDescr tween;

    private void Awake() {
        if (!buildOnAwake)
            return;

        BuildChain();
    }

    private void Start() {
        if (autoPlay) {
            AutoPlay();
        }
    }

    public void Update() {
        UpdateChain();
    }

    public override void BuildChain() {
        lineRenderer.positionCount = chainResolution;

        for (int i = 0; i < chainResolution; i++) {
            var increment = (float)1 / (chainResolution - 1) * i;
            var chainPosition = Vector3.Lerp(startAnchor.position, endAnchor.position, increment);
            lineRenderer.SetPosition(i, chainPosition);

            initPositions.Add(chainPosition);
        }
    }

    private void UpdateChain() {
        for (int i = 0; i < chainResolution; i++) {
            var increment = i / (float)(chainResolution-1);
            var newY = (preset.shapeCurve.Evaluate(increment) * preset.maxForce) * preset.movementCurve.Evaluate(resonanceTime) * amount;
            var initPos = initPositions[i];
            var newPos = new Vector3(initPos.x, initPos.y + newY, initPos.z);
            lineRenderer.SetPosition(i, newPos);
        }
    }

    private void AutoPlay() {
        StartCoroutine(Co());

        IEnumerator Co() {
            Ping();
            yield return new WaitForSeconds(preset.duration);
            StartCoroutine(Co());
        }
    }

    private void Ping() {
        if (tween != null) {
            LeanTween.cancel(tween.id);
        }

        tween = LeanTween.value(gameObject, 0, 1, preset.duration).setOnUpdate((float time) => {
            resonanceTime += preset.speed * Time.deltaTime;
            amount = preset.forceCurve.Evaluate(time);
        }).setOnComplete(() => {
            resonanceTime = 0;
        });
    }

    public void SetAnchors(Transform newStartAnchor, Transform newEndAnchor) {
        startAnchor = newStartAnchor;
        endAnchor = newEndAnchor;
    }

    public override void DebugPing() {
        Ping();
    }

    public override void DebugSwell() {
        Ping();
    }
}
