using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

	Rigidbody2D rb;
	bool idle;
	public float speed;
	public float waitTime;
	public float floorDrag;
	float originalDistance;
	bool moving = false;
	SpriteRenderer sr;
	public Sprite alert;
	public Sprite idles;
	public Transform sightStart, sightEnd;
	bool movingToCorpse;
	GameObject corpse;
	GameObject player;
	public float floorForce;
	public GameObject bullet;
	bool waitingForCorpse = false;
	bool hitting = false;
	Animator anm;
	bool inAnim;

	void Start ()
	{
		idle = true;
		rb = GetComponent<Rigidbody2D> ();
		sr = GetComponent<SpriteRenderer> ();
		sr.flipX = true;
		anm = gameObject.GetComponent <Animator> ();
	}

	void RayCasting ()
	{
		Debug.DrawRay (sightStart.position, (sightEnd.position - sightStart.position).normalized, Color.green);
		RaycastHit2D ray = Physics2D.Raycast (sightStart.position, (sightEnd.position - sightStart.position).normalized, 999, LayerMask.GetMask ("Player", "Dead Players", "Wall", "Star Zone"));
		if (ray && ray.collider.gameObject.layer == 8) {
			player = ray.collider.gameObject;
			KillPlayer ();
		}
		//ray = Physics2D.Linecast (sightStart.position, sightEnd.position, 1 << LayerMask.NameToLayer ("Dead Players"));
		if (ray && ray.collider.gameObject.layer == 13) {
			if (corpse != ray.collider.gameObject || hitting == false) {
				hitting = true;
				movingToCorpse = true;
				corpse = ray.collider.gameObject;
			}
		} else {
			if (hitting) {
				hitting = false;
				movingToCorpse = false;
				idle = true;
				sr.sprite = idles;
			}
		}


	}

	private IEnumerator Blink ()
	{
		inAnim = true;
		//anm.SetBool ("Blink", false);
		float timeNumber = Random.Range (0, 10);
		yield return new WaitForSeconds (timeNumber);
		anm.SetBool ("Blink", true);
		inAnim = false;
	}

	void KillPlayer ()
	{
		//StartCoroutine ("Wait");
		anm.SetBool ("Transition", false);
		anm.SetBool ("Alert", true);
		if (Global.me.timesCast == 0) {
			Global.me.timesCast += 1;
			Global.me.player.RestartFromOutside ();
		}
	}

	void Update ()
	{
		if (!movingToCorpse & !inAnim) {
			StartCoroutine (Blink ());
		}
	}

	void FixedUpdate ()
	{
		RayCasting ();

		if (idle) {
			if (moving == false) {
				StartCoroutine ("Patrol");
			}
		}

		if (movingToCorpse) {
			StopCoroutine ("Patrol");
			moving = false;
			//anm.enabled = false;
			anm.SetBool ("Alert", true);
			anm.SetBool ("Transition", false);
			Vector2 dir = transform.position - corpse.transform.position;
			idle = false;
			if (Mathf.Abs (dir.x) < 1.3 && waitingForCorpse == false) {
				rb.velocity = new Vector2 (0, 0);
				StartCoroutine ("WaitForCorpse");
			} else {
				rb.velocity = (-dir.normalized * 3f);
				//rb.AddForce (-dir.normalized * floorForce * 0.05f);
			}
		}
	}


	IEnumerator Patrol ()
	{
		moving = true;
		yield return new WaitForSeconds (waitTime);
		sightEnd.transform.localPosition = new Vector2 (20, 0);
		sr.flipX = true;
		MoveEnemy (speed, Vector2.left);
		yield return new WaitForSeconds (waitTime);
		sr.flipX = false;
		sightEnd.transform.localPosition = new Vector2 (-20, 0);
		MoveEnemy (-speed, Vector2.right);
		moving = false;
	}


	IEnumerator WaitForCorpse ()
	{
		movingToCorpse = false;
		waitingForCorpse = true;
		yield return new WaitForSeconds (waitTime - 3);
		Destroy (corpse);
		idle = true;
		anm.SetBool ("Transition", true);
		anm.SetBool ("Alert", false);
		//sr.sprite = idles;
		waitingForCorpse = false;
	}

	void MoveEnemy (float x, Vector2 pos)
	{
		rb.AddForce (Vector2.right * floorForce * x);
		rb.AddForce (Vector2.left * floorDrag * Mathf.Sign (rb.velocity.x) * Mathf.Pow (rb.velocity.x, 2));
	}
}