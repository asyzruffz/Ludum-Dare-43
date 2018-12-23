using UnityEngine;

public class BossAttack : StateMachineBehaviour {

    [SerializeField]
    float attackDelay = 2;

    BossController controller;
    GrudgeList grudge;
    FieldOfView fov;
    float timer;

    void Init (Animator animator) {
        if (controller == null) {
            controller = animator.GetComponent<BossController> ();
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
        timer = attackDelay;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (timer >= attackDelay) {
            controller.Attack ();
            timer = 0;
        }

        if (grudge.HasTarget ()) {
            if (!fov.IsVisible (grudge.GetTarget ())) {
                animator.SetTrigger ("Pursuit");
            }
        } else {
            animator.SetTrigger ("Wander");
        }

        timer += Time.deltaTime;
    }
}
