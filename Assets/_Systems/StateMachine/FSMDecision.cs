using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class FSMDecision : MonoBehaviour
{
    protected FiniteStateMachine fsm;
    [SerializeField] bool invert;

    [SerializeField] bool passed;

    public void SetFSM(FiniteStateMachine newFSM)
    {
        fsm = newFSM;
    }
    public virtual void InitDecision()
    {
    }
    public bool Evaluate()
    {
        if (invert)
        {
            return !DecisionEvaluate();
        }
        else
        {
            return DecisionEvaluate();
        }
    }

	public bool FixedUpdateEvaluate()
	{
		if (invert)
		{
			return !FixedUpdateDecisionEvaluate();
		}
		else
		{
			return FixedUpdateDecisionEvaluate();
		}
	}

    public virtual bool DecisionEvaluate()
    { 
        if(invert)
        {
            return true;
        }
        else
        {
			return false;
		}
    }

    public virtual bool FixedUpdateDecisionEvaluate()
    {
		if (invert)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void UpdateResult(bool result)
    {
        passed = result;
    }
}
