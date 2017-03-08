using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rating : MonoBehaviour
{

	Text text;

	// Use this for initialization
	void Start ()
	{
		text = GetComponent<Text> ();
		text.text = "";
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Global.me.levelWon) {
			text.text = "R A T I N G :  ";
		}
	}
}
