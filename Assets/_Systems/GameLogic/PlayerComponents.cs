using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponents : MonoBehaviour
{
    static PlayerComponents instance;

	//public AgentHealthManager healthManager;
	public static PlayerComponents Instance()
	{
		return instance;
	}

	void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
		else
		{
			Destroy(gameObject);
		}
	}
}
