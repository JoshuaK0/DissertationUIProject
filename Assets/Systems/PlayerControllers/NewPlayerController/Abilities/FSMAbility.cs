using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMAbility : FSMBehaviour
{
    [SerializeField] string abilityName;
    [SerializeField] float abilityCooldown;
    bool isAbilityReady = true;

    public bool GetIsAbilityReady()
    {
        return isAbilityReady;
    }

    public void DoAbility()
    {
        if (isAbilityReady)
        {
            isAbilityReady = false;
            Invoke("AbilityCoolDownReset", abilityCooldown);
        }
    }

    public virtual void AbilityCoolDownReset()
    {
        isAbilityReady = true;
    }
}
