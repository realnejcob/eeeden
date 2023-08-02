using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChainBase : MonoBehaviour {
    public bool updateSpringInEditor = true;
    public bool buildOnAwake = false;
    public bool autoPlay = false;

    [Header("ChainBase references:")]
    [SerializeField] protected int chainResolution = 8;
    [SerializeField] protected LineRenderer lineRenderer;
    [ReadOnly] [SerializeField] protected Transform startAnchor = null;
    [ReadOnly] [SerializeField] protected Transform endAnchor = null;

    private void OnDrawGizmos() {
        if (startAnchor == null || endAnchor == null)
            return;

        Gizmos.color = new Color(1, 1, 0, 0.25f);
        Gizmos.DrawLine(startAnchor.position, endAnchor.position);

        Gizmos.color = new Color(0, 1, 0, 0.25f);
        Gizmos.DrawCube(startAnchor.transform.position, Vector3.one * 0.1f);

        Gizmos.color = new Color(1, 0, 0, 0.25f);
        Gizmos.DrawCube(endAnchor.transform.position, Vector3.one * 0.1f);
    }

    public abstract void DebugPing();
    public abstract void DebugSwell();
    public abstract void BuildChain();

    protected int GetMiddleChainIdx() {
        var idx = (chainResolution - 1) / 2;
        return idx;
    }
}
