using UnityEngine;

public class SelectableUnit : MonoBehaviour {

    public bool isSelected;

    [SerializeField]
    GameObject[] selectedDisplays;
    
	void Update () {
		if (selectedDisplays != null) {
            for (int i = 0; i < selectedDisplays.Length; i++) {
                selectedDisplays[i].SetActive (isSelected);
            }
        }
	}

    public void Unselect () {
        if (isSelected) {
            RectangleSelect.selecteds.Remove (gameObject);
            isSelected = false;
        }
    }
}
