using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Global : MonoBehaviourSingleton<Global>
{

	public static Global me;
	public Player2 player;
	public Bullet bullet;
	public int timesCast = 0;
	public bool levelWon = false;
	public Button rButton;
	public Button nButton;

	void Start ()
	{
		Time.timeScale = 1;
		rButton.onClick.AddListener (restartButton);
		nButton.onClick.AddListener (NextButton);
	}

	private void Awake ()
	{
		me = this;
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Q)) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}
	}

	void restartButton ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	void NextButton ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
	}
}
