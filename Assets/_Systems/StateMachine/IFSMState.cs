using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFSMState
{
	public abstract void EnterState(FiniteStateMachine newFSM);

	public abstract void UpdateState();

	public abstract void FixedUpdateState();

	public abstract void ExitState();
}
