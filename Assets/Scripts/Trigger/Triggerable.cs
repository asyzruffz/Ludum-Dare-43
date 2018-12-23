using UnityEngine;
using UnityEngine.Events;

public class Triggerable : MonoBehaviour, ITriggerable {

    public TriggerAction open;
    public TriggerAction close;

    public virtual void Open () {
        print ("Triggerable [" + name + "] opened!");
        if (open != null) {
            open.Invoke ();
        }
    }

    public virtual void Close() {
        print ("Triggerable [" + name + "] closed!");
        if (close != null) {
            close.Invoke ();
        }
    }
}

[System.Serializable]
public class TriggerAction : UnityEvent { }
