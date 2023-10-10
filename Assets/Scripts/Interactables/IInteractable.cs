using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {
    public bool OnSouth(Interactor interactor);
    public bool OnLongSouth(Interactor interactor);
    public bool OnEast(Interactor interactor);
    public bool OnLeftShoulder(Interactor interactor);
    public bool OnRightShoulder(Interactor interactor);
    public bool OnRightStickTapRight(Interactor interactor);
    public bool OnRightStickTapLeft(Interactor interactor);
    public bool HoverEnable(Interactor interactor);
    public bool HoverDisable(Interactor interactor);
}
