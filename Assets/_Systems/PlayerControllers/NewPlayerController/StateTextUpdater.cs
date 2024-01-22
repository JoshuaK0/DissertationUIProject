using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateTextUpdater : MonoBehaviour
{
    [SerializeField] TMP_Text stateText;
    [SerializeField] TMP_Text speedTest;
    [SerializeField] FiniteStateMachine pm;
    [SerializeField] Rigidbody rb;
    // Update is called once per frame
    void FixedUpdate()
    {
        speedTest.text = rb.velocity.magnitude.ToString();
		stateText.text = pm.GetCurrentState();
	}
}
