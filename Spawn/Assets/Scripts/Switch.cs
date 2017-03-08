using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
	Vector2 pressedPos;
	Vector2 OriginalPos;
	public bool pressed = false;
	GameObject player;
	GameObject player2;
	bool firstPressed = false;
	float playerPosition;

	void Start ()
	{
		OriginalPos = transform.position;
		pressedPos = new Vector2 (transform.position.x, transform.position.y - 0.25f);
	}

	void Update ()
	{
		if (player == null) {
			if (player2 != null) {
				player = player2;
				player2 = null;
			} else {
				pressed = false;
				transform.position = OriginalPos;
			}
		}

		if (pressed == false) {
			transform.position = OriginalPos;
		}
	}


	void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.gameObject.tag == "0" || collision.gameObject.tag == "1" || collision.gameObject.tag == "2" || collision.gameObject.tag == "3" || collision.gameObject.tag == "Enemy") {
			if (firstPressed == false) {
				playerPosition = collision.gameObject.transform.position.y;
				firstPressed = true;
			}
			if (collision.gameObject.transform.position.y == playerPosition) {
				StartCoroutine (wait ());
				if (pressed == false) {
					player = collision.gameObject;
					transform.position = pressedPos;
					pressed = true;
				} else {
					player2 = collision.gameObject;
				}
			}
		}
	}

	void OnTriggerExit2D (Collider2D collision)
	{
		if (collision.gameObject.tag == "0" || collision.gameObject.tag == "1" || collision.gameObject.tag == "2" || collision.gameObject.tag == "3" || collision.gameObject.tag == "Enemy") {
			if (collision.gameObject == player) {
				transform.position = OriginalPos;
				pressed = false;
				player = null;
			} else if (collision.gameObject == player2) {
				player2 = null;
			}
		}
	}

	IEnumerator wait ()
	{
		yield return new WaitForSeconds (0.5f);
		firstPressed = false;
	}
}
