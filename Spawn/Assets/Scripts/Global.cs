using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviourSingleton<Global>
{

	public static Global me;
	public Player2 player;
	public Bullet bullet;
	public int timesCast = 0;

	private void Awake ()
	{
		me = this;
	}
}
