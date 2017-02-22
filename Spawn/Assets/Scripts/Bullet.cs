using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	float direction;
	public float speed;


	void Start ()
	{
		Global.me.bullet = this;
	}

	void Update ()
	{
		
		transform.position = new Vector2 (transform.position.x - speed, transform.position.y);
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		print ("is this happening");
		if (col.gameObject.layer == 8) {
			print ("this is happening!");
			Global.me.player.RestartFromOutside ();
		}
	}

	public void GiveDirection (float x)
	{
		direction = x;
	}

}