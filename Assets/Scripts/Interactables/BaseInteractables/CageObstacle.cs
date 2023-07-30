using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageObstacle : MonoBehaviour {
    [SerializeField] private List<Transform> fins;
    [SerializeField] private AnimationCurve distributionCurve;
    private float yEnd = -3;

    public Vector2[] curveEnds;

    public float position = 0;
    [Range(0.05f,1f)] public float curveLength = 0.5f;
    public float p = 0;

    private void Awake() {
        curveEnds = new Vector2[fins.Count];
    }

    private void Update() {
        p = position;

        for (int i = 0; i < fins.Count; i++) {
            var percentage = (float)i / fins.Count;

            var start = percentage - (curveLength/2);
            var end = percentage + (curveLength/2);

            curveEnds[i].x = start;
            curveEnds[i].y = end;

            Transform t = fins[i];
            var newP = Mathf.LerpUnclamped(p, start, end);
            newP = Mathf.InverseLerp(start, end, newP);
            newP = distributionCurve.Evaluate(newP);

            var newPos = new Vector3(t.transform.position.x, Mathf.Lerp(0, yEnd, newP), t.transform.position.z);
            t.transform.position = newPos;
        }
    }
}
