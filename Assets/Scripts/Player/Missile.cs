using UnityEngine;
using Ruffz.Utilities;
using System;

public class Missile : MonoBehaviour, IResetable {

    [SerializeField]
    float speed = 100;

    Rigidbody body;
    Vector3 direction;

	void Start () {
        body = GetComponent<Rigidbody> ();
        Invoke ("Disappear", 5);
    }
    
    public void GoTo (Vector3 position) {
        body.isKinematic = false;
        direction = (position - transform.position).normalized;
        body.AddForce (direction * speed, ForceMode.Impulse);
    }

    public void Explode (float delay) {
        Debug.Log ("BOOM!");
        Reset ();
        CancelInvoke ("Disappear");
        Invoke ("Disappear", delay);
    }

    void Disappear () {
        body.isKinematic = true;
        gameObject.SetActive (false);
    }

    public void Reset () {
        direction = Vector3.zero;
        body.isKinematic = true;
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
    }
}
