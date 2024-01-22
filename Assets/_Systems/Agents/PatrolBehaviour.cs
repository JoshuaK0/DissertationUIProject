using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class PatrolBehaviour : FSMBehaviour
{
	public List<Transform> waypoints; // Array of waypoints
	public float waitTime = 3f;   // Waiting time at each waypoint
	private int waypointIndex = 0; // Current waypoint index
	private NavMeshAgent agent;
	private float waitTimer = 0;  // Timer for waiting
	private bool isWaiting = false; // Flag to check if the agent is waiting
	public float rotationSpeed = 5f; // Speed of rotation

	public SquadPatrolManager patrolManager;
	PatrolRoute route;
	CombatantFSM combatantFSM;

	public override void EnterBehaviour()
	{
		combatantFSM = fsm.GetComponent<CombatantFSM>();
		
		float closestDist = Mathf.Infinity;
		foreach (PatrolRoute potentialRoute in patrolManager.GetRoutes())
		{
			if(potentialRoute.IsOccupied()) continue;
			if (Vector3.Distance(transform.position, potentialRoute.GetRouteLocation()) < closestDist)
			{
				closestDist = Vector3.Distance(transform.position, potentialRoute.GetRouteLocation());
				route = potentialRoute;
			}
		}
		route.IsOccupied();
		route.SetOccupied(true);
		waypoints = route.GetWaypoints();
		
		agent = combatantFSM.GetCombatantServices().GetNavMeshAgent();
		MoveToNextWaypoint();
	}

	public override void UpdateBehaviour()
	{
		// Check if agent is close to the waypoint and not currently waiting
		if (!isWaiting && !agent.pathPending && agent.remainingDistance < 0.5f)
		{
			agent.isStopped = true; // Stop the agent's movement
			agent.updateRotation = false; // Stop automatic rotation
			isWaiting = true;
			waitTimer = waitTime;
		}

		// Handle waiting
		if (isWaiting)
		{
			waitTimer -= Time.deltaTime;
			// Rotate towards the waypoint's forward direction
			RotateTowards(waypoints[waypointIndex].forward);
			
			if (waitTimer <= 0)
			{
				isWaiting = false;
				agent.isStopped = false; // Resume the agent's movement
				agent.updateRotation = true; // Resume automatic rotation
				MoveToNextWaypoint();
			}
		}
	}

	void RotateTowards(Vector3 direction)
	{
		// Normalize the direction vector and ensure it's horizontal (y = 0)
		Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z).normalized;

		// Calculate the rotation needed to look in the horizontal direction
		Quaternion targetRotation = Quaternion.LookRotation(horizontalDirection);

		// Slerp the rotation, affecting only the Y-axis
		combatantFSM.transform.rotation = Quaternion.Slerp(combatantFSM.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
	}

	void MoveToNextWaypoint()
	{
		if (waypoints.Count == 0)
			return;
		
		waypointIndex = (waypointIndex + 1) % waypoints.Count;
		combatantFSM.SetNavDestination(waypoints[waypointIndex].position);
	}

	public override void ExitBehaviour()
	{
		agent.isStopped = false; // Resume the agent's movement
		agent.updateRotation = true; // Resume automatic rotation
		route.SetOccupied(false);
	}

	void OnDrawGizmosSelected()
	{
		if (route == null) return;

		Gizmos.color = Color.yellow;
		for(int i = 0; i < waypoints.Count; i++)
		{
			Gizmos.DrawSphere(waypoints[i].position, 0.5f);
			if (i == waypoints.Count - 1)
				Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
			else
				Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
		}
	}
}
