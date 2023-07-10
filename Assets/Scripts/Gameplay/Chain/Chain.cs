using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour {
    public bool updateSpringInEditor = true;

    [Header("References:")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private List<ChainLink> chainLinks;

    [Header("Spring Settings:")]
    [SerializeField] private int chainResolution = 8;
    [Range(0, 1)] [SerializeField] private float velocityTransferSpread = 1;
    [Range(0, 5)] [SerializeField] private float stiffness = 0.25f;
    [Range(0, 1)] [SerializeField] private float decay = 0.85f;

    private List<float> leftDeltas = new List<float>();
    private List<float> rightDeltas = new List<float>();

    [Header("Debug Variables:")]
    [SerializeField] private float maxPingForce = 1;
    [SerializeField] private float swellCount = 10;
    [SerializeField] private float swellTime = 1;
    [SerializeField] private AnimationCurve swellCurve;

    private void Awake() {
        BuildChain();
        InitializeLineRenderer();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            PingMiddleChainLink(maxPingForce);
        } else if (Input.GetKeyDown(KeyCode.S)) {
            Swell();
        }

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

    private void Swell() {
        var canPing = true;
        var increment = 1 / swellCount;
        var count = 0;
        var gate = 0f;
        LeanTween.value(gameObject, 0, 1, swellTime).setOnUpdate((float t) => {
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

    private void BuildChain() {
        for (int i = 0; i < chainResolution; i++) {
            leftDeltas.Add(0);
            rightDeltas.Add(0);

            var increment = (float)1 / (chainResolution - 1) * i;
            var chainPosition = Vector3.Lerp(startPoint.position, endPoint.position, increment);
            var newChainLink = CreateChainLink(chainPosition);

            if (i == 0 || i == chainResolution - 1) {
                newChainLink.IsLocked = true;
            }
        }
    }

    private ChainLink CreateChainLink(Vector3 newPosition) {
        GameObject chainObj = new GameObject("ChainLink");
        chainObj.transform.position = newPosition;
        chainObj.transform.parent = gameObject.transform;

        var chainLink = chainObj.AddComponent<ChainLink>();
        chainLink.Initialize(stiffness, decay);
        chainLinks.Add(chainLink);

        return chainLink;
    }

    private void PingMiddleChainLink(float force) {
        var middleChainIdx = GetMiddleChainIdx();
        chainLinks[middleChainIdx].chainLinkSpring.SetForce(force);
    }

    private int GetMiddleChainIdx() {
        var idx = (chainResolution - 1) / 2;
        return idx;
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

    private void OnDrawGizmos() {
        if (startPoint != null) {
            Gizmos.color = new Color(0,1,0,0.25f);
            Gizmos.DrawCube(startPoint.transform.position, Vector3.one * 0.1f);
        }

        if (endPoint != null) {
            Gizmos.color = new Color(1,0,0,0.25f);
            Gizmos.DrawCube(endPoint.transform.position, Vector3.one * 0.1f);
        }
    }
}
