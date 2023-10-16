using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetKeyStateDecision : FSMDecision
{
    [SerializeField] KeyCode keyToCheck;
    enum KeyState {Down, Up, Held}

    [SerializeField] KeyState keyState;

    public override bool DecisionEvaluate()
    {
        switch (keyState)
        {
            case KeyState.Down:
                return Input.GetKeyDown(keyToCheck);
            case KeyState.Up:
                return Input.GetKeyUp(keyToCheck);
            case KeyState.Held:
                return Input.GetKey(keyToCheck);
            default:
                return false;
        }
    }
}
