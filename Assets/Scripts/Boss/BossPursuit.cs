using UnityEngine;
using UnityEngine.AI;

public class BossPursuit : StateMachineBehaviour {

    NavMeshAgent agent;
    GrudgeList grudge;
    FieldOfView fov;

    void Init (Animator animator) {
        if (agent == null) {
            agent = animator.GetComponent<NavMeshAgent> ();
        }
        if (grudge == null) {
            grudge = animator.GetComponent<GrudgeList> ();
        }
        if (fov == null) {
            fov = animator.GetComponent<FieldOfView> ();
        }
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Init (animator);
		if (grudge.HasTarget ()) {
			agent.SetDestination (grudge.GetTarget ().position);
		}
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (grudge.HasTarget ()) {
			if (fov.IsVisible (grudge.GetTarget ())) {
				animator.SetTrigger ("Attack");
			}
        } else {
			animator.SetTrigger ("Wander");
		}
    }
}
