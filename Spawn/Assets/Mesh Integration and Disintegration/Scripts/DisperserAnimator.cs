using UnityEngine;

/// <summary>
/// Animates the real mesh by activating or deactiving it. Makes the animation. The explosion or implosion that you saw before.
/// </summary>

public class DisperserAnimator : MonoBehaviour 
{
	public float speed = 0.25f;							
	public Vector3 distance = new Vector3(0f,2f,0f);
	public float random = 10f;
	public float factorChaos = 1f;
	public bool fadeOut = true;
	public float activateFadeTransitionSpeed = 6f;
	public bool threeHundredSixtyDegreesEffect = false;
	public GameObject disperserManagerPrefab;

	// I will recommend you to not use this delegate, you can always use the Disperser class that serves as an interface instead.
	public delegate void DisperserActivatedDelegate(Renderer renderer);
	public event DisperserActivatedDelegate OnDisperserActivatedMesh;

	// I will recommend you to not use this delegate, you can always use the Disperser class that serves as an interface instead.
	public delegate void DisperserDeactivatedDelegate(Renderer renderer);
	public event DisperserDeactivatedDelegate OnDisperserDeactivatedMesh;


	// Depending on which is the currently disperserState, this script will decide what to do.
	private DisperserState disperserState;

	private float lerpPosition;

	// All the thousands and tiny submeshes that the "real" mesh is subdivided.
	private DisperserAnimatorChild[] childs;

	// This is the real and only one mesh.
	private Renderer renderer2;
	private Collider collider2;
	private Disperser disperser;
	private DisperserProperties disperserProperties;

	public DisperserState State
	{
		get	{ return disperserState; }
	}

	void Awake()
	{
		if( ! DisperserError.error)
		{
			disperserState = DisperserState.deactivated;
			CheckForDisperserManager ();
		}
	}

	void Start()
	{
		if( ! DisperserError.error)
		{
			childs = GetComponentsInChildren<DisperserAnimatorChild> ();
			renderer2 = GetComponent<Renderer> ();
			collider2 = GetComponent<Collider> ();
			disperser = GetComponentInParent<Disperser> ();

			if (renderer2 == null) 
			{
				DisperserError.error = true;
				Debug.LogWarning ("You have to place your MeshRenderer or SkinnedMeshRenderer on this GameObject");
				Destroy (this);
				return;
			}

			if (disperser == null) 
			{
				DisperserError.error = true;
				Debug.LogWarning ("You have to attach a Disperser GameObject to the parent of all the childs.");
				Destroy (this);
				return;
			}

			if (collider2 != null) 
			{
				collider2.enabled = true;
			}

			renderer2.enabled = false;

			DeactivateAllRenderers ();
		}
	}

	void Update()
	{
		if( ! DisperserError.error)
		{
			if (disperserState == DisperserState.activating) 
			{
				Activate ();
			}
			
			if (disperserState == DisperserState.deactivating) 
			{
				Deactivate ();
			}

			if (disperserState == DisperserState.activatingFadeTransition) 
			{
				if (ActivateFadeTransition ()) 
				{
					// Now i'm activated
					disperserState = DisperserState.activated;
					// I don't need the thousands and tiny submeshes.
					DeactivateAllRenderers();
					// But i need my real mesh enabled.
					renderer2.enabled = true;

					if (collider2 != null) 
					{
						collider2.enabled = true;
					}

					if (OnDisperserActivatedMesh != null) 
					{
						OnDisperserActivatedMesh (renderer2);
					}
				}
			}
		}
	}

	/// <summary>
	/// Checks for disperser manager GameObject.
	/// </summary>
	private void CheckForDisperserManager()
	{
		if(disperserProperties == null)
		{
			GameObject disperserManager = GameObject.Find ("Disperser Manager");

			if (disperserManager == null) 
			{
				if (disperserManagerPrefab == null) 
				{
					DisperserError.error = true;
					Debug.LogWarning ("There is no disperserManagerPrefab Component attached on the DisperserMeshSlicer GameObject. Please attach one.");
					Destroy (this);
					return;
				}

				disperserManager = (GameObject)Instantiate (disperserManagerPrefab, Vector3.zero, Quaternion.identity);
				disperserManager.name = "Disperser Manager";
			}

			disperserProperties = disperserManager.GetComponent<DisperserProperties> ();

			if (disperserProperties == null) 
			{			
				DisperserError.error = true;
				Debug.LogWarning ("There is no DisperserProperties Component attached on the Disperser Manager GameObject. Please attach it.");
				Destroy (this);
				return;
			}
		}
	}


