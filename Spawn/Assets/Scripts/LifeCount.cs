using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeCount : MonoBehaviour
{

	public int numLife = 0;
	Text lives;
	public int gameLives = 0;
	public int enemy;

	void Start ()
	{
		lives = GetComponent<Text> ();
		lives.text = "Lives Used: 0";
		enemy = 5;
	}

	public void IncreaseLives ()
	{
		numLife += 1;
		lives.text = "Lives Used: " + numLife.ToString ();
		if (gameLives <= 2) {
			gameLives += 1;
		} else {
			gameLives = 0;
		}
	}

}
