using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviourSingleton<Global>
{

	public static Global me;
	public GameObject player;

	// Use this for initialization

	private void Awake ()
	{
		me = this;
	}

	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
