using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	private Rigidbody2D rb;
	private Vector2 originalLoc;
	private bool awake = true;
	private IEnumerator coroutine;
	private SpriteRenderer sr;
	public float speed;
	public GameObject clone;
	public float respawnTime;
	public Sprite dead;
	public Sprite alive;
	public float dropSpeed;
	private bool falling = true;
	private float dSpeed;
	private LifeCount lc;

	void Start ()
	{
		rb = GetComponent <Rigidbody2D> ();
		originalLoc = transform.position;
		gameObject.layer = 8;
		sr = gameObject.GetComponent <SpriteRenderer> ();
		sr.sprite = alive;
		lc = GameObject.FindObjectOfType<LifeCount> ();
	}

	void OnCollisionStay2D (Collision2D collision)
	{
		if (collision.collider.gameObject.tag.Equals ("floor") == true) {
			falling = false;
		}
	}

	void OnCollisionExit2D (Collision2D collision)
	{
		if (collision.collider.gameObject.tag.Equals ("floor") == true) {
			falling = true;
		}
	}

	void Update ()
	{
		if (awake == true) {
			rb.isKinematic = false;
			if (Input.GetKeyDown (KeyCode.R) && falling == false) {
				StartCoroutine (Restart ());
			}
		} else {
			rb.isKinematic = true;
		}
	}

	void FixedUpdate ()
	{
		Physics2D.IgnoreLayerCollision (8, 9, true);
		if (awake == true) {
			float moveHorizontal = Input.GetAxis ("Horizontal");
			if (falling) {
				dSpeed = dropSpeed;
			} else {
				dSpeed = 0;
			}
			Vector2 movement = new Vector2 (moveHorizontal * speed, dSpeed);
			rb.velocity = movement;
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
		lc.IncreaseLives ();
	}
}