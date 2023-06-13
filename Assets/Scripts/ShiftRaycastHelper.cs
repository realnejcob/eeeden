using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShiftRaycastHelper : MonoBehaviour {
    private float radius;
    private float segments = 11;
    private float spread = 165;

    private Vector3 offset = Vector3.up;

    public List<PodBase> AvailablePods { get; private set; } = new List<PodBase>();

    public void SetRadius(float newRadius) {
        radius = newRadius;
    }

    private void FixedUpdate() {
        AvailablePods.Clear();

        var rightStick = InputManager.Instance.RightStickRaw;
        var relativeRightStick = InputManager.Instance.GetRelativeInputDir(rightStick);

        var forwardVector = transform.forward;

        if (relativeRightStick.magnitude == 0)
            return;

        Gizmos.color = Color.yellow;

        var maxAngle = (int)(spread * 0.5f);
        var minAngle = -maxAngle;

        var startingPosition = transform.position + offset;
        transform.rotation = Quaternion.LookRotation(relativeRightStick);

        var increment = (int)(spread / segments);

        for (int angle = minAngle; angle <= maxAngle; angle += increment) {
            var rotation = Quaternion.AngleAxis(angle, transform.right) * forwardVector;
            var endPosition = startingPosition + (rotation * radius);

            var ray = new Ray(startingPosition, rotation);
            var mask = LayerMask.GetMask("Pod");

            if (Physics.SphereCast(ray, 0.15f, out var hit, radius, mask)) {
                Debug.DrawLine(startingPosition, endPosition, Color.green);
                var pod = hit.collider.GetComponent<PodBase>();
                if (AvailablePods.Contains(pod))
                    continue;

                AvailablePods.Add(pod);
            } else {
                Debug.DrawLine(startingPosition, endPosition, Color.grey);
            }
        }
    }

    public PodBase GetClosestPod() {
        var pod = AvailablePods.FirstOrDefault();

        if (AvailablePods.Count > 1) {
            var list = AvailablePods.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).ToList();
            pod = list[0];
        }

        return pod;
    }

    private void OnDrawGizmos() {
        /*
        var rightStick = InputManager.Instance.RightStickRaw;
        var relativeRightStick = InputManager.Instance.GetRelativeInputDir(rightStick);
        var forwardVector = transform.forward;

        if (relativeRightStick.magnitude == 0)
            return;

        Gizmos.color = Color.yellow;

        var maxAngle = (int)(spread * 0.5f);
        var minAngle = -maxAngle;

        var startingPosition = transform.position + offset;
        transform.rotation = Quaternion.LookRotation(relativeRightStick);

        var increment = (int)(spread / segments);

        for (int angle = minAngle; angle <= maxAngle; angle += increment) {
            var rotation = Quaternion.AngleAxis(angle, transform.right) * forwardVector;
            var endPosition = startingPosition + (rotation * radius);
            Gizmos.DrawLine(startingPosition, endPosition);
        }
        */
    }
}
