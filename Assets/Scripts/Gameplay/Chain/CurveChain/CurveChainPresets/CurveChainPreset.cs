using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCurveChainPreset", menuName = "Peg/Chain/Create New Chain Preset", order = 1)]
public class CurveChainPreset : ScriptableObject {
    public float maxForce = 0.25f;
    public float duration = 6f;
    public float speed = 15f;
    public AnimationCurve shapeCurve;
    public AnimationCurve movementCurve;
    public AnimationCurve forceCurve;
}
