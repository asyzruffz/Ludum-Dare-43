using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ruffz.Utilities;
using System;

public class DestinationMark : MonoBehaviour, IResetable {

    [SerializeField]
    float stayDuration = 2;

    [SerializeField]
    ParticleSystem particles;

    void Start () {
        Hide ();
    }

    public void Reset () {
        CancelInvoke ("Hide");
        gameObject.SetActive (true);
        particles.Play ();
        Invoke ("Hide", stayDuration);
    }

    void Hide () {
        gameObject.SetActive (false);
    }
}
