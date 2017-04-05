using UnityEngine;

/// <summary>
/*
	Just exits the application if the user press the "Escape" key. You can erase it if you want.
 */ 
/// </summary>
public class DisperserTestExit : MonoBehaviour 
{
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			Application.Quit ();
		}
	}
}
