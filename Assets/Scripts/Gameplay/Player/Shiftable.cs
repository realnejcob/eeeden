using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shiftable : MonoBehaviour {
    public bool IsControlling { get; private set; }

    [Header("Shiftable Variables:")]
    [SerializeField] private ShiftDisplay shiftDisplay;
    [SerializeField] private ShiftRaycastHelper shiftRaycastHelper;

    protected Rigidbody rb;
    protected Camera cam;

    private Shiftable targetedShiftable;
    private bool canDisplayShiftAOE = false;
    private bool canShift = false;

    private float shiftRadius = 4f;

    private List<Shiftable> availableShiftables;

    protected GroundRider groundRider;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        groundRider = GetComponent<GroundRider>();
        cam = Camera.main;

        shiftDisplay.SetAreaRadius(shiftRadius);
        shiftRaycastHelper.SetRadius(shiftRadius);

        DisablePod();
    }

    public InputManager GetInput() {
        return InputManager.Instance;
    }

    public void EnablePod() {
        IsControlling = true;

        //rb.isKinematic = false;

        canDisplayShiftAOE = true;
        shiftDisplay.gameObject.SetActive(false);
        shiftRaycastHelper.gameObject.SetActive(true);
    }

    public void DisablePod() {
        IsControlling = false;

        //rb.isKinematic = true;

        canDisplayShiftAOE = false;
        shiftDisplay.gameObject.SetActive(false);
        shiftRaycastHelper.gameObject.SetActive(false);
    }


    private void LateUpdate() {
        return;

        ShiftAreaOfEffectDisplay();

        if (!IsControlling)
            return;

        UpdateShiftAreaOfEffectConnections();
        CheckForShiftInput();
    }

    private void ShiftAreaOfEffectDisplay() {
        if (canDisplayShiftAOE) {
            if (GetInput().GetGamepad().leftShoulder.ReadValue() > 0 && !shiftDisplay.gameObject.activeInHierarchy) {
                shiftDisplay.gameObject.SetActive(true);
            } else if (GetInput().GetGamepad().leftShoulder.ReadValue() < 1 && shiftDisplay.gameObject.activeInHierarchy) {
                shiftDisplay.gameObject.SetActive(false);
            }
        } else if (!canDisplayShiftAOE && !shiftDisplay.gameObject.activeInHierarchy) {
            shiftDisplay.gameObject.SetActive(false);
        }
    }

    private void GetPodsInArea() {
        var mask = LayerMask.GetMask("Pod");
        var foundPods = Physics.OverlapSphere(transform.position, shiftRadius, mask);
        if (foundPods.Length <= 1)
            return;

        availableShiftables = new List<Shiftable>();

        foreach (var obj in foundPods) {
            var pod = obj.GetComponent<Shiftable>();
            if (!pod.IsControlling) {
                availableShiftables.Add(pod);
                print(pod.transform.position);
            }
        }
    }

    private void UpdateShiftAreaOfEffectConnections() {
        var rightStick = GetInput().GetRelativeInputDir(GetInput().RightStickRaw, true);
        targetedShiftable = shiftRaycastHelper.GetClosestPod();

        if (rightStick.magnitude > 0) {
            if (targetedShiftable == null) {
                shiftDisplay.SetConnectionEndPoint(rightStick);
            } else {
                shiftDisplay.SetConnectionEndPointAbsolute(targetedShiftable.transform.position);
            }

        } else {
            shiftDisplay.CenterConnectionPoints();
        }
    }

    private void CheckForShiftInput() {
        if (GetInput().GetGamepad().leftShoulder.ReadValue() < 1 && canShift) {
            canShift = false;
            Shift();
        } else if (GetInput().GetGamepad().leftShoulder.ReadValue() > 0 && !canShift) {
            canShift = true;
        }
    }

    private void Shift() {
        if (targetedShiftable != null) {
            shiftDisplay.CenterConnectionPoints();
            shiftDisplay.gameObject.SetActive(false);

            PodManager.Instance.SetActivePod(targetedShiftable);

            targetedShiftable = null;
        }
    }

    private void OnDrawGizmos() {
        if (!IsControlling)
            return;

        Gizmos.color = new Color(1, 1, 1, 0.01f);
        Gizmos.DrawSphere(transform.position + (Vector3.up), shiftRadius);
    }
}
