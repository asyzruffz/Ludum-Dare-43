using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public int hp;
    public int totalHp = 100;
	public bool isInvincible;

	public delegate void CharacterDeathDelegate();
	public delegate void CharacterHitDelegate();
	public event CharacterDeathDelegate deathEvent;
	public event CharacterHitDelegate hitEvent;

    bool isDead = false;

	void Start () {
		hp = totalHp;
	}

	void Update() {
		if (hp <= 0 && !isDead) {
            isDead = true;
            if (deathEvent != null) {
                deathEvent ();
            }
		}
	}

	public void TakeDamage(int damage) {
		if (!isInvincible) {
			hp -= damage;
            if (hitEvent != null) {
                hitEvent ();
            }
		}
	}
}
