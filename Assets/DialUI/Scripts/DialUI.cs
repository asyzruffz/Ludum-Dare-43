using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Ruffz.Utilities;

public class DialUI : MonoBehaviour, IResetable {

    [SerializeField]
    float radius = 1;
    [SerializeField]
    float openDuration = 0.5f;
    [SerializeField]
    float closeDuration = 0.2f;

    public List<DialItem> uiList = new List<DialItem> ();

    [SerializeField]
    DialItem btnPrefab;

    bool isOn;
    bool isSelected;
    int usedSize;

	void Start () {
        Reset ();
        usedSize = uiList.Count;
    }
	
	void Update () {
        if (Input.GetMouseButtonDown (0)) {
            if (isOn) {
                StartCoroutine (DelayedCancel ());
            }
        }
    }

    public void Require (int amount) {
        usedSize = amount;

        // Keep adding new button until we reach the required amount
        for (int i = uiList.Count; i < usedSize; i++) {
            uiList.Add (Instantiate (btnPrefab, transform));
        }
    }

    public void Open () {
        isOn = true;
        isSelected = false;
        for (int i = 0; i < usedSize; i++) {
            float angle = (0 - (i * 360f / usedSize)) * Mathf.Deg2Rad;
            Vector3 offset = new Vector3 (Mathf.Sin (angle), Mathf.Cos (angle), 0);

            // Pivot is the offset reversed and changed scale from [-1,1] to [0,1]
            RectTransform rectTrans = uiList[i].GetComponent<RectTransform> ();
            rectTrans.pivot = new Vector2 ((-offset.x + 1) / 2, (-offset.y + 1) / 2);

            Sequence openSequence = DOTween.Sequence ();
            openSequence.Append (uiList[i].transform.DOLocalMove (offset * radius, openDuration));
            openSequence.InsertCallback (openDuration * 0.1f, uiList[i].Show);
        }
    }

    public void Close () {
        isOn = false;

        Sequence closeSequence = DOTween.Sequence ();

        for (int i = 0; i < usedSize; i++) {
            Sequence subSequence = DOTween.Sequence ();
            subSequence.Append (uiList[i].transform.DOLocalMove (Vector3.zero, closeDuration));
            subSequence.InsertCallback (closeDuration * 0.8f, uiList[i].Hide);
            closeSequence.Insert (0, subSequence);
        }

        closeSequence.OnComplete (Disable);
    }

    public void Hide () {
        isOn = false;
        for (int i = 0; i < uiList.Count; i++) {
            uiList[i].Hide ();
            
            // Reset position
            uiList[i].GetComponent<RectTransform> ().pivot = Vector2.one * 0.5f;
            uiList[i].transform.localPosition = Vector3.zero;
        }

        Disable ();
    }

    public void Reset () {
        foreach (DialItem ui in uiList) {
            ui.SetHandler (this);
            ui.Hide ();
        }
    }

    IEnumerator DelayedCancel () {
        yield return new WaitForSeconds (0.2f);
        if (!isSelected) {
            Close ();
        }
    }

    public void SetSelected () {
        isSelected = true;
    }

    void Disable () {
        gameObject.SetActive (false);
    }
}
