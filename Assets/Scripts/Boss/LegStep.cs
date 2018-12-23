using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LegStep : MonoBehaviour {

    [SerializeField]
    Transform anchor;

    [Space]
    [SerializeField]
    Transform bodyOwner;
    [SerializeField]
    int legID;
    [SerializeField]
    int legNumber = 1;

    [SerializeField]
    float stepDuration = 1;
    [SerializeField]
    float stepThreshold = 2;

    static Dictionary<Transform, int> legTurn = new Dictionary<Transform, int> ();
    //static int legTurn = 0;

    bool stepping = false;

    void Start () {
        // Register leg turn for this body
        if (bodyOwner != null && !legTurn.ContainsKey(bodyOwner)) {
            legTurn.Add (bodyOwner, 0);
        } else if (bodyOwner == null) {
            Debug.Log ("This leg has no body owner!");
        }

        transform.parent = null;
    }
	
	void Update () {
        if (!stepping) {
            if (legID == legTurn[bodyOwner]) {
                if (NeedStepping ()) {
                    Step ();
                } else {
                    NextLeg ();
                }
            }
        }
    }

    void Step () {
        if (stepping) return;
        stepping = true;

        Sequence stepSequence = DOTween.Sequence ();

        stepSequence.Append (transform.DOMoveY (2, stepDuration / 2f));
        stepSequence.Append (transform.DOMoveY (0, stepDuration / 2f));
        stepSequence.Insert (0, transform.DOMoveX (anchor.position.x, stepDuration));
        stepSequence.Insert (0, transform.DOMoveZ (anchor.position.z, stepDuration));
        stepSequence.Insert (0, transform.DORotate (anchor.rotation.eulerAngles, stepDuration));

        stepSequence.OnComplete (SteppingDone);
    }

    void SteppingDone () {
        NextLeg ();
        stepping = false;
    }

    void NextLeg () {
        legTurn[bodyOwner] = (legTurn[bodyOwner] + 1) % legNumber; // Increment the turn for next leg
    }

    bool NeedStepping () {
        return Vector3.Distance (transform.position, anchor.position) > stepThreshold;
    }
}
