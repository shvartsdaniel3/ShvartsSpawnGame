using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
	Player2 player2;
	public bool playerLives = false;
	public bool playerDies = false;
	LifeCount lc;
	SpriteRenderer sr;
	Sprite dead;

	void Start ()
	{
		lc = GameObject.FindObjectOfType<LifeCount> ();
		lc.enemy = lc.enemy + 1;
		gameObject.tag = lc.enemy.ToString ();
		print (gameObject.tag.ToString ());
	}

	void OnTriggerEnter2D (Collider2D collision)
	{	
		if (collision.gameObject.tag == "0" || collision.gameObject.tag == "1" || collision.gameObject.tag == "2" || collision.gameObject.tag == "3") {
			player2 = collision.gameObject.GetComponent<Player2> ();
			sr = player2.GetComponent<SpriteRenderer> ();
			if (player2.awake == true) {
				player2.RestartFromOutside ();
				playerLives = true;
			} else if (player2.awake == false) {
				playerDies = true;
			} else {
				print ("player is neither dead nor alive???");
			}
		} //else {
		//playerLives = false;
		//playerDies = false;
		//}
	}


}