using System;
using System.Collections.Generic;
using UnityEngine;
using Ruffz.Utilities;

public class GrudgeList : MonoBehaviour {

    PriorityQueue<HitTarget> entries = new PriorityQueue<HitTarget> ();
    
    public Transform GetTarget () {
        return entries.Peek ().transform;
    }

    public void AddTarget (Transform transform, int priority = 1, float memory = 5) {
        HitTarget t = new HitTarget (transform, priority, memory);
        AddTarget (t);
    }

    public void AddTarget (HitTarget target) {
        // If already existed update it instead
        int ti = entries.ContainsAt (target);
        if (ti >= 0) {
            entries[ti] = target;
        } else {
            entries.Enqueue (target);
        }
    }

    public bool HasTarget () {
        return entries.Count > 0;
    }

    void Update () {
        // Decay the memory and remove it if forgotten or destroyed
        for (int i = 0; i < entries.Count; i++) {
            entries[i].memory -= Time.deltaTime;
            if (entries[i].memory <= 0 || entries[i].transform == null) {
                entries.RemoveAt (i);
            }
        }
    }
}

public class HitTarget : IComparable<HitTarget> {
    public Transform transform;
    public int priority;
    public float memory;

    public HitTarget () { priority = 1; memory = 5; } // By default only lasts for 5 seconds.
    public HitTarget (Transform transform, int priority = 1, float memory = 5) {
        this.transform = transform;
        this.priority = priority;
        this.memory = memory;
    }

    public int CompareTo (HitTarget other) {
        int result = this.priority - other.priority;
        if (result == 0) return result;
        return (int)Mathf.Sign (result);
    }

    public override string ToString () {
        string s = "(" + (transform == null ? "?" : transform.name);
        s += "|" + priority + "|" + memory + ")";
        return s;
    }

    public override bool Equals (object obj) {
        if (ReferenceEquals (null, obj)) {
            return false;
        } else if (obj.GetType () != GetType ()) {
            return false;
        }

        return this.transform == ((HitTarget)obj).transform;
    }

    public override int GetHashCode () {
        unchecked {
            const int HashingBase = (int)2166136261; // Choose large primes to avoid hashing collisions
            return HashingBase ^ (!ReferenceEquals (null, transform) ? transform.GetHashCode () : 0);
        }
    }
}
