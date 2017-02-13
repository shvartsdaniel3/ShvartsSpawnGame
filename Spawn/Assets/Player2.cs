using System.Collections;
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
	public Sprite dead;
	public Sprite alive;
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

	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
		jump = KeyCode.Space;
		left = KeyCode.A;
		right = KeyCode.D;
		leftA = KeyCode.LeftArrow;
		rightA = KeyCode.RightArrow;
		originalLoc = transform.position;
		gameObject.layer = 8;
		sr = gameObject.GetComponent <SpriteRenderer> ();
		sr.sprite = alive;
		lc = GameObject.FindObjectOfType<LifeCount> ();
		gameObject.tag = lc.gameLives.ToString ();
		awake = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		rb.gravityScale = gravity;
		if (awake == true) {
			//rb.isKinematic = false;
			if (onFloor) {
				if (Input.GetKeyDown (KeyCode.R)) {
					StartCoroutine (Restart ());
				}
			}
			if (Input.GetKeyDown (KeyCode.Space)) {
				jumpFlag = true;
			}
		} else {
			//rb.isKinematic = true;
		}
	}

	void OnTriggerEnter2D (Collider2D collision)
	{
		onFloor = true;
		floorObjs++;
	}

	void OnTriggerExit2D (Collider2D collision)
	{
		floorObjs--;
		if (floorObjs <= 0) {
			onFloor = false;
		}
	}

	void FixedUpdate ()
	{
		Physics2D.IgnoreLayerCollision (8, 9, true);
		if (awake == true) {
			if (jumpFlag && onFloor) {
				rb.AddForce (Vector2.up * jumpForce, ForceMode2D.Impulse);
			}
			jumpFlag = false;
			if (!Input.GetKey (jump) && rb.velocity.y >= 0) {
				rb.velocity = new Vector2 (rb.velocity.x, Mathf.Max (rb.velocity.y - jumpAggro, 0));
			}
			float goDir = 0;
			if (Input.GetKey (left) || Input.GetKey (leftA)) {
				goDir--;
			}
			if (Input.GetKey (right) || Input.GetKey (rightA)) {
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

	private IEnumerator Restart ()
	{
		awake = false;
		rb.velocity = new Vector2 (0, 0);
		sr.sprite = dead;	
		DestroyCorpses ();
		yield return new WaitForSeconds (respawnTime);
		gameObject.layer = 9;
		Instantiate (clone, originalLoc, Quaternion.identity);
		lc.IncreaseLives ();
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
