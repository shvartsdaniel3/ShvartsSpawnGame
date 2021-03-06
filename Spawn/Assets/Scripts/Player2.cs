﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{

	public float jumpForce;
	public float floorDrag;
	public float airDrag;
	public float floorForce;
	public float airForce;
	public float gravity;
	public float jumpAggro;
	public GameObject clone;
	public float respawnTime;
	public Sprite alive;
	public Sprite d1;
	public Sprite d2;
	public Sprite d3;
	Rigidbody2D rb;
	bool jumpFlag;
	bool onFloor;
	int floorObjs;
	KeyCode jump;
	KeyCode left;
	KeyCode leftA;
	KeyCode right;
	KeyCode rightA;
	LifeCount lc;
	Vector2 originalLoc;
	public bool awake = true;
	IEnumerator coroutine;
	SpriteRenderer sr;
	bool spawning = false;
	int curLives;
	GameObject child;
	Animator anm;
	bool inAnim;
	bool dead;
	public AudioClip glitch;
	public AudioClip jumpSound;
	ParticleSystem[] particle;
	public ParticleSystem explosion;
	ParticleSystem jumpPart;
	Screenshake shake;


	void Start ()
	{
		child = gameObject.transform.GetChild (0).gameObject;
		child.SetActive (false);
		Global.me.player = this;
		rb = GetComponent<Rigidbody2D> ();
		jump = KeyCode.Space;
		left = KeyCode.A;
		right = KeyCode.D;
		leftA = KeyCode.LeftArrow;
		rightA = KeyCode.RightArrow;
		originalLoc = transform.position;
		gameObject.layer = 8;
		sr = gameObject.GetComponent <SpriteRenderer> ();
		anm = gameObject.GetComponent <Animator> ();
		sr.sprite = alive;
		lc = GameObject.FindObjectOfType<LifeCount> ();
		gameObject.tag = 
		lc.gameLives.ToString ();
		awake = true;
		curLives = lc.numLife;
		anm.enabled = true;
		sr.sortingOrder = 1;
		particle = gameObject.GetComponentsInChildren<ParticleSystem> ();
		foreach (ParticleSystem i in particle) {
			if (i.tag.ToString () == "Explosion") {
				explosion = i;
			} else if (i.tag.ToString () == "jump") {
				jumpPart = i;
			}
		}
		shake = GameObject.FindObjectOfType<Screenshake> ();
	}

	void Update ()
	{

		if (Global.me.explode && !awake) {
			explosion.Play ();
			explosion.transform.parent = null;
			Global.me.explode = false;
		}
		if (awake & !inAnim) {
			StartCoroutine (Blink ());
		}
		rb.gravityScale = gravity;
		if (awake == true) {
			if (onFloor) {
				if (Input.GetKeyDown (KeyCode.R)) {
					Global.me.restart = true;
					Global.me.sound.PlaySound (glitch, 0.6f, false);
					StartCoroutine (Restart ());
				}
			}
			if (Input.GetKeyDown (KeyCode.Space)) {
				jumpFlag = true;
			}
		}
	
		if (lc.numLife == curLives + 1) {
			curLives = lc.numLife;
			if (!awake && !dead) {
				anm.enabled = false;
				sr.sprite = d1;
				dead = true;
				sr.sortingOrder = 0;
			} else if (sr.sprite == d1) {
				sr.sprite = d2;
				anm.SetBool ("Dead2", true);
			} else if (sr.sprite == d2) {
				sr.sprite = d3;
				anm.SetBool ("Dead3", true);
			}
		}
	}


	void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.gameObject.tag == "floor") {
			onFloor = true;
			floorObjs++;
		}
	}

	void OnTriggerExit2D (Collider2D collision)
	{
		if (collision.gameObject.tag == "floor") {
			floorObjs--;
			if (floorObjs <= 0) {
				onFloor = false;
			}
		}
	}

	void FixedUpdate ()
	{

		Physics2D.IgnoreLayerCollision (8, 13, true);
		if (awake == true) {
			if (jumpFlag && onFloor) {
				jumpPart.Play ();
				Global.me.sound.PlaySound (jumpSound, 0.5f, false);
				rb.AddForce (Vector2.up * jumpForce, ForceMode2D.Impulse);
			}
			jumpFlag = false;
			if (!Input.GetKey (jump) && rb.velocity.y >= 0) {
				rb.velocity = new Vector2 (rb.velocity.x, Mathf.Max (rb.velocity.y - jumpAggro, 0));
			}
			float goDir = 0;
			if (Input.GetKey (left) || Input.GetKey (leftA)) {
				sr.flipX = true;
				goDir--;
			}
			if (Input.GetKey (right) || Input.GetKey (rightA)) {
				sr.flipX = false;
				goDir++;
			}
			if (onFloor) {
				rb.AddForce (Vector2.right * floorForce * goDir);
				rb.AddForce (Vector2.left * floorDrag * Mathf.Sign (rb.velocity.x) * Mathf.Pow (rb.velocity.x, 2));
			} else {
				rb.AddForce (Vector2.right * airForce * goDir);
				rb.AddForce (Vector2.left * airDrag * Mathf.Sign (rb.velocity.x) * Mathf.Pow (rb.velocity.x, 2));
			}
		}
	}

	private IEnumerator Blink ()
	{
		inAnim = true;
		anm.SetBool ("Blink", false);
		float timeNumber = Random.Range (0, 10);
		yield return new WaitForSeconds (timeNumber);
		anm.SetBool ("Blink", true);
		inAnim = false;
	}

	private IEnumerator Restart ()
	{
		if (spawning == false) {
			spawning = true;
			awake = false;
			rb.velocity = new Vector2 (0, 0);
			gameObject.layer = 13;
			DestroyCorpses ();
			lc.IncreaseLives ();
			yield return new WaitForSeconds (respawnTime);
			child.SetActive (true);
			Instantiate (clone, originalLoc, Quaternion.identity);
			Global.me.timesCast = 0;
			spawning = false;
		}
	}

	public void RestartFromOutside ()
	{
		StartCoroutine (Restart ());
	}

	void DestroyCorpses ()
	{
		if (gameObject.tag == "3") {
			Destroy (GameObject.FindGameObjectWithTag ("0"));
		} else if (gameObject.tag == "0") {
			Destroy (GameObject.FindGameObjectWithTag ("1"));
		} else if (gameObject.tag == "1") {
			Destroy (GameObject.FindGameObjectWithTag ("2"));
		} else if (gameObject.tag == "2") {
			Destroy (GameObject.FindGameObjectWithTag ("3"));
		}
	}
}