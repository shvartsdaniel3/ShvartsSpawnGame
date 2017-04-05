using UnityEngine;
using System.Collections.Generic;

/// <summary>
/*
 	WARNING: This script will destroy every Disperser class from the parent GameObject to every of his childs. This script do not destroy anything else.
	It's the opposite script for DisperserBuilder. This destroys all things that DisperserBuilder creates. So, if you want to remove all Disperser components quickly, just add this class to the parent GameObject (Whose children have renderers).
 */ 
/// </summary>
[ExecuteInEditMode]
public class DisperserDestructor : MonoBehaviour 
{
	private Disperser disperser;
	private DisperserAnimatorManager disperserAnimatorManager;

	void Start()
	{
		disperser = GetComponent<Disperser> ();
		disperserAnimatorManager = GetComponent<DisperserAnimatorManager> ();
		DisperserDestructor [] disperserDestructors = GetComponents<DisperserDestructor> ();

		if (disperserDestructors.Length == 1) 
		{		
			if (disperser != null) 
			{
				DestroyImmediate (disperser);
			}

			if (disperserAnimatorManager != null) 
			{
				DestroyImmediate (disperserAnimatorManager);
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
					if (disperserMeshSlicer != null) 
					{
						DestroyImmediate (disperserMeshSlicer);
					}

					if (disperserSkinnedSlicer != null) 
					{
						DestroyImmediate (disperserSkinnedSlicer);
					}

					countRenderers++;
				} else 
				{
					if (renderers[i] is SkinnedMeshRenderer) 
					{
						if (disperserSkinnedSlicer != null) 
						{
							DestroyImmediate (disperserSkinnedSlicer);
						}

						if (disperserMeshSlicer != null) 
						{
							DestroyImmediate (disperserMeshSlicer);
						}

						countRenderers++;
					} else 
					{
						validRenderer = false;
					}
				}

				if (validRenderer) 
				{
					if (disperserAnimator != null) 
					{
						DestroyImmediate (disperserAnimator);
					}

					if (disperserRendererManager != null) 
					{
						DestroyImmediate (disperserRendererManager);
					}
				}
			}

			Debug.Log ("Successfully destroyed all classes related to dispersers. Renderers count: " + countRenderers.ToString());
			DestroyImmediate (this);
		}
		else
		{
			Debug.LogWarning ("You cannot have two or more DisperserDestructor classes in one GameObject");
			Destroy (this);
		}
	}
}
