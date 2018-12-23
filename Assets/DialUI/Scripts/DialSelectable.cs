using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialSelectable : MonoBehaviour {

    [SerializeField]
    DialUIPool UIPool;

    [SerializeField]
    MouseButton mouseButton = MouseButton.Left;

    public List<SelectableAction> actions = new List<SelectableAction> ();

    [HideInInspector]
    public Vector3 selectedPosition;

    //bool isSelected = false;
    
    void OnMouseOver () {
        if (Input.GetMouseButtonUp ((int)mouseButton)) {
            Vector3 mousePos = Input.mousePosition;

            // Set the world click position
            Ray ray = Camera.main.ScreenPointToRay (mousePos);
            RaycastHit hit;
            if (Physics.Raycast (ray, out hit)) {
                selectedPosition = hit.point;
            } else {
                selectedPosition = Camera.main.ScreenToWorldPoint (mousePos);
            }

            if (actions.Count == 0) {
                return; // Return if no action can be done
            }

            // Get available dial
            DialUI dial = UIPool.GetAvailable ();

            // Set the buttons
            dial.Require (actions.Count);
            for (int i = 0; i < actions.Count; i++) {
                dial.uiList[i].btnName.text = actions[i].name;
                dial.uiList[i].btn.onClick.AddListener (actions[i].action.Invoke);
            }

            // Open the dial at mouse click position
            dial.transform.position = mousePos;
            dial.Open ();
        }
    }
}

[System.Serializable]
public class DialAction : UnityEvent { }

[System.Serializable]
public class SelectableAction {
    public string name;
    public DialAction action = new DialAction ();
}

public enum MouseButton {
    Left,
    Right,
    Middle
}