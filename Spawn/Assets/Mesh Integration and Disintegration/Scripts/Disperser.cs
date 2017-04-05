using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Please note that "Disperser.cs" have to be on a GameObject, that is parent of all the rest Disperser's scripts, and also parent of the Mesh Renderer (One or more) that you want to manipulate.
/// So, it's easy. Just put this script on the top of the hiearchy parent, as the example shows.
/// This script is a Disperser Manager and it serves as an interface for manipulate the "Explosion" and "Implosion" effect. So, if you don't want to touch anything from this package and just want to use it,
/// then use this script as an interface. You just have to call the "StartActivate" and "StartDeactivate" methods with certain parameters in order to produce the explosion or the implossion. 
/// </summary>

public class Disperser : MonoBehaviour 
{
	private DisperserAnimatorManager disperserAnimatorManager;

	void Awake()
	{
		if (!DisperserError.error) 
		{
			disperserAnimatorManager = GetComponent<DisperserAnimatorManager> ();

			if (disperserAnimatorManager == null) 
			{
				disperserAnimatorManager = gameObject.AddComponent<DisperserAnimatorManager> ();
			}
		}
	}

	void Start()
	{
		disperserAnimatorManager.OnDisperserActivatedMesh += OnDisperserActivatedMesh;
		disperserAnimatorManager.OnDisperserDeactivatedMesh += OnDisperserDeactivatedMesh;
	}

	void Destroy()
	{
		disperserAnimatorManager.OnDisperserActivatedMesh += OnDisperserActivatedMesh;
		disperserAnimatorManager.OnDisperserDeactivatedMesh += OnDisperserDeactivatedMesh;
	}


	/// <summary>
	/// You can use this to catch when a mesh has been deactivated.
	/// </summary>
	void OnDisperserDeactivatedMesh (Renderer renderer)
	{

	}

	/// <summary>
	/// You can use this to catch when a mesh has been deactivated.
	/// </summary>
	void OnDisperserActivatedMesh (Renderer renderer)
	{

	}

	public DisperserState State
	{
		get { return disperserAnimatorManager.State; }
	}

	/// <summary>
	/// Starts the mesh activation. Thousands of tiny submeshes will join together to form the right mesh. The final position of the submeshes is the right mesh that you are using in the right position it is.
	/// </summary>
	/// <returns>Returns true if could make the activation. </returns>
	/// <param name="speed">Velocity of the activation.</param>
	/// <param name="distance">This determines a constant distance from the final position of the submesh, that the submeshes begin to move.</param>
	/// <param name="random">A random value for the start position of the submeshes in a Sphere.</param>
	/// <param name="factorChaos">If the value is lower, the submeshes will start in accordance with the "random" property above. If not, the submeshes will start in chaotical positions (Not inside a Sphere) </param>
	public bool StartActivate(float speed,Vector3 distance,float random,float factorChaos,bool threeHundredSixtyDegreesEffect)
	{
		return disperserAnimatorManager.StartActivate (speed, distance, random,factorChaos,threeHundredSixtyDegreesEffect);
	}

	/// <summary>
	/// Starts the mesh deactivation. The mesh will subdivide in thousands of tiny submeshes, that will travel trough space, and disappear. The final position of the submeshes is a position in some part of the scene.
	/// </summary>
	/// <returns>Returns true if could make the deactivation. </returns>
	/// <param name="speed">Velocity of the deactivation</param>
	/// <param name="distance">This determines a constant distance from the final position of the submesh, that submeshes begin to move.</param>
	/// <param name="random">A random value for the end position of the submeshes in a Sphere.</param>
	/// <param name="factorChaos">If the value is lower, the submeshes will start in accordance with the "random" property above. If not, the submeshes will start in chaotical positions (Not inside a Sphere)</param>
	public bool StartDeactivate(float speed,Vector3 distance,float random,float factorChaos,bool fadeOut,bool threeHundredSixtyDegreesEffect)
	{
		return disperserAnimatorManager.StartDeactivate (speed, distance, random,factorChaos,fadeOut,threeHundredSixtyDegreesEffect);
	}
}
