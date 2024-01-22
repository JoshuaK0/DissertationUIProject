using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMState : MonoBehaviour
{
    protected FiniteStateMachine fsm;

    [SerializeField] List<FSMBehaviour> behaviours = new List<FSMBehaviour>();
    [SerializeField] List<FSMTransition> transitions = new List<FSMTransition>();

    public void EnterState(FiniteStateMachine newFSM)
    {
        fsm = newFSM;
        foreach (FSMBehaviour behaviour in behaviours)
        {
            behaviour.SetFSM(fsm);
        }

        foreach (FSMBehaviour behaviour in behaviours)
        {
            behaviour.EnterBehaviour();
        }

        foreach (FSMTransition transition in transitions)
        {
            foreach (FSMDecision decision in transition.GetDecisions())
            {
                decision.SetActive(true);
                decision.SetFSM(fsm);
                decision.InitDecision();
            }
        }
    }

    public void UpdateState()
    {
        foreach (FSMTransition transition in transitions)
        {
            bool canTransition = true;
            foreach (FSMDecision decision in transition.GetDecisions())
            {
                bool decisionResult = decision.Evaluate();
                if (!decisionResult)
                {
                    canTransition = false;
                }
                decision.UpdateResult(decisionResult);
            }
            if(canTransition)
            {
                fsm.ChangeState(transition.GetTransitionState(), transition);
                return;
            }
        }

        foreach (FSMBehaviour behaviour in behaviours)
        {
            behaviour.UpdateBehaviour();
        }
    }

	public void FixedUpdateState()
	{
		foreach (FSMBehaviour behaviour in behaviours)
		{
			behaviour.FixedUpdateBehaviour();
		}
	}

	public void ExitState()
    {
        foreach (FSMBehaviour behaviour in behaviours)
        {
            behaviour.ExitBehaviour();
        }
		foreach (FSMTransition transition in transitions)
		{
			foreach (FSMDecision decision in transition.GetDecisions())
			{
				decision.SetActive(false);
			}
		}
	}
}
