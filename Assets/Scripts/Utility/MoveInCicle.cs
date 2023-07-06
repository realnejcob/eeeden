using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInCicle : MonoBehaviour {
    [SerializeField] private float speed = 1;
    [SerializeField] private float radius = 1;

    [SerializeField] private float yBounceSpeed = 2;
    [SerializeField] private float yBounceAmount = 0.5f;

    private float yOffset;

    private void Awake() {
        yOffset = transform.position.y;
    }

    private void Update() {
        var x = Mathf.Sin(Time.time * speed) * radius;
        var z = Mathf.Cos(Time.time * speed) * radius;
        var y = Mathf.Sin(Time.time * yBounceSpeed) * yBounceAmount;
        transform.position = new Vector3 (x, y + yOffset, z);
    }
}
