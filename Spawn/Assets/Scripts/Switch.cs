using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
	Vector2 pressedPos;
	Vector2 OriginalPos;
	public bool pressed = false;
	GameObject player;

	void Start ()
	{
		OriginalPos = transform.position;
		pressedPos = new Vector2 (transform.position.x, transform.position.y - 0.25f);
	}

	void Update ()
	{
		if (player == null) {
			pressed = false;
			transform.position = OriginalPos;
		}
	}


	void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.gameObject.tag == "0" || collision.gameObject.tag == "1" || collision.gameObject.tag == "2" || collision.gameObject.tag == "3" || collision.gameObject.tag == "Enemy") {
			if (pressed == false) {
				player = collision.gameObject;
				transform.position = pressedPos;
				pressed = true;
			}
		}
	}

	void OnTriggerExit2D (Collider2D collision)
	{
		if (collision.gameObject == player) {
			transform.position = OriginalPos;
			pressed = false;
		}
	}
}
