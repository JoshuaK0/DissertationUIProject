using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDownDecision : FSMDecision
{
    enum EvaluationType {getButtonDown, getButtonUp, getButton};
    [SerializeField] EvaluationType evaluationType;
    [SerializeField] KeyCode key;
    public override bool DecisionEvaluate()
    {
        if (evaluationType == EvaluationType.getButtonDown)
        {
            return Input.GetKeyDown(key);
        }
        else if (evaluationType == EvaluationType.getButtonUp)
        {
            return Input.GetKeyUp(key);
        }
        else if (evaluationType == EvaluationType.getButton)
        {
            return Input.GetKey(key);
        }
        return false;
    }
}
