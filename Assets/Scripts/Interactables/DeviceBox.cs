using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceBox : Device {
    public Color openedColor;
    public Color closedColor;

    private MeshRenderer meshRenderer;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public override void Open() {
        base.Open();

        meshRenderer.materials[0].color = openedColor;
    }

    public override void Close() {
        base.Close();

        meshRenderer.materials[0].color = closedColor;
    }
}
