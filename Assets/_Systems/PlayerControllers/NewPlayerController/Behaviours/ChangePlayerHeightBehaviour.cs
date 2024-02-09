using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerHeightBehaviour : FSMBehaviour
{
    [SerializeField] float playerHeight;
    [SerializeField] Rigidbody rb;
    [SerializeField] float downForce;

    float prevHeight;

    PlayerMovementFSM pm;

    public override void EnterBehaviour()
    {
		rb.AddForce(Vector3.down * downForce, ForceMode.VelocityChange);
		pm = fsm.GetComponent<PlayerMovementFSM>();
        prevHeight = pm.GetCurrentPlayerHeight();
        pm.SetCurrentPlayerHeight(playerHeight);
		
	}

    public override void ExitBehaviour()
    {
        pm.SetCurrentPlayerHeight(prevHeight);
    }
}
