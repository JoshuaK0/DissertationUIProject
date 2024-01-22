using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRadioSoundBehaviour : FSMBehaviour
{
	[SerializeField] AudioSource audioSource;
	[SerializeField] Vector2 minMaxPitch;
	[SerializeField] float probability;
	[SerializeField] AudioClip[] clips;
	[SerializeField] AudioClip radioEnd;

	bool shouldPlay;

	public override void EnterBehaviour()
	{
		shouldPlay = Random.Range(0f, 1f) <= probability;
		
		if(shouldPlay)
		{
			audioSource.pitch = Random.Range(minMaxPitch.x, minMaxPitch.y);
			audioSource.clip = clips[Random.Range(0, clips.Length)];
			audioSource.Play();
		}
	}
}
