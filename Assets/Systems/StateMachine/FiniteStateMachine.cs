using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class FiniteStateMachine : MonoBehaviour
{
    [Header("FSM Properties")]

    [SerializeField] FSMState startState;

    [SerializeField] protected FSMState currentState;

    int tracebackLength;

    [SerializeField] List<FSMTraceback> traceback = new List<FSMTraceback>();

    FSMState previousState;

    bool exiting;

    public virtual void Start()
    {
        tracebackLength = traceback.Count;
        ChangeState(startState);
    }

    public void ChangeState(FSMState newState)
    {
        FSMState lastState = currentState;
        DoChange(newState);
        traceback.Add(new FSMTraceback(currentState, lastState, null));
    }

    public void ChangeState(FSMState newState, FSMTransition transition)
    {
        
        if(newState == null)
        {
            Debug.Log("State Missing");
            return;
        }
        FSMState lastState = currentState;
        DoChange(newState);
        AddTraceback(currentState, lastState, transition);
    }

    void DoChange(FSMState newState)
    {
        exiting = true;
        if (currentState != null)
        {
            currentState.ExitState();
            previousState = currentState;
        }
        currentState = newState;
        currentState.EnterState(this);
    }

    void AddTraceback(FSMState newState, FSMState lastState, FSMTransition transition)
    {
        traceback.Add(new FSMTraceback(newState, lastState, transition));
        while (traceback.Count > tracebackLength)
        {
            traceback.RemoveAt(0);
        }
    }

    public virtual void Update()
    {
        if(exiting)
        {
            exiting = false;
            return;
        }
        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }
    
    public virtual void FixedUpdate()
    {
		if (currentState != null)
		{
			currentState.FixedUpdateState();
		}
	}

    public virtual void OnDrawGizmos()
    {
        if(currentState!=null)
        {
            Handles.Label(transform.position, currentState.ToString().Remove(currentState.ToString().Length - 8));
        }
    }

    public string GetCurrentState()
    {
        return currentState.transform.name;
    }

    public FSMState GetPreviousState()
    {
        return previousState;
    }
}

[System.Serializable]

public class FSMTraceback
{
    public string stateName;
    public FSMState toState;
    public FSMState fromState;
    public FSMTransition transition;

    public FSMTraceback(FSMState toState, FSMState fromState, FSMTransition transition)
    {
        if (fromState != null)
        {
            this.stateName = toState.ToString().Remove(toState.ToString().Length - 8) + " from " + fromState.ToString().Remove(fromState.ToString().Length - 8);
        }
        else
        {
            this.stateName = "Set state to " + toState.ToString().Remove(toState.ToString().Length - 8);
        }
        this.toState = toState;
        this.fromState = fromState;
        this.transition = transition;
    }
}
