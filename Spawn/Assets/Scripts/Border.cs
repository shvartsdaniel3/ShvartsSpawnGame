using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Border : MonoBehaviour
{

	private Image img;


	// Use this for initialization
	void Start ()
	{
		img = GetComponent<Image> ();
		img.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Global.me.levelWon) {
			img.enabled = true;
		}
	}
}
