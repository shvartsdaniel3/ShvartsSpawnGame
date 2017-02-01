using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeCount : MonoBehaviour
{

	public int numLife = 0;
	Text lives;

	void Start ()
	{
		lives = GetComponent<Text> ();
		lives.text = "0/3";
	}

	public void IncreaseLives ()
	{
		numLife += 1;
		lives.text = numLife.ToString () + "/3";
	}

}
