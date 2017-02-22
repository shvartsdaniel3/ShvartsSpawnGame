using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinText : MonoBehaviour
{

	Text text;
	public bool winState = false;
	LifeCount lc;

	// Use this for initialization
	void Start ()
	{
		lc = GameObject.FindObjectOfType<LifeCount> ();
		text = GetComponent<Text> ();
		text.text = "";
		text.text = text.text.Replace ("NewLine", "/n");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (winState) {
			text.text = "You Win!";
		}
	}
}
