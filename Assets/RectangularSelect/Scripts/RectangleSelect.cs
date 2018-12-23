using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RectangleSelect : MonoBehaviour {

    //[SerializeField]
    Camera cam;

    [SerializeField]
    LayerMask selectMask = -1;

    [SerializeField]
    [Range (0, 1)]
    float transparency = 1f;

    [SerializeField]
    float minHoldTime = 0.5f;

    [SerializeField]
    bool disableSingleClick = false;

    [SerializeField]
    Image rectBorder;

    public static List<GameObject> selecteds = new List<GameObject> ();
    public static UnityAction onDoneSelection;
    public static bool hasSelection {
        get { return selecteds.Count > 0; }
    }

    RectTransform rectTrans;
    //bool held = false;
    bool selecting = false;
    float timer = 0;
    Vector3 startPos;
    
    void Start () {
        rectTrans = GetComponent<RectTransform> ();
        if (rectBorder == null) {
            rectBorder = GetComponent<Image> ();
        }
        if (cam == null) {
            cam = Camera.main;
        }

        Hide ();
    }
	
	void Update () {
        // Starting to click, save the start position
        if (Input.GetMouseButtonDown (0)) {
            startPos = Input.mousePosition;
        }

        // Releasing the click and done selecting
        if (Input.GetMouseButtonUp (0)) {
            if (selecting) {
                SelectWithin (startPos, Input.mousePosition);
            } else if (!disableSingleClick) {
                SelectOnly (startPos);
            }

            selecting = false;
            Hide ();

            if (onDoneSelection != null) {
                onDoneSelection ();
            }
        }

        // Holding timing before we decide to do rectangular select
		if (Input.GetMouseButton (0) && !selecting) {
            timer += Time.deltaTime;
            if (timer >= minHoldTime) {
                selecting = true;
                timer = 0;
            }
        }

        if (selecting) {
            Show ();
            Rect rect = GetSelectingRect (Input.mousePosition);
            rectTrans.position = rect.position;
            rectTrans.sizeDelta = rect.size;
        }

        //held = Input.GetMouseButton (0);
	}

    void SelectWithin (Vector3 cornerA, Vector3 cornerB, bool union = false) {
        if (!union) {
            ClearSelection ();
        }

        Vector3 p1 = cam.ScreenToWorldPoint (new Vector3 (cornerA.x, cornerA.y, cam.nearClipPlane));
        Vector3 p2 = cam.ScreenToWorldPoint (new Vector3 (cornerB.x, cornerB.y, cam.nearClipPlane));

        Vector3 centre = (p1 + p2) / 2f;
        Vector3 half = new Vector3 (Mathf.Abs (p1.x - p2.x) / 2f, Mathf.Abs (p1.y - p2.y) / 2f, 0.5f);

        RaycastHit[] hits = Physics.BoxCastAll (centre, half, cam.transform.forward, cam.transform.rotation, Mathf.Infinity, selectMask);

        for (int i = 0; i < hits.Length; i++) {

            SelectableUnit unit = hits[i].collider.GetComponent<SelectableUnit> ();
            if (unit) {
                unit.isSelected = true;
            }

            selecteds.Add (hits[i].collider.gameObject);
        }
    }

    void SelectOnly (Vector3 atPosition, bool union = false) {
        Ray ray = cam.ScreenPointToRay (atPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            if (!union) {
                ClearSelection ();
            }
            
            SelectableUnit unit = hit.collider.GetComponent<SelectableUnit> ();
            if (unit) {
                unit.isSelected = true;
            }

            selecteds.Add (hit.collider.gameObject);
        }
    }

    void Hide () {
        rectBorder.color = new Color (rectBorder.color.r, rectBorder.color.g, rectBorder.color.b, 0);
    }

    void Show () {
        rectBorder.color = new Color (rectBorder.color.r, rectBorder.color.g, rectBorder.color.b, transparency);
    }

    Rect GetSelectingRect (Vector3 endPos) {
        Vector2 minPos, maxPos;
        
        if (startPos.x <= endPos.x) {
            minPos.x = startPos.x;
            maxPos.x = endPos.x;
        } else {
            minPos.x = endPos.x;
            maxPos.x = startPos.x;
        }

        if (startPos.y <= endPos.y) {
            minPos.y = startPos.y;
            maxPos.y = endPos.y;
        } else {
            minPos.y = endPos.y;
            maxPos.y = startPos.y;
        }
        
        return new Rect (minPos, maxPos - minPos);
    }

    void ClearSelection () {
        for (int i = 0; i < selecteds.Count; i++) {
            SelectableUnit unit = selecteds[i].GetComponent<SelectableUnit> ();
            if (unit) {
                unit.isSelected = false;
            }
        }
        selecteds.Clear ();
    }
}
