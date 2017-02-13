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
	LineOfSight sight;
	LineOfSight[] enemyColliders;
	LifeCount lc;
	SpriteRenderer sr;
	public Sprite alert;
	public Sprite idles;

	// Use this for initialization
	void Start ()
	{
		//gameObject.layer = 8;
		idle = true;
		lc = GameObject.FindObjectOfType<LifeCount> ();
		rb = GetComponent<Rigidbody2D> ();
		startPosition = transform.position.x;
		distance = endPosition - startPosition;
		originalDistance = distance;
		enemyColliders = GameObject.FindObjectsOfType<LineOfSight> ();
		foreach (LineOfSight i in enemyColliders) {
			if (i.tag == lc.enemy.ToString ()) {
				sight = i;
			}
		}
		sr = GetComponent<SpriteRenderer> ();
	}

	void Update ()
	{
		if (sight.playerLives) {
			//idle = false;
			StartCoroutine ("Wait");
			sight.playerLives = false;
			//StopCoroutine ("Patrol");
		} else if (sight.playerDies) {
			print ("dead");
		}
	}

	void FixedUpdate ()
	{
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
			MoveEnemy (speed, Vector2.left);
			distance = endPosition - transform.position.x;
			yield return new WaitForSeconds (waitTime);
			sight.transform.position = new Vector2 (sight.transform.position.x - 5, sight.transform.position.y);
			MoveEnemy (-speed, Vector2.right);
			distance = endPosition - transform.position.x;
			yield return new WaitForSeconds (waitTime);
			sight.transform.position = new Vector2 (sight.transform.position.x + 5, sight.transform.position.y);
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
		//rb.AddForce (pos * floorDrag * Mathf.Sign (rb.velocity.x) * Mathf.Pow (rb.velocity.x, 2));
	}

}