	/// <summary>
	/// This function is deprecated. But for now i want to have it, so please don't remove it. It makes an alpha transition between the thousands of childs and the real renderer. That could work if they (The childs and the real renderer) were different. But they aren't.
	/// </summary>
	private bool ActivateFadeTransition()
	{
		if (disperserProperties.uvMapping) 
		{
			lerpPosition = 1f;
		} else 
		{
			lerpPosition += Time.deltaTime * activateFadeTransitionSpeed;
		}

		for (int x = 0; x < renderer2.materials.Length; x++) 
		{
			Color color = renderer2.materials [x].color;
			renderer2.materials [x].color = new Color (color.r, color.g, color.b, Mathf.Lerp (0f, 1f, lerpPosition));
		}

		// Iterates through all the thousands and tiny submeshes
		for (int i = 0; i < childs.Length; i++) 
		{
			if (lerpPosition < 1f) 
			{
				for (int j = 0; j < childs [i].meshRenderer.materials.Length; j++) 
				{
					Color color = childs [i].meshRenderer.materials [j].color;
					childs [i].meshRenderer.materials [j].color = new Color (color.r, color.g, color.b, Mathf.Lerp (1f, 0f, lerpPosition));
				}
			} else 
			{
				for (int j = 0; j < childs [i].meshRenderer.materials.Length; j++) 
				{
					Color color = childs [i].meshRenderer.materials [j].color;
					childs [i].meshRenderer.materials [j].color = new Color (color.r, color.g, color.b, 0f);
				}
			}
		}

		return lerpPosition >= 1f;
	}

	/// <summary>
	/// Determines whether this animator can start activation.
	/// </summary>
	/// <returns>Returns true if this animator can start activation</returns>
	public bool CanStartActivate()
	{
		return (disperserState == DisperserState.deactivated || disperserState == DisperserState.activated);
	}

	/// <summary>
	/// Determines whether this animator can start deactivation.
	/// </summary>
	/// <returns>Returns true if this animator can start deactivation</returns>
	public bool CanStartDeactivate()
	{
		return (disperserState == DisperserState.deactivated || disperserState == DisperserState.activated);
	}

