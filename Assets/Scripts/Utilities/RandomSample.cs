using System.Collections.Generic;
using UnityEngine;

public class RandomSample  {

    List<int> pop = new List<int> ();
    int total;
	bool resetOnEmpty;

    public RandomSample (int sample, bool autoReset = false) {
        total = sample;
		resetOnEmpty = autoReset;
		Reset ();
    }
	
    public int Next() {
        if (pop.Count == 0) {
			if (resetOnEmpty) {
				Reset ();
			} else {
				Debug.Log ("Sample is empty!");
				return -1;
			}
        }

        int index = Random.Range (0, pop.Count);
        int picked = pop[index];
        pop.RemoveAt (index);
        return picked;
    }

    public void Reset () {
        pop.Clear ();
        for (int i = 0; i < total; i++) {
            pop.Add (i);
        }
    }

    public bool IsEmpty () {
        return pop.Count == 0;
    }
}
