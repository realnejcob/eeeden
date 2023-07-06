using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gate : MonoBehaviour {
    public bool IsUnlocked { get; set; } = false;
    public bool CanInteract { get; set; } = true;

    public event Action OnUnlock;
    public event Action OnLock;

    public void Unlock() {
        if (!CanInteract)
            return;

        IsUnlocked = true;
        OnUnlockSuccess();

        OnUnlock?.Invoke();
    }

    public void Lock() {
        if (!CanInteract)
            return;

        IsUnlocked = false;
        OnLockSuccess();

        OnLock?.Invoke();
    }

    public abstract void OnUnlockSuccess();
    public abstract void OnLockSuccess();
}
