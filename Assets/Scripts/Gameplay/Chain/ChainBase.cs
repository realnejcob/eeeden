using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChainBase : MonoBehaviour {
    public bool updateSpringInEditor = true;
    public bool buildOnAwake = false;

    [Header("ChainBase references:")]
    [SerializeField] protected int chainResolution = 8;
    [SerializeField] protected LineRenderer lineRenderer;
    [SerializeField] protected Transform startPoint;
    [SerializeField] protected Transform endPoint;

    private void OnDrawGizmos() {
        if (startPoint == null || endPoint == null)
            return;

        Gizmos.color = new Color(1, 1, 0, 0.25f);
        Gizmos.DrawLine(startPoint.position, endPoint.position);

        Gizmos.color = new Color(0, 1, 0, 0.25f);
        Gizmos.DrawCube(startPoint.transform.position, Vector3.one * 0.1f);

        Gizmos.color = new Color(1, 0, 0, 0.25f);
        Gizmos.DrawCube(endPoint.transform.position, Vector3.one * 0.1f);
    }

    public abstract void DebugPing();
    public abstract void DebugSwell();
    public abstract void BuildChain();

    protected int GetMiddleChainIdx() {
        var idx = (chainResolution - 1) / 2;
        return idx;
    }
}
