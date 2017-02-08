using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	private bool jumping = false;
	private Rigidbody2D rb;
	private Vector2 originalLoc;
	private bool awake = true;
	private IEnumerator coroutine;
	private SpriteRenderer sr;
	private bool falling = true;
	private float dSpeed;
	private LifeCount lc;
	private float currentJumpVel;
	public float speed;
	public GameObject clone;
	public float respawnTime;
	public Sprite dead;
	public Sprite alive;
	public float dropSpeed;
	public float gravity;
	public float jumpSpeed;
	public float jumpSpeed2;
	private Vector2 movement;

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
		if (collision.collider.gameObject.tag.Equals ("floor") == true && collision.collider.gameObject.tag != ("Side")) {
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
			if (jumping) {
				DealWithJumping ();
			}
		} else {
			rb.isKinematic = true;
		}
	}

	void DealWithJumping ()
	{
		//rb.AddForce (transform.up * jumpSpeed2, ForceMode2D.Impulse);
		currentJumpVel -= gravity;
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		if (collision.collider.gameObject.tag.Equals ("floor") == true) {
			jumping = false;
		}

	}

	void FixedUpdate ()
	{
		Physics2D.IgnoreLayerCollision (8, 9, true);
		if (awake == true) {
			float moveHorizontal = Input.GetAxis ("Horizontal");
			if (falling) {
				dSpeed = dropSpeed;
				currentJumpVel = dropSpeed;
			} else {
				dSpeed = 0;
				if (Input.GetKeyDown (KeyCode.Space)) {
					jumping = true;
					currentJumpVel = jumpSpeed;
				} else {
					currentJumpVel = dropSpeed;
				}
			}
			movement = new Vector2 (moveHorizontal * speed, z);
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