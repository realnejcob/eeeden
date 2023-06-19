using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Device : MonoBehaviour {
    protected bool IsOpen { get; set; } = false;
    public bool isReOpenable = true;
    public List<Opener> openersToUnlock = new List<Opener>();

    public virtual void Open() {
        IsOpen = true;
    }
    public virtual void Close() {
        IsOpen = false;
    }

    public virtual void CheckForInteraction() {
        if (!isReOpenable && IsOpen)
            return;

        if (GetCanOpen()) {
            Open();

            if (!isReOpenable) {
                LockAllKeys();
            }

            return;
        }

        Close();
    }

    private void OnEnable() {
        foreach (var key in openersToUnlock) {
            key.OnLock += CheckForInteraction;
            key.OnUnlock += CheckForInteraction;
        }
    }

    private void OnDisable() {
        foreach (var key in openersToUnlock) {
            key.OnLock -= CheckForInteraction;
            key.OnUnlock -= CheckForInteraction;
        }
    }

    private bool GetCanOpen() {
        var canOpen = false;

        foreach (var key in openersToUnlock) {
            if (key.IsUnlocked) {
                canOpen = true;
                continue;
            }

            canOpen = false;
            break;
        }

        return canOpen;
    }

    private void LockAllKeys() {
        foreach (var key in openersToUnlock) {
            key.CanInteract = false;
        }
    }
}
