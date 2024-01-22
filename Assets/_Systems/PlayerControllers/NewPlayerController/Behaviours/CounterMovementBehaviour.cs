using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMovementAdvanced;

public class CounterMovementBehaviour : FSMBehaviour
{
    [SerializeField] Vector2 drag;
    [SerializeField] bool disableInAir;
    [SerializeField] GroundDetector groundDetector;

	PlayerMovementFSM pm;

	public override void EnterBehaviour()
    {
		pm = fsm.GetComponent<PlayerMovementFSM>();
	}

    public override void FixedUpdateBehaviour()
    {
		if (disableInAir && (!groundDetector.IsGrounded() || pm.IsJumping()))
		{
			pm.SetDrag(Vector3.zero);
		}
        else
        {
			pm.SetDrag(drag);
		}
    }
}
