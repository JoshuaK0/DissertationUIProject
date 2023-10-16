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
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        stateText.text = pm.GetCurrentState();
        speedTest.text = rb.velocity.magnitude.ToString("F1");
    }
}
