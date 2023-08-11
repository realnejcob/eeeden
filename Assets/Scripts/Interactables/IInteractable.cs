using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {
    public bool PressInteract(Interactor interactor);
    public bool LongPressInteract(Interactor interactor);
}
