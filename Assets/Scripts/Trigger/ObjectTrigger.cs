using UnityEngine;
public class ObjectTrigger : MonoBehaviour {

    public Triggerable triggerObject; // ITriggerable won't show in inspector, so we use the concrete Triggerable class
    public bool ignoreTrigger;
    public bool sticky;

    bool isOpened;
    int countUser = 0;

    void OnTriggerEnter (Collider target) {
        if (ignoreTrigger)
            return;

        if (target.gameObject.CompareTag ("Player")) {
            countUser++;
            triggerObject.Open ();
        }
    }

    void OnTriggerExit (Collider target) {
        if (ignoreTrigger || sticky)
            return;

        if (target.gameObject.CompareTag ("Player")) {
            countUser--;
            // Only close when no one else is there anymore
            if (countUser == 0) {
                triggerObject.Close ();
            }
        }
    }

    public void Toggle () {
        SwitchOn (!isOpened);
    }

    public void SwitchOn (bool on) {
        if (on) {
            isOpened = true;
            triggerObject.Open ();
        } else {
            isOpened = false;
            triggerObject.Close ();
        }
    }

    void OnDrawGizmos() {
		Gizmos.color = ignoreTrigger ? Color.gray : Color.green;
        Gizmos.DrawWireCube (transform.position, transform.rotation * transform.lossyScale);
	}
}
