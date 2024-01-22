using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwarenessManager : MonoBehaviour
{	
	[Header("Awareness")]
	[SerializeField] float currentAwareness;
	[SerializeField] float maxAwareness;
	[SerializeField] float currentMinAwareness;

	[SerializeField] float awarenessLoseRate;
	[SerializeField] float awarenessLoseDelay;

	public delegate void onAwarenessChange();

	public event onAwarenessChange OnAwarenessGainedLocally;

	float awarenessLoseTimer;
	public void GainAwarenessOverTime(float awarenessGainAmount)
	{
		if (currentAwareness >= maxAwareness)
		{
			currentAwareness = maxAwareness;
		}
		else
		{
			currentAwareness += awarenessGainAmount * Time.deltaTime;
		}
		OnAwarenessGainedLocally?.Invoke();
		awarenessLoseTimer = awarenessLoseDelay;
		
	}

	public void GainInstantaneousAwareness(float awarenessGainAmount)
	{
		if (currentAwareness >= maxAwareness)
		{
			currentAwareness = maxAwareness;
		}
		else
		{
			currentAwareness += awarenessGainAmount;
		}
		OnAwarenessGainedLocally?.Invoke();
		awarenessLoseTimer = awarenessLoseDelay;
	}

	public void LoseAwareness(float awarenessLoseAmount)
	{
		if (currentAwareness > currentMinAwareness)
		{
			currentAwareness -= awarenessLoseAmount * Time.deltaTime;
		}
		else
		{
			currentAwareness = currentMinAwareness;
		}
	}

	public float GetCurrentAwareness()
	{
		return currentAwareness;
	}

	public float GetMaxAwareness()
	{
		return maxAwareness;
	}

	public void SetMinAwareness(float newAwareness)
	{
		currentMinAwareness = newAwareness;
		if(currentAwareness < currentMinAwareness)
		{
			currentAwareness = currentMinAwareness;
		}
	}

	public float GetCurrentMinAwareness()
	{
		return currentMinAwareness;
	}

	public void SetCurrentAwareness(float newAwareness)
	{
		currentAwareness = newAwareness;
	}

	void Update()
	{
		if(awarenessLoseTimer > 0 )
		{
			awarenessLoseTimer -= Time.deltaTime;
		}
		else
		{
			awarenessLoseTimer = 0;
			LoseAwareness(awarenessLoseRate);
		}
	}
}
