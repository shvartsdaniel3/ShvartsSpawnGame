using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
	Player2 player2;
	public bool playerLives = false;
	public bool playerDies = false;
	LifeCount lc;
	public float subamount;
	SpriteRenderer sr;
	bool shrinking;

	void Start ()
	{
		lc = GameObject.FindObjectOfType<LifeCount> ();
//		lc.enemy = lc.enemy + 1;
//		gameObject.tag = lc.enemy.ToString ();
		sr = GetComponent<SpriteRenderer> ();
		shrinking = false;
	}

	void OnTriggerStay2D (Collider2D other)
	{
		if (other.gameObject.tag == "0" || other.gameObject.tag == "1" || other.gameObject.tag == "2" || other.gameObject.tag == "3") {
			shrinking = true;
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		if (col.gameObject.tag == "0" || col.gameObject.tag == "1" || col.gameObject.tag == "2" || col.gameObject.tag == "3") {
			shrinking = false;
		}
	}

	void Update ()
	{
		if (shrinking = true) {
			if (transform.localScale.x >= 0) {
				transform.localScale = new Vector2 (transform.localScale.x - subamount, 1);
			}
		} //else if (shrinking = false) {
		//if (transform.localScale.x <= 1) {
		//	transform.localScale = new Vector2 (transform.localScale.x + subamount, 1);
		//}
		//}
	}

	void OnTriggerEnter2D (Collider2D collision)
	{	
		if (collision.gameObject.tag == "0" || collision.gameObject.tag == "1" || collision.gameObject.tag == "2" || collision.gameObject.tag == "3") {
			player2 = collision.gameObject.GetComponent<Player2> ();
			if (player2.awake == true) {
				player2.RestartFromOutside ();
				playerLives = true;
			} else if (player2.awake == false) {
				playerDies = true;
			} else {
				print ("player is neither dead nor alive???");
			}
		}

	}


}