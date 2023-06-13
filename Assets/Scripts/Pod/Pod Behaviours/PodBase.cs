using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PodBase : MonoBehaviour {
    public bool IsControlling { get; private set; }

    [Header("Base Variables:")]
    [SerializeField] private MeshRenderer headMesh;
    [SerializeField] private MeshRenderer bodyMesh;
    [SerializeField] private Material enabledMaterial;
    [SerializeField] private Material disabledMaterial;
    [SerializeField] private float enabledHeight = 1.025f;
    [SerializeField] private float disabledHeight = 1.025f;
    [SerializeField] private ShiftDisplay shiftDisplay;
    [SerializeField] private ShiftRaycastHelper shiftRaycastHelper;

    protected Rigidbody rb;
    protected Camera cam;

    private PodBase targetedPod;
    private bool canDisplayShiftAOE = false;
    private bool canShift = false;

    private float shiftRadius = 4f;

    private List<PodBase> availablePods;

    private GroundRider groundRider;

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
        groundRider.SetHeight(enabledHeight);

        headMesh.gameObject.SetActive(true);
        SetBodyMaterial(enabledMaterial);

        canDisplayShiftAOE = true;
        shiftDisplay.gameObject.SetActive(true);
        shiftRaycastHelper.gameObject.SetActive(true);
    }

    public void DisablePod() {
        IsControlling = false;

        //rb.isKinematic = true;
        groundRider.SetHeight(disabledHeight);

        headMesh.gameObject.SetActive(false);
        SetBodyMaterial(disabledMaterial);

        canDisplayShiftAOE = false;
        shiftDisplay.gameObject.SetActive(false);
        shiftRaycastHelper.gameObject.SetActive(false);
    }

    private void SetBodyMaterial(Material newMaterial) {
        var matArr = bodyMesh.materials;
        matArr[0] = newMaterial;
        bodyMesh.materials = matArr;
    }

    private void Update() {
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

        availablePods = new List<PodBase>();

        foreach (var obj in foundPods) {
            var pod = obj.GetComponent<PodBase>();
            if (!pod.IsControlling) {
                availablePods.Add(pod);
                print(pod.transform.position);
            }
        }
    }

    private void UpdateShiftAreaOfEffectConnections() {
        var rightStick = GetInput().GetRelativeInputDir(GetInput().RightStickRaw, true);
        targetedPod = shiftRaycastHelper.GetClosestPod();

        if (rightStick.magnitude > 0) {
            if (targetedPod == null) {
                shiftDisplay.SetConnectionEndPoint(rightStick);
            } else {
                shiftDisplay.SetConnectionEndPointAbsolute(targetedPod.transform.position);
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
        if (targetedPod != null) {
            shiftDisplay.CenterConnectionPoints();
            shiftDisplay.gameObject.SetActive(false);

            PodManager.Instance.SetActivePod(targetedPod);
            targetedPod = null;
        }
    }

    private void OnDrawGizmos() {
        if (!IsControlling)
            return;

        Gizmos.color = new Color(0, 1, 0, 0.05f);
        Gizmos.DrawSphere(transform.position + (Vector3.up), shiftRadius);
    }
}
