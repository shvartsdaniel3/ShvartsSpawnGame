using UnityEngine;
using System.Collections.Generic;

public class DisperserAnimatorManager : MonoBehaviour 
{
	public float speed = 0.25f;
	public Vector3 distance = new Vector3(0f,2f,0f);
	public float random = 10f;
	public float lapseBetweenAnimations = 1f;
	public float factorChaos = 1f;
	public bool fadeOut = true;
	public int trianglesPerSubMesh = 1;
	public bool threeHundredSixtyDegreesEffect = false;

	public delegate void DisperserActivatedDelegate2(Renderer renderer);
	public event DisperserActivatedDelegate2 OnDisperserActivatedMesh;

	public delegate void DisperserDeactivatedDelegate2(Renderer renderer);
	public event DisperserDeactivatedDelegate2 OnDisperserDeactivatedMesh;

	private List<DisperserAnimator> disperserAnimators;
	private List<DisperserRendererManager> disperserRenderersManager;

	void Awake()
	{
		if( ! DisperserError.error)
		{
			disperserAnimators = new List<DisperserAnimator>(GetComponentsInChildren<DisperserAnimator> ());

			if (disperserAnimators == null) 
			{
				DisperserError.error = true;
				Debug.LogWarning ("The GameObject doesn't have childs with DisperserAnimators");
				Destroy (this);
				return;
			}

			disperserRenderersManager = new List<DisperserRendererManager>(GetComponentsInChildren<DisperserRendererManager>());

			if (disperserRenderersManager == null) 
			{
				DisperserError.error = true;
				Debug.LogWarning ("The GameObject doesn't have childs with DisperserRendererManager");
				Destroy (this);
				return;
			}

			for (int i = 0; i < disperserAnimators.Count; i++) 
			{
				disperserAnimators[i].OnDisperserActivatedMesh += HandleOnDisperserActivatedMesh;
				disperserAnimators[i].OnDisperserDeactivatedMesh += HandleOnDisperserDeactivatedMesh;
			}

			for (int j = 0; j < disperserRenderersManager.Count; j++) 
			{
				disperserRenderersManager [j].trianglesPerSubMesh = trianglesPerSubMesh;
			}
		}
	}

	void OnDestroy()
	{
		for (int i = 0; i < disperserAnimators.Count; i++) 
		{
			disperserAnimators[i].OnDisperserActivatedMesh -= HandleOnDisperserActivatedMesh;
			disperserAnimators[i].OnDisperserDeactivatedMesh -= HandleOnDisperserDeactivatedMesh;
		}
	}

	public DisperserState State
	{
		get	{ return disperserAnimators [0].State; }
	}

	/// <summary>
	/// You can use this to catch when a mesh has been deactivated.
	/// </summary>
	void HandleOnDisperserDeactivatedMesh (Renderer renderer)
	{
		if (OnDisperserDeactivatedMesh != null) 
		{
			OnDisperserDeactivatedMesh (renderer);
		}
	}

	/// <summary>
	/// You can use this to catch when a mesh has been deactivated.
	/// </summary>
	void HandleOnDisperserActivatedMesh (Renderer renderer)
	{
		if (OnDisperserActivatedMesh != null) 
		{
			OnDisperserActivatedMesh (renderer);
		}
	}

	/// <summary>
	/// Starts the mesh activation for all the DisperserAnimators (all the renderers). Thousands of tiny submeshes will join together to form the right mesh. The final position of the submeshes is the right mesh that you are using in the right position it is.
	/// </summary>
	/// <param name="speed">Velocity of the activation.</param>
	/// <param name="distance">This determines a constant distance from the final position of the submesh, that the submeshes begin to move.</param>
	/// <param name="random">A random value for the start position of the submeshes in a Sphere.</param>
	/// <param name="factorChaos">If the value is lower, the submeshes will start in accordance with the "random" property above. If not, the submeshes will start in chaotical positions (Not inside a Sphere) </param>
	public bool StartActivate(float _speed,Vector3 _distance,float _random,float _factorChaos,bool _threeHundredSixtyDegreesEffect)
	{
		bool ok = true;

		if( ! DisperserError.error)
		{
			SetDisperserAnimatorValues (_speed, _distance, _random, _factorChaos,false,_threeHundredSixtyDegreesEffect);

			int i = 0;

			while (i < disperserAnimators.Count && ok) 
			{
				ok = disperserAnimators [i].CanStartActivate ();
				i++;
			}

			if (ok) 
			{
				for (int j = 0; j < disperserAnimators.Count; j++) 
				{				
					disperserAnimators [j].StartActivate (speed, distance, random, factorChaos,threeHundredSixtyDegreesEffect);
				}
			}
		}
		else
		{
			ok = false;
		}	

		return ok;
	}

	/// <summary>
	/// Starts the mesh deactivation for all the DisperserAnimators (all the renderers). The mesh will subdivide in thousands of tiny submeshes, that will travel trough space, and disappear. The final position of the submeshes is a position in some part of the scene.
	/// </summary>
	/// <param name="speed">Velocity of the deactivation</param>
	/// <param name="distance">This determines a constant distance from the final position of the submesh, that submeshes begin to move.</param>
	/// <param name="random">A random value for the end position of the submeshes in a Sphere.</param>
	/// <param name="factorChaos">If the value is lower, the submeshes will start in accordance with the "random" property above. If not, the submeshes will start in chaotical positions (Not inside a Sphere)</param>
	public bool StartDeactivate(float _speed,Vector3 _distance,float _random,float _factorChaos,bool _fadeOut,bool _threeHundredSixtyDegreesEffect)
	{
		bool ok = true;

		if( ! DisperserError.error)
		{
			SetDisperserAnimatorValues (_speed, _distance, _random, _factorChaos,_fadeOut,_threeHundredSixtyDegreesEffect);

			int i = 0;

			while (i < disperserAnimators.Count && ok) 
			{
				ok = disperserAnimators [i].CanStartDeactivate();
				i++;
			}

			if (ok) 
			{
				for (int j = 0; j < disperserAnimators.Count; j++) 
				{
					disperserAnimators [j].StartDeactivate (speed, distance, random, factorChaos,fadeOut,threeHundredSixtyDegreesEffect);
				}
			}
		}
		else
		{
			ok = false;
		}

		return ok;
	}

	/// <summary>
	/// Simply sets all propertys of this script.
	/// </summary>
	/// <param name="speed">Velocity of the deactivation</param>
	/// <param name="distance">This determines a constant distance from the final position of the submesh, that submeshes begin to move.</param>
	/// <param name="random">A random value for the end position of the submeshes in a Sphere.</param>
	/// <param name="factorChaos">If the value is lower, the submeshes will start in accordance with the "random" property above. If not, the submeshes will start in chaotical positions (Not inside a Sphere)</param>
	private void SetDisperserAnimatorValues(float _speed,Vector3 _distance,float _random,float _factorChaos,bool _fadeOut,bool _threeHundredSixtyDegreesEffect)
	{
		speed = _speed;
		distance = _distance;
		random = _random;
		factorChaos = _factorChaos;
		fadeOut = _fadeOut;
		threeHundredSixtyDegreesEffect = _threeHundredSixtyDegreesEffect;
	}





}