	/// <summary>
	/// Starts the mesh activation. Thousands of tiny submeshes will join together to form the right mesh. The final position of the submeshes is the right mesh that you are using in the right position it is.
	/// </summary>
	/// <param name="speed">Velocity of the activation.</param>
	/// <param name="distance">This determines a constant distance from the final position of the submesh, that the submeshes begin to move.</param>
	/// <param name="random">A random value for the start position of the submeshes in a Sphere.</param>
	/// <param name="factorChaos">If the value is lower, the submeshes will start in accordance with the "random" property above. If not, the submeshes will start in chaotical positions (Not inside a Sphere) </param>
	public bool StartActivate(float _speed,Vector3 _distance,float _random,float _factorChaos,bool _threeHundredSixtyDegreesEffect)
	{
		bool ok = false;

		if( ! DisperserError.error)
		{
			if (disperserState == DisperserState.deactivated || disperserState == DisperserState.activated ) 
			{
				speed = _speed;
				distance = _distance;
				random = _random;
				disperserState = DisperserState.activating;
				threeHundredSixtyDegreesEffect = _threeHundredSixtyDegreesEffect;

				if (speed <= 0f) 
				{
					DisperserError.error = true;
					Debug.LogWarning ("The speed value cannot be negative.");
					Destroy (this);
					return false;
				}

				if (collider2 != null) 
				{
					collider2.enabled = false;
				}

				// Now, all the thousands of tiny submeshes are activated, and the unique and normal mesh renderer will be deactivated for a while.
				ActivateAllRenderers();
				renderer2.enabled = false;
				factorChaos = _factorChaos;

				float multiplier;
				fadeOut = false;

				// Iterates through all the thousands and tiny submeshes, and sets its start positions / end positions and start rotations / end rotations.
				// We will use this later to make a motion tween. All the values are set according to the param values.
				for(int i = 0; i < childs.Length; i++)
				{
					multiplier = Random.Range(1f,factorChaos);
					childs[i].StartPosition = disperser.transform.position + distance + (Random.insideUnitSphere * random) * multiplier;
					childs[i].EndPosition = disperser.transform.position;
					//childs[i].StartRotation = Quaternion.Euler(Vector3.one * Random.Range(1f,360f));
					childs[i].StartRotation = Quaternion.Euler(Vector3.one * 360f);
					childs [i].EndRotation = Quaternion.identity;	
					childs [i].transform.position = childs [i].StartPosition;
					childs [i].transform.localRotation = childs [i].StartRotation;

					lerpPosition = 0f;

					for (int j = 0; j < childs [i].meshRenderer.materials.Length; j++) 
					{
						Color color = childs [i].meshRenderer.materials [j].color;
						childs [i].meshRenderer.materials [j].color = new Color (color.r, color.g, color.b, 1f);
					}
				}

				ok = true;
			}
		}

		return ok;
	}

	/// <summary>
	/// Starts the mesh deactivation. The mesh will subdivide in thousands of tiny submeshes, that will travel trough space, and disappear. The final position of the submeshes is a position in some part of the scene.
	/// </summary>
	/// <param name="speed">Velocity of the deactivation</param>
	/// <param name="distance">This determines a constant distance from the final position of the submesh, that submeshes begin to move.</param>
	/// <param name="random">A random value for the end position of the submeshes in a Sphere.</param>
	/// <param name="factorChaos">If the value is lower, the submeshes will start in accordance with the "random" property above. If not, the submeshes will start in chaotical positions (Not inside a Sphere)</param>
	public bool StartDeactivate(float _speed,Vector3 _distance,float _random,float _factorChaos,bool _fadeOut,bool _threeHundredSixtyDegreesEffect)
	{
		bool ok = false;

		if( ! DisperserError.error)
		{
			if (disperserState == DisperserState.deactivated || disperserState == DisperserState.activated ) 
			{
				speed = _speed;
				distance = _distance;
				random = _random;
				disperserState = DisperserState.deactivating;
				fadeOut = _fadeOut;
				threeHundredSixtyDegreesEffect = _threeHundredSixtyDegreesEffect;

				if (speed <= 0f) 
				{
					DisperserError.error = true;
					Debug.LogWarning ("The speed value cannot be negative.");
					Destroy (this);
					return false;
				}

				if (collider2 != null) 
				{
					collider2.enabled = false;
				}

				// Now, all the thousands of tiny submeshes are activated, and the unique and normal mesh renderer will be deactivated for a while.
				ActivateAllRenderers();
				renderer2.enabled = false;


				factorChaos = _factorChaos;

				float multiplier;

				// Iterates through all the thousands and tiny submeshes, and sets its start positions / end positions and start rotations / end rotations.
				// We will use this later to make a motion tween. All the values are set according to the param values.
				for(int i = 0; i < childs.Length; i++)
				{
					multiplier = Random.Range(1f,factorChaos);
					childs[i].StartPosition = disperser.transform.position;
					childs[i].EndPosition = disperser.transform.position + distance + (Random.insideUnitSphere * random) * multiplier;
					childs[i].StartRotation = Quaternion.identity;
					//childs[i].EndRotation = Quaternion.Euler(Vector3.one * Random.Range(1f,360f));
					childs[i].StartRotation = Quaternion.Euler(Vector3.one * 360f);
					childs [i].transform.position = childs [i].StartPosition;
					childs [i].transform.localRotation = childs [i].StartRotation;

					for (int j = 0; j < childs [i].meshRenderer.materials.Length; j++) 
					{
						Color color = childs [i].meshRenderer.materials [j].color;
						childs [i].meshRenderer.materials [j].color = new Color (color.r, color.g, color.b, 1f);
					}

					lerpPosition = 0f;
				}
				ok = true;
			}
		}

		return ok;
		
	}

