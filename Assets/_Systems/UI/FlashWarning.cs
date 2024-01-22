using UnityEngine;

public class FlashWarning : MonoBehaviour
{
	public Material materialToFlash;
	public Color color1 = new Color(0, 1, 0, 1); // Example HDR color (bright green)
	public Color color2 = new Color(1, 0, 0, 1); // Example HDR color (bright red)
	public AnimationCurve colorCurve;
	public float warningLevel = 0.0f; // Ranges from 0 (no warning) to 1 (maximum warning)
	public float minFrequency = 1.0f; // Minimum frequency in Hz
	public float maxFrequency = 2.0f; // Maximum frequency in Hz
	public AnimationCurve frequencyCurve;
	public float minFlashDuration = 0.3f; // Minimum duration of each flash in seconds
	public float maxFlashDuration = 0.5f; // Maximum duration of each flash in seconds
	public AnimationCurve flashDurationCurve;
	public float emissionIntensity = 2.0f; // Intensity of the emission

	private float currentFrequency;
	private float currentFlashDuration;
	private float timer = 0.0f;
	private bool isEmissiveOn = false;

	void Update()
	{
		// Interpolate frequency and flash duration based on warning level
		if(warningLevel == 0)
		{
			UpdateEmissiveColor(Color.black * 0);
			return;
		}
		currentFrequency = Mathf.Lerp(minFrequency, maxFrequency, frequencyCurve.Evaluate(warningLevel));
		currentFlashDuration = Mathf.Lerp(maxFlashDuration, minFlashDuration, flashDurationCurve.Evaluate(warningLevel));

		// Update the timer
		timer += Time.deltaTime;

		// Calculate the current color based on the warning level
		Color currentColor = Color.Lerp(color1, color2, colorCurve.Evaluate(warningLevel)) * emissionIntensity;

		// Check if it's time to toggle the emissive state
		if (timer >= (isEmissiveOn ? currentFlashDuration : 1.0f / currentFrequency - currentFlashDuration))
		{
			isEmissiveOn = !isEmissiveOn;
			UpdateEmissiveColor(isEmissiveOn ? currentColor : Color.black * emissionIntensity);
			timer = 0.0f;
		}
	}

	void UpdateEmissiveColor(Color color)
	{
		// Make sure the material has an emissive property
		if (materialToFlash.HasProperty("_EmissionColor"))
		{
			// Set the HDR color
			materialToFlash.SetColor("_EmissionColor", color * Mathf.LinearToGammaSpace(emissionIntensity));
			// Update the material's global illumination
		}
	}

	public void SetWarningLevel(float newWarningLevel)
	{
		warningLevel = Mathf.Clamp(newWarningLevel, 0.0f, 1.0f);
	}
}
