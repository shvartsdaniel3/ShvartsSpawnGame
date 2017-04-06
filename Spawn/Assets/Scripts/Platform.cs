using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
	Switch s;
	public float maxHeight;
	public float growSpeed;
	Vector2 originalPos;
	public bool down;
	Screenshake shake;
	public AudioClip grumble;
	AudioSource aus;
	bool playing = false;

	void Start ()
	{
		originalPos = transform.position;
		s = gameObject.GetComponentInParent (typeof(Switch)) as Switch;
		if (down) {
			growSpeed = -growSpeed;
		}
		shake = GameObject.FindObjectOfType<Screenshake> ();

	}

	void Update ()
	{
		if (s.pressed == true) {
			if (down) {
				if (transform.position.y > maxHeight) {
					shake.SetScreenshake (0.3f, 0.1f);
					if (!playing) {
						Global.me.sound.PlaySound (grumble, 0.4f, true);
						aus = Global.me.sound.aus;
						playing = true;
					}
					transform.position = new Vector2 (transform.position.x, transform.position.y + growSpeed);
				} else if (transform.position.y <= maxHeight) {
					aus.volume -= 0.1f;
					playing = false;
				}
			} else {
				if (transform.position.y < maxHeight) {
					shake.SetScreenshake (0.3f, 0.1f);
					if (!playing) {
						Global.me.sound.PlaySound (grumble, 0.4f, true);
						aus = Global.me.sound.aus;
						playing = true;
					}
					transform.position = new Vector2 (transform.position.x, transform.position.y + growSpeed);
				} else if (transform.position.y >= maxHeight) {
					aus.volume -= 0.1f;
					playing = false;
				}
			}
		} else {
			if (down) {
				if (transform.position.y < originalPos.y) {
					shake.SetScreenshake (0.3f, 0.1f);
					if (!playing) {
						Global.me.sound.PlaySound (grumble, 0.4f, true);
						aus = Global.me.sound.aus;
						playing = true;
					}
					transform.position = new Vector2 (transform.position.x, transform.position.y - growSpeed);
				} else if (transform.position.y >= originalPos.y && aus != null) {
					aus.volume -= 0.1f;
					playing = false;
				}
			} else {
				if (transform.position.y > originalPos.y) {
					shake.SetScreenshake (0.3f, 0.1f);
					if (!playing) {
						Global.me.sound.PlaySound (grumble, 0.4f, true);
						aus = Global.me.sound.aus;
						playing = true;
					}
					transform.position = new Vector2 (transform.position.x, transform.position.y - growSpeed);
				} else if (transform.position.y <= originalPos.y && aus != null) {
					aus.volume -= 0.1f;
					playing = false;
				}
			}
		}
	}
}
