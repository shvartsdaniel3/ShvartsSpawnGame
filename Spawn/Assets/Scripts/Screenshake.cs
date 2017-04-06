using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;

public class Screenshake : MonoBehaviour
{


	Vector3 defaultCameraPos;
	Vector3 weightedDirection;
	float screenshakeTimer = 0;
	float thisMagnitude = 0;
	AnalogGlitch glitch;


	void Start ()
	{
		defaultCameraPos = transform.position;
		glitch = gameObject.GetComponent<AnalogGlitch> ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (screenshakeTimer > 0) {
			Vector2 shakeDirection = Random.insideUnitCircle * thisMagnitude * Mathf.Clamp01 (screenshakeTimer);

			transform.position = defaultCameraPos + (Vector3)shakeDirection;

			screenshakeTimer -= Time.deltaTime;
		}
		if (Global.me.restart == true && glitch.scanLineJitter <= 0.5) {
			glitch.scanLineJitter += 0.1f;
			glitch.verticalJump += 0.1f;
			glitch.colorDrift += 0.3f;
		} else if (glitch.scanLineJitter > 0) {
			Global.me.restart = false;
			glitch.scanLineJitter -= 0.1f;
			glitch.verticalJump -= 0.1f;
			glitch.colorDrift -= 0.3f;
		} else if (glitch.scanLineJitter < 0) {
			glitch.scanLineJitter = 0;
			glitch.verticalJump = 0;
			glitch.colorDrift = 0;
		}
	}

	public void SetScreenshake (float magnitude, float duration, Vector3 direction)
	{
		thisMagnitude = magnitude;
		screenshakeTimer = duration;
		weightedDirection = weightedDirection;
	}

	public void SetScreenshake (float magnitude, float duration)
	{
		SetScreenshake (magnitude, duration, Vector3.zero);
	}


}
