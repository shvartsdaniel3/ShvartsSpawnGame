using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grade : MonoBehaviour
{

	public int minLives;
	Text text;
	LifeCount lc;

	// Use this for initialization
	void Start ()
	{
		lc = GameObject.FindObjectOfType<LifeCount> ();
		text = GetComponent<Text> ();
		text.text = "";
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Global.me.levelWon) {
			if (lc.numLife <= minLives) {
				text.text = "A+";
			} else if (lc.numLife > minLives && lc.numLife <= 1.1 * minLives) {
				text.text = "A";
			} else if (lc.numLife > 1.1 * minLives && lc.numLife <= 1.2 * minLives) {
				text.text = "A-";
			} else if (lc.numLife > 1.2 * minLives && lc.numLife <= 1.3 * minLives) {
				text.text = "B+";
			} else if (lc.numLife > 1.3 * minLives && lc.numLife <= 1.4 * minLives) {
				text.text = "B";
			} else if (lc.numLife > 1.4 * minLives && lc.numLife <= 1.5 * minLives) {
				text.text = "B-";
			} else if (lc.numLife > 1.5 * minLives && lc.numLife <= 1.6 * minLives) {
				text.text = "C+";
			} else if (lc.numLife > 1.6 * minLives && lc.numLife <= 1.7 * minLives) {
				text.text = "C";
			} else if (lc.numLife > 1.7 * minLives && lc.numLife <= 1.8 * minLives) {
				text.text = "C-";
			} else if (lc.numLife > 1.8 * minLives && lc.numLife <= 1.9 * minLives) {
				text.text = "D+";
			} else if (lc.numLife > 1.9 * minLives && lc.numLife <= 2.0 * minLives) {
				text.text = "D";
			} else if (lc.numLife > 2.0 * minLives && lc.numLife <= 2.1 * minLives) {
				text.text = "D-";
			} else if (lc.numLife > 2.1 * minLives) {
				text.text = "F";
			} 
		}
	}
}
