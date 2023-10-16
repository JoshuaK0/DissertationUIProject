using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerHeightBehaviour : FSMBehaviour
{
    [SerializeField] float playerHeight;

    float prevHeight;

    PlayerMovementFSM pm;

    public override void EnterBehaviour()
    {
        pm = fsm.GetComponent<PlayerMovementFSM>();
        prevHeight = pm.GetCurrentPlayerHeight();
        pm.SetCurrentPlayerHeight(playerHeight);
    }

    public override void ExitBehaviour()
    {
        pm.SetCurrentPlayerHeight(prevHeight);
    }
}
