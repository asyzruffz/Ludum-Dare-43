using UnityEngine;

public class EscapeToExit : MonoBehaviour {
    
	void Update () {
		if (Input.GetButtonUp("Cancel")) {
            Application.Quit ();
        }
	}

}
