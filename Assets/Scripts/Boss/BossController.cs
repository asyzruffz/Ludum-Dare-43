using UnityEngine;

public class BossController : MonoBehaviour {

    [SerializeField]
    HandStamp[] hands;

    Health health;
    FieldOfView fov;
    GrudgeList grudge;
    RandomSample sample;

	void Start () {
        health = GetComponent<Health> ();
        fov = GetComponent<FieldOfView> ();
        grudge = GetComponent<GrudgeList> ();
        sample = new RandomSample (hands != null ? hands.Length : 0, true);
	}
	
	void Update () {
        for (int i = 0; i < fov.visibleTargets.Count; i++) {
            grudge.AddTarget (fov.visibleTargets[i], 2, 3);
        }
	}

    public void Attack () {
        if (grudge.HasTarget ()) {
            HandStamp hand = hands[sample.Next ()];

            Transform target = grudge.GetTarget ();
            hand.Slam (target.position);
        }
    }

    void OnCollisionEnter (Collision other) {
        Missile missile = other.collider.GetComponent<Missile> ();
        if (other.collider.CompareTag ("Hazard")) {
            health.TakeDamage (50);
            if (missile) {
                missile.Explode (0);
            }
        }
    }
}
