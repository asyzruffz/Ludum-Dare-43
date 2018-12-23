using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthbar : MonoBehaviour {

    [SerializeField]
    Health healthiness;

    [SerializeField]
    RectTransform healthFill;

    float initialWidth;

    void Start () {
        initialWidth = healthFill.rect.width;
	}
	
	void Update () {
        healthFill.sizeDelta = new Vector2 (((float)healthiness.hp / healthiness.totalHp - 1) * initialWidth, 0);
    }
}
