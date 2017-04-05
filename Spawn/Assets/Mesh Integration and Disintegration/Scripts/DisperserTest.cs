using UnityEngine;

/// <summary>
/// This is just a simple test to show you the package effect. This class uses the interface of the system that i have created. But i strongly encourage you to replace this simple class test, in whatever you want,
/// using the classes that i have made. 
/// 
/// Basically it activates and deactivates the mesh, every "x" seconds.
/// </summary>

public class DisperserTest : MonoBehaviour 
{
	public float speed = 0.25f;
	public Vector3 distance = new Vector3(0f,2f,0f);
	public float random = 10f;
	public float lapseBetweenAnimations = 1f;
	public float factorChaos = 1f;
	public bool fadeOut = true;
	public bool threeHundredSixtyDegreesEffect = false;

	public Disperser disperser;

	private float time;

	void Start()
	{
		if( ! DisperserError.error)
		{
			time = 0f;

			if (disperser == null) 
			{
				disperser = GetComponent<Disperser> ();

				if (disperser == null) 
				{
					disperser = gameObject.AddComponent<Disperser> ();
				}
			}
		}
	}

	void Update()	
	{
		if( ! DisperserError.error)
		{
			if (disperser.State == DisperserState.deactivated) 
			{
				time += Time.deltaTime;

				if(time >= lapseBetweenAnimations)
				{
					disperser.StartActivate (speed, distance, random,factorChaos,threeHundredSixtyDegreesEffect);
					time = 0f;
				}
			}

			if (disperser.State == DisperserState.activated) 
			{
				time += Time.deltaTime;
				
				if(time >= lapseBetweenAnimations)
				{
					disperser.StartDeactivate (speed, distance, random,factorChaos,fadeOut,threeHundredSixtyDegreesEffect);
					time = 0f;
				}
			}
		}
	}
}
