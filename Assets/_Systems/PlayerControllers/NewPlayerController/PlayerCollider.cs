using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] PlayerMovementFSM pm;

    public Rigidbody GetRigidbody()
    {
        return rb;
    }

    public PlayerMovementFSM GetPlayerMovement()
    {
        return pm;
    }
}
