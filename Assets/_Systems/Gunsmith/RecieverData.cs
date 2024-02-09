using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecieverData : MonoBehaviour
{
	[SerializeField] Transform gripSocket;

	public Transform GetGripSocket()
	{
		return gripSocket;
	}
}
