using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class PulseCore : MonoBehaviour {
    private MeshRenderer meshRenderer;
    private float ranOffset;
    private float ranTime;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
        ranOffset = Random.Range(0f, 100f);
        ranTime = Random.Range(1f,0.5f);
    }

    private void Update() {
        var t = Mathf.Sin(Time.time * ranTime + ranOffset) / 2f + 0.5f;
        var lerped = Mathf.Lerp(0.8f, 1f, t);
        meshRenderer.materials[0].SetFloat("FresnelEdge1", lerped);
    }
}
