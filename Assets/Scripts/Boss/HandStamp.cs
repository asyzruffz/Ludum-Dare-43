using UnityEngine;
using DG.Tweening;

public class HandStamp : MonoBehaviour {

    [SerializeField]
    Transform anchor;
    
    bool slamming = false;
    float angle;

	void Start () {
        transform.parent = null;
        angle = Random.Range (0, 2 * Mathf.PI);
    }

    void Update () {
        if (!slamming) {
            angle += Time.deltaTime;
            Vector3 floatTarget = anchor.position + Vector3.up * Mathf.Sin (angle);
            transform.position = Vector3.Lerp (transform.position, floatTarget, Time.deltaTime * 2);
            transform.rotation = Quaternion.Lerp (transform.rotation, anchor.rotation, Time.deltaTime * 3);
        }
    }

    public void Slam (Vector3 target) {
        if (slamming) return;
        slamming = true;

        Sequence slamSequence = DOTween.Sequence ();

        // Hover over the target
        slamSequence.Append (transform.DOMove (new Vector3 (target.x, transform.position.y, target.z), 2));
        slamSequence.Join (transform.DORotate (Vector3.zero, 2));
        slamSequence.AppendInterval (1);

        // Slam down
        slamSequence.AppendCallback (SlamEffect);
        slamSequence.Append (transform.DOMoveY (0, 0.05f));
        slamSequence.AppendInterval (2);

        slamSequence.OnComplete (Return);
    }

    void Return () {
        Sequence returnSequence = DOTween.Sequence ();

        returnSequence.Append (transform.DOMove (anchor.position, 3));
        returnSequence.Join (transform.DORotateQuaternion (anchor.rotation, 3));

        returnSequence.OnComplete (SlammingDone);
    }

    void SlammingDone () {
        slamming = false;
    }

    void SlamEffect () {
        Camera.main.GetComponent<CameraShake> ().ShakeCamera ();
    }
}
