using UnityEngine;
using System.Collections;

public class DemoManagerControl : MonoBehaviour 
{
	public GameObject cursorObject;

	public GameObject welcomeObject;
	public float countdownTickTime;
	public int numCountdownTicks;

	public GameObject horizonLineObject;
	public float blinkTickTime;
	public int numHorizonBlinks;

	private UnityEngine.UI.Image cursorImage;
	private UnityEngine.UI.Text welcomeText;
	private UnityEngine.UI.Image horizonLineImage;

	// Use this for initialization
	void Start() 
	{
		StartCoroutine("StartListener");
	}
	
	// Update is called once per frame
	void Update() 
	{
	
	}

	IEnumerator StartListener()
	{
		while(true)
		{
			if(Input.GetButtonDown("Fire1"))
			{
				StartCoroutine("WelcomeCountdown");
				StopCoroutine("StartListener");
			}
			yield return null;
		}
	}


	IEnumerator WelcomeCountdown()
	{
		welcomeText = welcomeObject.GetComponent<UnityEngine.UI.Text>();

		// Spawn "Welcome" text (or enable if UI)
		 

		yield return new WaitForSeconds(countdownTickTime);

		// Turn on cursor
		ShowCursor(true);

		for(int ii=0; ii<numCountdownTicks; ii++)
		{
			welcomeText.text += " .";
			yield return new WaitForSeconds(countdownTickTime);
		}

		ShowCursor(false);
		StopCoroutine("WelcomeCountdown");
		ResetOrientation();
	}


	public void ResetOrientation()
	{
		// Horizon line confirmation blink
		StartCoroutine("HorizonBlink");

		// Confirmation sound

		// Reset game world orientation

	}


	IEnumerator HorizonBlink()
	{
		horizonLineImage = horizonLineObject.GetComponent<UnityEngine.UI.Image>();

		// Enable horizon line
		horizonLineImage.enabled = true;

		for(int ii=0; ii<numHorizonBlinks; ii++)
		{
			if(horizonLineImage.enabled)
			{
				// Disable horizon line
				horizonLineImage.enabled = false;
			}
			else
			{
				// Enable horizon line
				horizonLineImage.enabled = true;
			}

			yield return new WaitForSeconds(blinkTickTime);
		}

		StopCoroutine("HorizonBlink");
	}


	public void ShowCursor(bool show)
	{
		cursorImage = cursorObject.GetComponent<UnityEngine.UI.Image>();

		if(show)
		{
			// Enable cursor UI
			cursorImage.enabled = true;
		}
		else
		{
			// Disable cursor UI
			cursorImage.enabled = false;
		}
	}

}