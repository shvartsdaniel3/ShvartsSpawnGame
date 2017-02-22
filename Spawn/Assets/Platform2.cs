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

	void Start ()
	{
		originalPos = transform.position;
		s = gameObject.GetComponentInParent (typeof(Switch)) as Switch;
	}

	void Update ()
	{
		if (s.pressed == true) {
			pressed = true;
		}
		if (pressed == true) {
			if (maxHeightReached == false && transform.position.y < maxHeight) {
				transform.position = new Vector2 (transform.position.x, transform.position.y + growSpeed);
			} else if (transform.position.y > originalPos.y) {
				maxHeightReached = true;
				if (timesCast == 0) {
					StartCoroutine ("Decrease");
					timesCast = 1;
				}
				if (waitBool == false) {
					transform.position = new Vector2 (transform.position.x, transform.position.y - growSpeed);
				}
			}
		}

		if (transform.position.y == originalPos.y) {
			timesCast = 0;
			maxHeightReached = false;
			pressed = false;
		}
	}

	IEnumerator Decrease ()
	{
		waitBool = true;
		yield return new WaitForSeconds (waitTime);
		waitBool = false;
	}
}
