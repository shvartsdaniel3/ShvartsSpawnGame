using UnityEngine;
using System.Collections.Generic;

/// <summary>
/*
	Add this class to the parent GameObject (whose children has one ore more Renderers) and everything will work without doing anything else.
	If you see any warning, please pay attention to them.
 */ 
/// </summary>

[ExecuteInEditMode]
public class DisperserBuilder : MonoBehaviour 
{
	public GameObject disperserManagerPrefab;
	private Disperser disperser;
	private DisperserAnimatorManager disperserAnimatorManager;
	private DisperserProperties disperserProperties;

	/// <summary>
	/// Add all scripts neccesary to run "Mesh Integration and Disintegration" in one particular GameObject.
	/// </summary>
	void Start()
	{
		disperser = GetComponent<Disperser> ();
		disperserAnimatorManager = GetComponent<DisperserAnimatorManager> ();
		DisperserBuilder [] disperserBuilders = GetComponents<DisperserBuilder> ();

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
			DestroyImmediate (this);
			return;
		}

		if (disperserBuilders.Length == 1) 
		{		
			if (disperser == null) 
			{
				disperser = gameObject.AddComponent<Disperser> ();
			}

			if (disperserAnimatorManager == null) 
			{
				disperserAnimatorManager = gameObject.AddComponent<DisperserAnimatorManager> ();
			}
			
			List<Renderer> renderers = new List <Renderer> (GetComponentsInChildren<Renderer> ());
			bool validRenderer = true;

			DisperserAnimator disperserAnimator;
			DisperserRendererManager disperserRendererManager;
			DisperserMeshSlicer disperserMeshSlicer;
			DisperserSkinnedSlicer disperserSkinnedSlicer;

			int countRenderers = 0;

			for (int i = 0; i < renderers.Count; i++) 
			{
				disperserAnimator = renderers [i].gameObject.GetComponent<DisperserAnimator> ();
				disperserRendererManager = renderers [i].gameObject.GetComponent<DisperserRendererManager> ();
				disperserMeshSlicer = renderers [i].gameObject.GetComponent<DisperserMeshSlicer> ();
				disperserSkinnedSlicer = renderers [i].gameObject.GetComponent<DisperserSkinnedSlicer> ();

				if (renderers[i] is MeshRenderer) 
				{
					if (disperserMeshSlicer == null) 
					{
						disperserMeshSlicer = renderers [i].gameObject.AddComponent<DisperserMeshSlicer> ();
						disperserMeshSlicer.disperserManagerPrefab = disperserManagerPrefab;
						disperserMeshSlicer.parentSubmeshPrefab = disperserProperties.submeshesParentPrefab;
					}
					countRenderers++;
				} else 
				{
					if (renderers[i] is SkinnedMeshRenderer) 
					{
						if (disperserSkinnedSlicer == null) 
						{
							disperserSkinnedSlicer = renderers [i].gameObject.AddComponent<DisperserSkinnedSlicer> ();
							disperserSkinnedSlicer.disperserManagerPrefab = disperserManagerPrefab;
							disperserSkinnedSlicer.parentSubmeshPrefab = disperserProperties.submeshesParentPrefab;
						}
						countRenderers++;
					} else 
					{
						validRenderer = false;
					}
				}

				if (validRenderer) 
				{
					if (disperserAnimator == null) 
					{
						disperserAnimator = renderers [i].gameObject.AddComponent<DisperserAnimator> ();
						disperserAnimator.disperserManagerPrefab = disperserManagerPrefab;
					}

					if (disperserRendererManager == null) 
					{
						disperserRendererManager = renderers [i].gameObject.AddComponent<DisperserRendererManager> ();
						disperserRendererManager.prefabSubmesh = disperserProperties.prefabSubmesh;
					}
				}
			}

			Debug.Log ("All Disperser classes have been added successfully. Renderers count: " + countRenderers.ToString());
			DestroyImmediate (this);
		}
		else
		{
			Debug.LogWarning ("You cannot have two or more DisperserBuilder classes in one GameObject");
			DestroyImmediate (this);
		}
	}
}
