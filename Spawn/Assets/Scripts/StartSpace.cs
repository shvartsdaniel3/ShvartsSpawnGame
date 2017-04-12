using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSpace : MonoBehaviour
{

	GameObject player;
	SpriteRenderer sr;
	public Color original;
	public Color collide;

	void OnCollisionEnter2D (Collision2D collision)
	{
		player = collision.gameObject;
		sr = player.GetComponent<SpriteRenderer> ();
		StartCoroutine (Flash ());
	}

	private IEnumerator Flash ()
	{
		sr.material.color = collide;
		yield return new WaitForSeconds (3);
		sr.material.color = original;
		yield return new WaitForSeconds (0.2f);
	}

}
