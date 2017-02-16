using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

	Rigidbody2D rb;
	bool idle;
	public float speed;
	public float waitTime;
	float startPosition;
	public float endPosition;
	float distance;
	public float floorDrag;
	float originalDistance;
	bool moving = false;
	SpriteRenderer sr;
	public Sprite alert;
	public Sprite idles;
	public Transform sightStart, sightEnd;
	Transform originalEnd;
	public bool spottedAlive = false;
	public bool spottedDead = false;
	//LineRenderer lr;
	GameObject player;
	GameObject altPlayer;
	bool firstPlayer = false;
	float playerDistance;
	Player2 p2;
	bool movingToCorpse;
	GameObject corpse;
	public float floorForce;

	// Use this for initialization
	void Start ()
	{
		originalEnd = sightEnd;
		//lr = GetComponent<LineRenderer> ();
		//gameObject.layer = 8;
		idle = true;
		rb = GetComponent<Rigidbody2D> ();
		startPosition = transform.position.x;
		distance = endPosition - startPosition;
		originalDistance = distance;
		sr = GetComponent<SpriteRenderer> ();
	}

	void Update ()
	{
	}

	void RayCasting ()
	{
		Debug.DrawRay (sightStart.position, (sightEnd.position - sightStart.position).normalized, Color.green);
		RaycastHit2D ray = Physics2D.Raycast (sightStart.position, (sightEnd.position - sightStart.position).normalized, 999, LayerMask.GetMask ("Player", "Dead Players"));
		if (ray && ray.collider.gameObject.layer == 8) {
			KillPlayer ();
		}
		ray = Physics2D.Linecast (sightStart.position, sightEnd.position, 1 << LayerMask.NameToLayer ("Dead Players"));
		if (ray && ray.collider.gameObject.layer == 13) {
			movingToCorpse = true;
			corpse = ray.collider.gameObject;
		}
	}

	void KillPlayer ()
	{
		StartCoroutine ("Wait");
		if (Global.me.timesCast == 0) {
			Global.me.timesCast += 1;
			Global.me.player.RestartFromOutside ();
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
			StartCoroutine ("Wait");
			Vector3 dir = corpse.transform.position - transform.position;
			if (transform.position.x * (-dir.normalized.x) <= ((corpse.transform.position.x + 1.2f) * (-dir.normalized.x))) {
				rb.velocity = new Vector2 (0, 0);
				StartCoroutine ("WaitForCorpse");
				idle = true;
				movingToCorpse = false;
			} else {
				idle = false;
				rb.AddForce (dir.normalized * floorForce * 0.05f);
				//rb.AddForce (-dir.normalized * floorDrag * Mathf.Sign (rb.velocity.x) * Mathf.Pow (rb.velocity.x, 2));
			}
		}
	}


	IEnumerator Patrol ()
	{
		moving = true;
		if (distance >= 0) {
			sightEnd.transform.localPosition = new Vector2 (20, 0);
			MoveEnemy (speed, Vector2.left);
			distance = endPosition - transform.position.x;
			yield return new WaitForSeconds (waitTime);
			sightEnd.transform.localPosition = new Vector2 (-20, 0);
			MoveEnemy (-speed, Vector2.right);
			distance = endPosition - transform.position.x;
			yield return new WaitForSeconds (waitTime);
			sightEnd.transform.localPosition = new Vector2 (20, 0);
			distance = originalDistance;
		}
		moving = false;
		yield return new WaitForSeconds (waitTime);
	}

	IEnumerator Wait ()
	{
		sr.sprite = alert;
		yield return new WaitForSeconds (waitTime - 2);
		sr.sprite = idles;
	}

	IEnumerator WaitForCorpse ()
	{
		yield return new WaitForSeconds (waitTime - 3);
		Destroy (corpse);
	}

	void MoveEnemy (float x, Vector2 pos)
	{
		rb.AddForce (Vector2.right * floorForce * x);
		rb.AddForce (Vector2.left * floorDrag * Mathf.Sign (rb.velocity.x) * Mathf.Pow (rb.velocity.x, 2));
		//rb.velocity = new Vector2 (x, rb.velocity.y);
	}

}
