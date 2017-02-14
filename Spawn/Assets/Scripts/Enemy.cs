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
	public bool spottedAlive = false;
	public bool spottedDead = false;
	LineRenderer lr;

	// Use this for initialization
	void Start ()
	{
		lr = GetComponent<LineRenderer> ();
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
		if (spottedAlive) {
			idle = false;
			StartCoroutine ("Wait");
		} else if (spottedDead) {
			idle = false;
		}
	}

	void RayCasting ()
	{
		Debug.DrawLine (sightStart.position, sightEnd.position, Color.green);
		lr.SetPosition (0, sightStart.position);
		lr.SetPosition (1, sightEnd.position);
		spottedAlive = Physics2D.Linecast (sightStart.position, sightEnd.position, 1 << LayerMask.NameToLayer ("Player"));
		spottedDead = Physics2D.Linecast (sightStart.position, sightEnd.position, 1 << LayerMask.NameToLayer ("Dead Players"));
	}

	void FixedUpdate ()
	{
		RayCasting ();
		if (idle) {
			if (moving == false) {
				StartCoroutine ("Patrol");
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
		yield return new WaitForSeconds (waitTime);
		sr.sprite = idles;
	}

	void MoveEnemy (float x, Vector2 pos)
	{
		rb.velocity = new Vector2 (x, rb.velocity.y);
	}

}
