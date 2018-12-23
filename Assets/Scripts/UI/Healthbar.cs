using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour {

    [SerializeField]
    Health healthiness;

    [SerializeField]
    Transform healthFill;

    void Start () {
		
	}
	
	void Update () {
        transform.LookAt (transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        healthFill.localScale = new Vector3 ((float)healthiness.hp / healthiness.totalHp, healthFill.localScale.y, healthFill.localScale.z);
    }
}
