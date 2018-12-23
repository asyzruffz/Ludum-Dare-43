using UnityEngine;

public class PawnController : MonoBehaviour {

    [SerializeField]
    GameObject splatPrefab;

    Health health;

	void Start () {
        health = GetComponent<Health> ();
        health.deathEvent += Die;
    }
	
	void Update () {
		
	}

    void OnCollisionEnter (Collision other) {
        if (other.collider.CompareTag ("Enemy")) {
            health.TakeDamage (5);
        }
    }

    void Die () {
        Debug.Log (name + " dead!");

        SelectableUnit unit = GetComponent<SelectableUnit> ();
        if (unit) {
            unit.Unselect ();
        }

        Instantiate (splatPrefab, transform.position, Quaternion.identity);
        Destroy (gameObject, 0.2f);
    }
}
