using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    [SerializeField] private float patrolSpeed = 5f;
    [SerializeField] private Transform route;
    
    private List<Transform> waypoints = new();
    private Transform currentWaypoint;
    private int currentWaypointIndex;
    
    public override void OnEnterState(FSMController controller)
    {
        base.OnEnterState(controller);
        
        foreach (Transform point in route)
        {
            waypoints.Add(point);
        }

        currentWaypointIndex = 0;
        currentWaypoint = waypoints[currentWaypointIndex];
    }
    
    public override void OnUpdateState()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, patrolSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentWaypoint.position) < 0.1f)
        {
            CalculateNextWaypoint();
        }
    }

    private void CalculateNextWaypoint()
    {
        currentWaypointIndex++;
        currentWaypointIndex %= waypoints.Count;
        currentWaypoint = waypoints[currentWaypointIndex];
    }
    
    public override void OnExitState()
    {
        
    }
}
