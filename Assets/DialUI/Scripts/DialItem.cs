using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialItem : MonoBehaviour {

    public Button btn;
    public TextMeshProUGUI btnName;

    DialUI _handler;

    public bool hasHandler {
        get { return _handler != null; }
    }

    void Start () {
        if (btn == null) {
            btn = GetComponent<Button> ();
        }
        if (btnName == null) {
            btnName = GetComponentInChildren<TextMeshProUGUI> ();
        }

        if (btn) {
            btn.onClick.AddListener (HideAll);
        }
    }

    public void Hide () {
        gameObject.SetActive (false);
    }

    public void Show () {
        gameObject.SetActive (true);
    }

    public void HideAll () {
        if (hasHandler) {
            _handler.SetSelected ();
            _handler.Hide ();
        } else {
            Hide ();
        }
    }

    public void SetHandler (DialUI handler) {
        _handler = handler;
    }
}
