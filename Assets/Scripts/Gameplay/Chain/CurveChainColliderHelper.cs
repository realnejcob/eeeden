using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveChainColliderHelper : MonoBehaviour {
    public bool isActive = true;

    private void OnDrawGizmos() {
        if (!isActive)
            return;

        Gizmos.color = new Color(1, 0, 0, 1);
        Gizmos.DrawSphere(transform.position, transform.localScale.x);
    }
}
