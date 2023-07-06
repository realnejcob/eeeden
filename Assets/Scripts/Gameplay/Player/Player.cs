using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Shiftable {
    [Header("Explore Pod Variables:")]
    [SerializeField] private float maxSpeed = 8;
    [SerializeField] private float acceleration = 200;
    [SerializeField] private float maxAccelerationForce = 150;
    private Vector3 m_UnitGoal;
    private Vector3 m_GoalVel;

    [SerializeField] private float turnSmoothTime = 0.5f;
    private float rotationVelocity;

    private void FixedUpdate() {
        if (!IsControlling)
            return;

        Move();
        Look();
    }

    private void Look() {
        var input = GetInput().GetRelativeInputDir(GetInput().LeftStickRaw);
        if (input != Vector3.zero) {
            var targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg;
            var smoothedAngle = Mathf.SmoothDampAngle(rb.rotation.eulerAngles.y, targetAngle, ref rotationVelocity, turnSmoothTime);
            rb.rotation = Quaternion.Euler(0, smoothedAngle, 0);
        }

    }

    private void Move() {
        Vector3 move = GetInput().GetRelativeInputDir(GetInput().LeftStickRaw);
        if (move.magnitude > 1f)
            move.Normalize();

        m_UnitGoal = move;

        Vector3 goalVel = m_UnitGoal * maxSpeed;
        m_GoalVel = Vector3.MoveTowards(m_GoalVel, goalVel, acceleration * Time.deltaTime);

        Vector3 neededAccel = (m_GoalVel - rb.velocity) / Time.deltaTime;
        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccelerationForce);

        rb.AddForce(Vector3.Scale(neededAccel * rb.mass, new Vector3(1, 0, 1)));
    }
}
