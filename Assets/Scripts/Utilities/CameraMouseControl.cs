using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseControl : MonoBehaviour {

    [Header("Scroll")]
    [SerializeField]
    int borderScrollWidth = 50;
    [SerializeField]
    float scrollSpeed = 10;

    [SerializeField]
    bool limitScroll;
    [SerializeField]
    [ConditionalHide("limitScroll", true)]
    Vector3 minScroll, maxScroll;

    [Header("Zoom")]
    [SerializeField]
    float zoomSpeed = 10;

    Vector3 scrollDirection;
    
	void Update () {
        scrollDirection = Vector3.zero;

        if (Input.mousePosition.x < borderScrollWidth) {
            scrollDirection.x = -1;
        } else if (Input.mousePosition.x > Screen.width - borderScrollWidth) {
            scrollDirection.x = 1;
        }

        if (Input.mousePosition.y < borderScrollWidth) {
            scrollDirection.z = -1;
        } else if (Input.mousePosition.y > Screen.height - borderScrollWidth) {
            scrollDirection.z = 1;
        }

        scrollDirection.Normalize ();

        transform.Translate (scrollDirection * scrollSpeed * Time.deltaTime, Space.World);
        if (limitScroll) {
            transform.position = new Vector3 (
                Mathf.Clamp (transform.position.x, minScroll.x, maxScroll.x),
                Mathf.Clamp (transform.position.y, minScroll.y, maxScroll.y),
                Mathf.Clamp (transform.position.z, minScroll.z, maxScroll.z));
        }

        float scrollDelta = Input.GetAxis ("Mouse ScrollWheel");
        transform.Translate (scrollDelta * transform.forward * zoomSpeed * Time.deltaTime, Space.Self);
    }
}
