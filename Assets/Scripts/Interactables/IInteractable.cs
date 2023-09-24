using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {
    public bool PressInteract(Interactor interactor);
    public bool LongPressInteract(Interactor interactor);
    public bool HoverEnable(Interactor interactor);
    public bool HoverDisable(Interactor interactor);
}
