using UnityEngine;
using UnityEngine.AI;

public class BossWander : StateMachineBehaviour {

    [SerializeField]
    float wanderRadius = 5;

    [SerializeField]
    float wanderPeriod = 3;

    [SerializeField]
    float waitBeforeAttack = 2;

    NavMeshAgent agent;
    GrudgeList grudge;
    FieldOfView fov;
    float waitTimer;
    float wanderTimer;

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
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Init (animator);
        waitTimer = 0;
        wanderTimer = wanderPeriod;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (grudge.HasTarget ()) {
            if (waitTimer >= waitBeforeAttack) {
                if (fov.IsVisible (grudge.GetTarget ())) {
                    animator.SetTrigger ("Attack");
                } else {
                    animator.SetTrigger ("Pursuit");
                }
            }

            waitTimer += Time.deltaTime;
        }

        if (wanderTimer >= wanderPeriod) {
            agent.SetDestination (RandomDestination ());
            wanderTimer = 0;
        }

        wanderTimer += Time.deltaTime;
    }

    Vector3 RandomDestination () {
        Vector2 randDirection = Random.insideUnitCircle.normalized;
        Vector3 randTarget = new Vector3 (randDirection.x, 0, randDirection.y) * wanderRadius;

        NavMeshHit navHit;
        if (NavMesh.SamplePosition (agent.transform.position + randTarget, out navHit, Mathf.Infinity, 1)) {
            return navHit.position;
        }

        return agent.transform.position;
    }
}
