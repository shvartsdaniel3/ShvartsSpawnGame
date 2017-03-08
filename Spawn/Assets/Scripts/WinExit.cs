using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinExit : MonoBehaviour
{

	void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.gameObject.tag == "0" || collision.gameObject.tag == "1" || collision.gameObject.tag == "2" || collision.gameObject.tag == "3") {
			Time.timeScale = 0;
			Global.me.levelWon = true;
		}
	}
}
