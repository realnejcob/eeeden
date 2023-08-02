using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringChain : ChainBase {
    private List<SpringChainLink> chainLinks = new List<SpringChainLink>();

    [Header("Spring Settings:")]
    [Range(0, 1)] [SerializeField] private float velocityTransferSpread = 1;
    [Range(0, 5)] [SerializeField] private float stiffness = 0.25f;
    [Range(0, 1)] [SerializeField] private float decay = 0.85f;

    private List<float> leftDeltas = new List<float>();
    private List<float> rightDeltas = new List<float>();

    [Header("Strike Variables:")]
    [SerializeField] private float maxPingForce = 1;
    [SerializeField] private int pingPerSecond = 10;
    [SerializeField] private float swellTimeSeconds = 1;
    [SerializeField] private AnimationCurve swellCurve;

    private void Awake() {
        BuildChain();
        InitializeLineRenderer();
    }

    private void Update() {
        UpdateChainLinkNeighbours();
        UpdateLineRenderer();

#if UNITY_EDITOR
        if (updateSpringInEditor) {
            foreach (var chainLink in chainLinks) { 
                chainLink.chainLinkSpring.stiffness = stiffness;
                chainLink.chainLinkSpring.decay = decay;
            }
        }
#endif
    }

    public override void BuildChain() {
        for (int i = 0; i < chainResolution; i++) {
            leftDeltas.Add(0);
            rightDeltas.Add(0);

            var increment = (float)1 / (chainResolution - 1) * i;
            var chainPosition = Vector3.Lerp(startAnchor.position, endAnchor.position, increment);
            var newChainLink = CreateChainLink(chainPosition);

            if (i == 0 || i == chainResolution - 1) {
                newChainLink.IsLocked = true;
            }
        }
    }

    private SpringChainLink CreateChainLink(Vector3 newPosition) {
        GameObject chainObj = new GameObject("ChainLink");
        chainObj.transform.position = newPosition;
        chainObj.transform.parent = gameObject.transform;

        var chainLink = chainObj.AddComponent<SpringChainLink>();
        chainLink.Initialize(stiffness, decay);
        chainLinks.Add(chainLink);

        return chainLink;
    }

    private void PingMiddleChainLink(float force) {
        var middleChainIdx = GetMiddleChainIdx();
        chainLinks[middleChainIdx].chainLinkSpring.SetForce(force);
    }

    public override void DebugSwell() {
        var canPing = true;
        var totalPingCount = pingPerSecond * swellTimeSeconds;
        var increment = 1 / totalPingCount;
        var count = 0;
        var gate = 0f;
        LeanTween.value(gameObject, 0, 1, swellTimeSeconds).setOnUpdate((float t) => {
            if (t > gate && canPing) {
                var force = swellCurve.Evaluate(t) * maxPingForce;
                if (count % 2 == 0)
                    force *= -1;

                PingMiddleChainLink(force);
                count++;
                gate = increment * count;
                canPing = false;
            }

            canPing = true;
        });
    }

    public override void DebugPing() {
        PingMiddleChainLink(maxPingForce);
    }

    private void UpdateChainLinkNeighbours() {
        for (int i = 0; i < chainLinks.Count; i++) {
            if (i > 0) {
                var current = chainLinks[i].chainLinkSpring;
                var previous = chainLinks[i - 1].chainLinkSpring;
                leftDeltas[i] = velocityTransferSpread * (current.height - previous.height);
                chainLinks[i - 1].chainLinkSpring.velocity += leftDeltas[i];
            }
            
            if (i < chainLinks.Count - 1) {
                var current = chainLinks[i].chainLinkSpring;
                var next = chainLinks[i + 1].chainLinkSpring;
                rightDeltas[i] = velocityTransferSpread * (current.height - next.height);
                chainLinks[i + 1].chainLinkSpring.velocity += rightDeltas[i];
            }
        }
    }

    private void InitializeLineRenderer() {
        lineRenderer.positionCount = chainLinks.Count;
    }

    private void UpdateLineRenderer() { 
        for (int i = 0;i < lineRenderer.positionCount; i++) {
            var newPos = chainLinks[i].transform.position;
            lineRenderer.SetPosition(i, newPos);
        }
    }
}
