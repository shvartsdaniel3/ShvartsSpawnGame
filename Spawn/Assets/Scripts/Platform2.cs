using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform2 : MonoBehaviour
{
	Switch s;
	public float maxHeight;
	public float growSpeed;
	Vector2 originalPos;
	public float waitTime;
	int timesCast = 0;
	bool maxHeightReached = false;
	bool waitBool = false;
	bool pressed = false;
	Screenshake shake;
	public AudioClip grumble;
	AudioSource aus;
	bool playing = false;
	bool up = false;
	bool down = false;

	void Start ()
	{
		originalPos = transform.position;
		s = gameObject.GetComponentInParent (typeof(Switch)) as Switch;
		shake = GameObject.FindObjectOfType<Screenshake> ();
	}

	void Update ()
	{
		if (s.pressed == true) {
			pressed = true;
		}
		if (pressed == true) {
			if (maxHeightReached == false && transform.position.y < maxHeight) {
				shake.SetScreenshake (0.3f, 0.1f);
				up = true;
				if (playing == false) {
					Global.me.sound.PlaySound (grumble, 0.4f, true);
					playing = true;
					aus = Global.me.sound.aus;
				}
				transform.position = new Vector2 (transform.position.x, transform.position.y + growSpeed);
			} else if (transform.position.y > originalPos.y) {
				if (up) {
					aus.volume -= 0.1f;
				}
				if (aus.volume <= 0) {
					up = false;
				}
				maxHeightReached = true;
				if (timesCast == 0) {
					StartCoroutine ("Decrease");
					timesCast = 1;
					playing = false;
				}
				if (waitBool == false) {
					shake.SetScreenshake (0.3f, 0.1f);
					if (playing == false) {
						Global.me.sound.PlaySound (grumble, 0.4f, true);
						aus = Global.me.sound.aus;
						playing = true;
						down = true;
					}
					transform.position = new Vector2 (transform.position.x, transform.position.y - growSpeed);
				} 
			}
		}

		if (transform.position.y == originalPos.y) {
			if (down) {
				aus.volume -= 0.1f;
			}
			timesCast = 0;
			maxHeightReached = false;
			pressed = false;
			playing = false;
			//s.pressed = false;
		}
	}

	IEnumerator Decrease ()
	{
		//shake.SetScreenshake (0, 0);
		waitBool = true;
		yield return new WaitForSeconds (waitTime);
		waitBool = false;
	}
}
