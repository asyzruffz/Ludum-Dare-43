using UnityEngine;

public class Switch : MonoBehaviour, ITriggerable {

    [SerializeField]
    [ShowOnly]
    bool down;

    public ObjectTrigger[] triggers;

    public bool sticky;
    [ConditionalHide("sticky", true)]
    public bool autoReset;
    [ConditionalHide ("sticky", true)]
    public float stickyDuration = 1;

    float timer = 0;
    //Animator animator;
    bool entered = false;
    int countUser = 0;

    void Start () {
        //animator = GetComponent<Animator> ();
    }

    void Update () {
        if (autoReset) {
            if (down) {
                if (timer >= stickyDuration) {
                    timer = 0;
                    Close ();
                }

                timer += Time.deltaTime;
            }
        }
    }

    public virtual void Open () {
        //animator.SetInteger("AnimState", 1);
        foreach (ObjectTrigger trigger in triggers) {
            if (trigger != null) {
                if (sticky && !autoReset)
                    trigger.Toggle ();
                else
                    trigger.SwitchOn (true);
            }
        }
    }

    public virtual void Close () {
        //animator.SetInteger ("AnimState", 0);
        down = false;
        foreach (ObjectTrigger trigger in triggers) {
            if (trigger != null)
                trigger.SwitchOn (false);
        }
    }

    protected void OnTriggerEnter (Collider target) {
        countUser++;

        if (target.gameObject.CompareTag ("Player")) {
            down = true;
        }

        Open ();
    }

    protected void OnTriggerExit (Collider target) {
        countUser--;

        if (sticky && down)
            return;

        // Only close when no one else is there anymore
        if (countUser == 0) {
            entered = false;
            Close ();
        }
    }

    void OnDrawGizmos () {
        Gizmos.color = sticky ? Color.red : Color.blue;

        foreach (ObjectTrigger trigger in triggers) {
            if (trigger != null)
                Gizmos.DrawLine (transform.position, trigger.triggerObject.transform.position);
        }

    }

    public bool IsDown () {
        if (down && !entered) {
            entered = true;
            return true;
        }

        return false;
    }
}
