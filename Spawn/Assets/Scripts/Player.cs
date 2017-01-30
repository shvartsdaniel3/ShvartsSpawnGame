using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	public float speed;
	private Rigidbody2D rb;
	public GameObject clone;
	private Vector2 originalLoc;
	private bool awake = true;
	public float respawnTime;
	private IEnumerator coroutine;
	public Sprite dead;
	SpriteRenderer sr;

	void Start ()
	{
		rb = GetComponent <Rigidbody2D> ();
		originalLoc = transform.position;
		gameObject.layer = 8;
		Physics2D.IgnoreLayerCollision (8, 9, true);
		sr = gameObject.GetComponent <SpriteRenderer> ();
	}

	void Update ()
	{
		if (awake == true) {
			float moveHorizontal = Input.GetAxis ("Horizontal");
			Vector2 movement = new Vector2 (moveHorizontal, 0);
			rb.velocity = movement * speed;
			if (Input.GetKeyDown (KeyCode.R)) {
				coroutine = Restart ();
				StartCoroutine (coroutine);
			}
		}
	}

	private IEnumerator Restart ()
	{
		awake = false;
		rb.velocity = new Vector2 (0, 0);
		sr.sprite = dead;
		yield return new WaitForSeconds (respawnTime);
		gameObject.layer = 9;
		Instantiate (clone, originalLoc, Quaternion.identity);
	}
}