using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveChain : ChainBase {
    [Header("Spring Settings:")]
    [SerializeField] private CurveChainPreset preset;
    private List<Vector3> initPositions = new List<Vector3>();

    private float resonanceTime = 0;
    private float amount = 0;
    private float displacementModifier = 1;

    private LTDescr tween;

    public float InitialLength { get; private set; } = 0;
    public float BreakLength { get; private set; } = 0;

    private void Start() {
        BuildChain();
        ConfigureCollider();

        InitialLength = GetCurrentLength();
        BreakLength = InitialLength + breakLengthOffset;

        SetPitchColor();

        chainCollider.OnExitSameSide += Ping;

        Ping();

        if (autoPlay) {
            AutoPlay();
        }
    }

    public void FixedUpdate() {
        UpdateDisplacement();
        UpdateChain();
    }

    public void Update() {
        CheckForBoundsDestroy();
    }

    public void Pitch(int step) {
        var newPitch = currentPitch + step;
        currentPitch = newPitch;
    }

    public void SetPitchColor() {
        var pitchColor = GetPitchColor(currentPitch);
        lineRenderer.materials[0].SetColor("_BaseColor", pitchColor);
    }

    private Color GetPitchColor(int pitch) {
        var normalizedPitch = Mathf.InverseLerp(minPitch, maxPitch, pitch);
        return pitchGradient.Evaluate(normalizedPitch);
    }

    private void CheckForBoundsDestroy() {
        if (chainCollider.IsOutOfBounds) {
            if (GetCurrentLength() > BreakLength) {
                PegConnectionManager.Instance.RemoveConnectionByCurveChain(this);
            }
        }
    }

    public bool ExceedsPitch() {
        if (currentPitch > maxPitch || currentPitch < minPitch) {
            return true;
        }

        return false;
    }

    public override void BuildChain() {
        lineRenderer.positionCount = chainResolution;

        for (int i = 0; i < chainResolution; i++) {
            var increment = (float)1 / (chainResolution - 1) * i;
            var chainPosition = GetChainPosition(increment);

            initPositions.Add(chainPosition);

            lineRenderer.SetPosition(i, chainPosition);
        }
    }

    private float GetCurrentLength() {
        var length = 0f;
        length += Vector3.Distance(startAnchor.position, middleAnchor.position);
        length += Vector3.Distance(middleAnchor.position, endAnchor.position);
        return length;
    }

    public void ConfigureCollider() {
        chainCollider.transform.position = middleAnchor.position;
        chainCollider.transform.LookAt(endAnchor, chainCollider.transform.up);

        var length = Vector3.Distance(startAnchor.position, endAnchor.position);
        chainCollider.transform.localScale = chainCollider.transform.localScale + (Vector3.forward * length);
    }

    private void UpdateChain() {
        for (int i = 0; i < chainResolution; i++) {
            var initPos = initPositions[i];
            var normalizedIdx = i / (float)(chainResolution-1);

            var displaceOffset = GetDisplaceOffset(normalizedIdx) - initPos;

            var newPos = new Vector3(
                initPos.x + displaceOffset.x,
                initPos.y + (GetAnimatedY(normalizedIdx) * displacementModifier),
                initPos.z + displaceOffset.z);

            lineRenderer.SetPosition(i, newPos);
        }
    }

    private void UpdateDisplacement() {
        if (chainCollider.IsTouching) {
            var playerPos = PodManager.Instance.GetActivePod().transform.position;
            middleAnchor.transform.position = new Vector3(playerPos.x, middleAnchorInitPosition.y, playerPos.z) + chainCollider.enterDirection;
            displacementModifier = 0;
        } else {
            middleAnchor.transform.position = middleAnchorInitPosition;
            displacementModifier = 1;
        }
    }

    private float GetAnimatedY(float increment) {
       return preset.shapeCurve.Evaluate(increment) * preset.maxForce * preset.movementCurve.Evaluate(resonanceTime) * amount;
    }

    private Vector3 GetDisplaceOffset(float increment) {
        var lerped = LerpTripleVector3(startAnchor.position, middleAnchor.position, endAnchor.position, increment);
        return new Vector3(lerped.x, 0, lerped.z);
    }

    private Vector3 GetChainPosition(float increment) {
        return Vector3.Lerp(startAnchor.position, endAnchor.position, increment);
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

    public void ConfigureAnchors(Transform newStartAnchor, Transform newEndAnchor) {
        startAnchor = newStartAnchor;
        endAnchor = newEndAnchor;
        middleAnchor = CreateMiddleAnchor();
    }

    private Transform CreateMiddleAnchor() {
        var newMiddleAnchor = new GameObject("Middle Anchor");
        newMiddleAnchor.transform.SetParent(transform);
        newMiddleAnchor.transform.position = Vector3.Lerp(startAnchor.position, endAnchor.position, 0.5f);

        middleAnchorInitPosition = newMiddleAnchor.transform.position;

        return newMiddleAnchor.transform;
    }

    public override void DebugPing() {
        Ping();
    }

    public override void DebugSwell() {
        Ping();
    }

    Vector3 LerpTripleVector3(Vector3 a, Vector3 b, Vector3 c, float t) {
        if (t <= 0.5f) {
            return Vector3.Lerp(a, b, t * 2f);
        } else {
            return Vector3.Lerp(b, c, (t * 2f) - 1f);
        }
    }
}
