using UnityEngine;

public class FSMController : MonoBehaviour
{
    private State currentState;
    
    private PatrolState patrolState;
    private ChaseState chaseState;
    private AttackState attackState;

    private void Awake()
    {
        patrolState = GetComponent<PatrolState>();
        chaseState = GetComponent<ChaseState>();
        attackState = GetComponent<AttackState>();
        
        currentState = patrolState;
        currentState.OnEnterState(this);
    }

    private void Update()
    {
        if (currentState is null)
        {
            return;
        }
        
        currentState.OnUpdateState();
    }

    public void ChangeState(State newState)
    {
        if (currentState != null)
        {
            currentState.OnExitState();
        }
        
        currentState = newState;
        currentState.OnEnterState(this);
    }
}
