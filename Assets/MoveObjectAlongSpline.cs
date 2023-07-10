using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class MoveObjectAlongSpline : MonoBehaviour {
    [SerializeField] private SplineContainer spline;
    [SerializeField] private Transform objectToMove;

    public float time = 0;
    public float speed = 1;
    private float t = 0;
    public Vector3 pos;

    private void Update() {
        t = time % 1;
        pos = spline.EvaluatePosition(t);
        objectToMove.position = pos;
    }
}
