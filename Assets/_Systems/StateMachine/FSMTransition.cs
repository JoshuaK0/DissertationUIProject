using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FSMTransition
{
    [SerializeField]  string name;
    [SerializeField] FSMState goesToState;
    [SerializeField] [TextArea(3, 10)] string when;
    [SerializeField] List<FSMDecision> decisions;

    public List<FSMDecision> GetDecisions()
    {
        return decisions;
    }
    public FSMState GetTransitionState()
    {
        return goesToState;
    }
}
