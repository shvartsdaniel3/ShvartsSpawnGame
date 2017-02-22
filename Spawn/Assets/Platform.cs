using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
	Switch s;
	public float maxHeight;
	public float growSpeed;
	Vector2 originalPos;

	void Start ()
	{
		originalPos = transform.position;
		s = gameObject.GetComponentInParent (typeof(Switch)) as Switch;
	}

	void Update ()
	{
		if (s.pressed == true) {
			if (transform.position.y < maxHeight) {
				transform.position = new Vector2 (transform.position.x, transform.position.y + growSpeed);
			}
		} else {
			if (transform.position.y > originalPos.y) {
				transform.position = new Vector2 (transform.position.x, transform.position.y - growSpeed);
			}
		}
	}
}
