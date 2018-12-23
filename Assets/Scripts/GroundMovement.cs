using UnityEngine;
using UnityEngine.AI;

public class GroundMovement : MonoBehaviour {

    [SerializeField]
    DestinationMark destMark;
    
    void Update () {
        if (Input.GetMouseButtonDown (1)) {
            Vector3 mousePos = Input.mousePosition;

            // Set the world click position
            Ray ray = Camera.main.ScreenPointToRay (mousePos);
            RaycastHit hit;
            if (Physics.Raycast (ray, out hit)) {
                NavMeshHit navHit;
                if (NavMesh.SamplePosition (hit.point, out navHit, Mathf.Infinity, 1)) {
                    MoveTo (navHit.position);
                } else {
                    MoveTo (hit.point);
                }
            }
        }
    }

    public void MoveTo (Vector3 pos) {
        for (int i = 0; i < RectangleSelect.selecteds.Count; i++) {
            NavMeshAgent agent = RectangleSelect.selecteds[i].GetComponent<NavMeshAgent> ();
            if (agent) {
                agent.SetDestination (pos);
            }

            destMark.transform.position = pos;
            destMark.Reset ();
        }
    }
}
