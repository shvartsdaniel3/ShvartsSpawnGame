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
	bool sightDirection;

	// Use this for initialization
	void Start ()
	{
		sightDirection = true;
		//true is right, false is left
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
	}

	void Update ()
	{
		if (sight.playerLives) {
			sight.playerLives = false;
			print ("detecting life");
			//StopCoroutine ("Patrol");
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

	void MoveEnemy (float x, Vector2 pos)
	{
		rb.velocity = new Vector2 (x, rb.velocity.y);
		//rb.AddForce (pos * floorDrag * Mathf.Sign (rb.velocity.x) * Mathf.Pow (rb.velocity.x, 2));
	}

}