	/// <summary>
	/// It activates the mesh by repeated calls on the Update method.
	/// </summary>
	private void Activate()
	{
		// ¿Did i finished the activation of the mesh?
		if (Lerp())
		{
			// Now i have to make a transition between the tiny meshes and the real mesh
			disperserState = DisperserState.activatingFadeTransition;
			lerpPosition = 0f;
			renderer2.enabled = true;

			for (int x = 0; x < renderer2.materials.Length; x++) 
			{
				Color color = renderer2.materials [x].color;
				renderer2.materials [x].color = new Color (color.r, color.g, color.b, Mathf.Lerp (0f, 1f, 0f));
			}
		}
	}


	/// <summary>
	/// It deactivates the mesh by repeated calls on the Update method.
	/// </summary>
	private void Deactivate()
	{
		// ¿Did i finished the deactivation of the mesh?

		if (Lerp())
		{
			// Now i'm deactivated
			disperserState = DisperserState.deactivated;
			// I don't need the thousands and tiny submeshes.
			DeactivateAllRenderers();
			// But i need my real mesh enabled.
			renderer2.enabled = false;

			if (collider2 != null) 
			{
				collider2.enabled = false;
			}

			if (OnDisperserDeactivatedMesh != null) 
			{
				OnDisperserDeactivatedMesh (renderer2);
			}
		}
	}

	/// <summary>
	/// Activates all the thousands and tiny submeshes.
	/// </summary>
	private void ActivateAllRenderers()
	{
		for (int i = 0; i < childs.Length; i++) 
		{
			childs[i].meshRenderer.enabled = true;
		}
	}

	/// <summary>
	/// Deactivates all the thousands and tiny submeshes.
	/// </summary>
	private void DeactivateAllRenderers()
	{
		for(int i = 0; i < childs.Length; i++)
		{
			childs[i].meshRenderer.enabled = false;
		}
	}

	/// <summary>
	/// It makes a lerp between all the thousands and tiny submeshes of your real mesh. It does the animation itself.
	/// </summary>
	private bool Lerp()
	{
		lerpPosition += Time.deltaTime * speed;
		float lerpRotation = lerpPosition;

		// Iterates through all the thousands and tiny submeshes
		for(int i = 0; i < childs.Length; i++)
		{
			// Do the position and rotation lerp until it is ended.
			if(lerpPosition < 1f)
			{
				childs[i].transform.position = Vector3.Lerp(childs[i].StartPosition,childs[i].EndPosition,lerpPosition);

				if(threeHundredSixtyDegreesEffect)
				{
					childs[i].transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.one * 360f,Vector3.zero,lerpRotation));
				}
				else
				{
					childs[i].transform.localRotation = Quaternion.Euler(Vector3.Lerp(childs[i].StartRotation.eulerAngles,childs[i].EndRotation.eulerAngles,lerpRotation));
				}


				if (fadeOut) 
				{
					for (int j = 0; j < childs [i].meshRenderer.materials.Length; j++) 
					{
						Color color = childs [i].meshRenderer.materials [j].color;
						childs [i].meshRenderer.materials [j].color = new Color (color.r, color.g, color.b, Mathf.Lerp (1f, 0f, lerpPosition));
					}
				}
			}
			else
			{
				childs[i].transform.position = childs[i].EndPosition;
				childs[i].transform.localRotation = Quaternion.identity;

				if (fadeOut) 
				{
					for (int j = 0; j < childs [i].meshRenderer.materials.Length; j++) 
					{
						Color color = childs [i].meshRenderer.materials [j].color;
						childs [i].meshRenderer.materials [j].color = new Color (color.r, color.g, color.b, 0f);
					}
				}
			}
		}
		
		return (lerpPosition >= 1f);
	}

}