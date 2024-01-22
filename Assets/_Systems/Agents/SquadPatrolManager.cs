using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadPatrolManager : MonoBehaviour
{
	[SerializeField] List<PatrolRoute> routes = new List<PatrolRoute>();

	public List<PatrolRoute> GetRoutes()
	{
		return routes;
	}
}
