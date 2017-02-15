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
		Global.me.player.SendMessage ("Restart", null);
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
			
		} else {
			sr.sprite = idles;
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
		yield return new WaitForSeconds (waitTime);
		sr.sprite = idles;
	}

	void MoveEnemy (float x, Vector2 pos)
	{
		rb.velocity = new Vector2 (x, rb.velocity.y);
	}

}
