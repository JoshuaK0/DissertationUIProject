using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawnBlock : MonoBehaviour, IUnitSpawner
{
	[SerializeField] GameObject unitGameObject;
	[SerializeField] GameObject gameObjectToDestroy;
	[SerializeField] int unitTeam;

	public bool CanSpawn()
	{
		return true;
	}

	public void SpawnUnit()
	{
/*		GameObject newUnit = Instantiate(unitGameObject, transform.position, transform.rotation);
		newUnit.GetComponent<SoldierIdentifier>().SetTeamID(unitTeam);
		Destroy(gameObjectToDestroy);*/
	}
}
