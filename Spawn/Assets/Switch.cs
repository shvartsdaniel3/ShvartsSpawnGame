using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
	Vector2 pressedPos;
	Vector2 OriginalPos;
	public bool pressed = false;

	void Start ()
	{
		OriginalPos = transform.position;
		pressedPos = new Vector2 (transform.position.x, transform.position.y - 0.25f);
	}

	void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.gameObject.tag == "0" || collision.gameObject.tag == "1" || collision.gameObject.tag == "2" || collision.gameObject.tag == "3") {
			transform.position = pressedPos;
			pressed = true;
		}
	}

	void OnTriggerExit2D (Collider2D collision)
	{
		if (collision.gameObject.tag == "0" || collision.gameObject.tag == "1" || collision.gameObject.tag == "2" || collision.gameObject.tag == "3") {
			transform.position = OriginalPos;
			pressed = false;
		}
	}
}
