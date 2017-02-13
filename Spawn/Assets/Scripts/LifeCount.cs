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
		lives.text = "0/3";
		enemy = 5;
	}

	public void IncreaseLives ()
	{
		numLife += 1;
		if (numLife <= 3) {
			lives.text = numLife.ToString () + "/3";
		}
		if (gameLives <= 2) {
			gameLives += 1;
		} else {
			gameLives = 0;
		}
	}

}
