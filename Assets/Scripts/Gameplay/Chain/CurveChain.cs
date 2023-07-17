using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveChain : ChainBase {
    [Header("Spring Settings:")]
    [SerializeField] private float maxPingForce = 0.25f;
    [SerializeField] private float pingDuration = 1;
    [SerializeField] private float animationSpeed = 0.25f;
    [SerializeField] private AnimationCurve shapeCurve;
    [SerializeField] private AnimationCurve resonanceCurve;
    [SerializeField] private AnimationCurve pingCurve;

    private List<Vector3> initPositions = new List<Vector3>();

    private float resonanceTime = 0;
    private float amount = 0;

    private LTDescr tween;

    private void Awake() {
        BuildChain();
    }

    public void Update() {
        UpdateChain();
    }

    public override void BuildChain() {
        lineRenderer.positionCount = chainResolution;

        for (int i = 0; i < chainResolution; i++) {
            var increment = (float)1 / (chainResolution - 1) * i;
            var chainPosition = Vector3.Lerp(startPoint.position, endPoint.position, increment);
            lineRenderer.SetPosition(i, chainPosition);

            initPositions.Add(chainPosition);
        }
    }

    private void UpdateChain() {
        for (int i = 0; i < chainResolution; i++) {
            var increment = i / (float)(chainResolution-1);
            var newY = (shapeCurve.Evaluate(increment) * maxPingForce) * resonanceCurve.Evaluate(resonanceTime) * amount;
            var initPos = initPositions[i];
            var newPos = new Vector3(initPos.x, initPos.y + newY, initPos.z);
            lineRenderer.SetPosition(i, newPos);
        }
    }

    private void Ping() {
        if (tween != null) {
            LeanTween.cancel(tween.id);
        }

        tween = LeanTween.value(gameObject, 0, 1, pingDuration).setOnUpdate((float time) => {
            resonanceTime += animationSpeed * Time.deltaTime;
            amount = pingCurve.Evaluate(time);
        }).setOnComplete(() => {
            resonanceTime = 0;
        });
    }

    public override void DebugPing() {
        Ping();
    }

    public override void DebugSwell() {
        Ping();
    }
}
