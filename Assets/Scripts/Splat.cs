using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splat : MonoBehaviour {
    
    [SerializeField]
    SpriteRenderer splatRender;

    [SerializeField]
    Sprite[] spriteChoices;

	void Start () {
        Randomize ();
    }
	
	void Randomize () {
        splatRender.sprite = spriteChoices[Random.Range (0, spriteChoices.Length)];
        splatRender.flipX = (Random.Range (0, 2) == 0);
        splatRender.transform.Rotate (Vector3.forward, Random.Range (0f, 360f), Space.Self);
	}
}
