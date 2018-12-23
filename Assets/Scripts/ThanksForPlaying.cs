using UnityEngine;

public class ThanksForPlaying : MonoBehaviour {

    [SerializeField]
    GameObject thanksBoard;

    [SerializeField]
    Health bossHealth;

    void Start () {
        bossHealth.deathEvent += DisplayThanks;
	}
	
	void DisplayThanks () {
        thanksBoard.SetActive (true);
        Invoke ("ExitGame", 3);
	}

    void ExitGame () {
        Debug.Log ("Exit!");
        Application.Quit ();
    }
}
