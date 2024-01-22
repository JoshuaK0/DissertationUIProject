using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnitInitializer : MonoBehaviour
{
	static UnitInitializer instance;
	public static UnitInitializer Instance
	{
		get
		{
			return instance;
		}
	}

	List<SquadManager> squadManagers = new List<SquadManager>();

	void Awake()
	{
		instance = this;
	}
	public void InitializeUnits()
	{
/*		
		var spawners = FindObjectsOfType<MonoBehaviour>().OfType<IUnitSpawner>();
		foreach (IUnitSpawner spawner in spawners)
		{
			if (spawner.CanSpawn())
			{
				
				spawner.SpawnUnit();
			}
		}

		foreach(EnemySoldierFSM fsm in FindObjectsOfType<EnemySoldierFSM>())
		{
			int id = fsm.gameObject.GetComponent<SoldierIdentifier>().GetTeamID();
			if (id+1 > squadManagers.Count)
			{
				GameObject newTeam = new GameObject();
				newTeam.transform.SetParent(transform);
				newTeam.name = "Team " + id;
				SquadManager newSquadManager = newTeam.AddComponent<SquadManager>();
				squadManagers.Add(newSquadManager);
				fsm.SetSquadManager(newSquadManager);
				newSquadManager.RegisterMember(fsm.GetComponent<SoldierIdentifier>());
				UnitManager.Instance().RegisterSquad(newSquadManager);
			}
			else
			{
				fsm.SetSquadManager(squadManagers[id]);
				squadManagers[id].RegisterMember(fsm.GetComponent<SoldierIdentifier>());
			}
		}*/
	}
}
