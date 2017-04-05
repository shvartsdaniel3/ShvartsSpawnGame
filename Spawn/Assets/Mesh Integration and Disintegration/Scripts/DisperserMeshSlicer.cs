using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This script works for MeshRenderers (Not Skinned)
/// </summary>

public class DisperserMeshSlicer : MonoBehaviour 
{
	public GameObject disperserManagerPrefab;
	public GameObject parentSubmeshPrefab;
	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private DisperserRendererManager disperserRendererManager;
	private DisperserProperties disperserProperties;

	void Awake()
	{
		if ( ! DisperserError.error)
		{
			meshRenderer = GetComponent<MeshRenderer> ();
			meshFilter = GetComponent<MeshFilter> ();
			disperserRendererManager = GetComponent<DisperserRendererManager> ();
			CheckForDisperserManager ();
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
	/// Subdivide the mesh into thousands of tiny submeshes. Creates one GameObject for every submesh generated, and assign all the components and propertys to make it visible in the scene, and as a child
	/// of the parent mesh.
	/// </summary>
	/// <param name="prefab">Prefab of the GameObject used for assign to every submesh generated. The submesh by itself does not have any sense. It needs to be into a GameObject.</param>
	public void Slice(GameObject prefab)
	{
		if( ! DisperserError.error)
		{
			if (meshRenderer == null) 
			{
				meshRenderer = GetComponent<MeshRenderer> ();

				if (meshRenderer == null) 
				{
					DisperserError.error = true;
					Debug.LogWarning ("You must have a MeshRenderer.");
					Destroy (this);
					return;
				}
			}

			if (meshFilter == null) 
			{
				meshFilter = GetComponent<MeshFilter> ();

				if (meshFilter == null) 
				{
					DisperserError.error = true;
					Debug.LogWarning ("You must have a MeshFilter.");
					Destroy (this);
					return;
				}
			}

			CheckForDisperserManager ();
			parentSubmeshPrefab = disperserProperties.submeshesParentPrefab;

			if (disperserRendererManager == null) 
			{
				disperserRendererManager = GetComponent<DisperserRendererManager> ();

				if (disperserRendererManager == null) 
				{
					disperserRendererManager = gameObject.AddComponent<DisperserRendererManager> ();
					disperserRendererManager.prefabSubmesh = disperserProperties.prefabSubmesh;
				}
			}

			// Subdivide the mesh into thousands of tiny submeshes and get the list of those submeshes.
			List<Mesh> submeshes = DisperserMeshesSlicer.SliceIntoMeshes (meshFilter.mesh, disperserRendererManager.trianglesPerSubMesh,disperserProperties.uvMapping);

			if (submeshes != null) 
			{
				// For every submesh in the list, creates a GameObject and prepare it to make it visible and useful in the scene.
				CreateSubGameObjects (submeshes, prefab);
				meshRenderer.enabled = false;
			} else 
			{
				Debug.LogWarning ("Cannot create submeshes. Please review the log warnings.");
				return;
			}
		}
		else
		{
			return;
		}
	}

	/// <summary>
	/// Receives a list of submeshes, and for every submesh, instantiates a GameObject and prepare it to make it visible and useful in the scene.
	/// </summary>
	/// <param name="submeshes">List of submeshes.</param>
	/// <param name="prefab">Prefab to create a game object and assign the properly submesh.</param>
	private void CreateSubGameObjects(List<Mesh> submeshes,GameObject prefab)
	{
		GameObject goSubmesh;
		GameObject goParent;

		goParent = (GameObject)Instantiate (parentSubmeshPrefab, Vector3.zero, Quaternion.identity);
		goParent.name = "Submeshes";		
		goParent.transform.parent = transform;
		goParent.transform.localPosition = Vector3.zero;
		goParent.transform.localRotation = Quaternion.identity;

		// Iterates through all the submeshes
		for (int i = 0; i < submeshes.Count; i++) 
		{
			// Instantiates a GameObject, as a child.
			goSubmesh = (GameObject)Instantiate(prefab,transform.position,transform.rotation);
			goSubmesh.name = "Submesh " + i.ToString();	
			goSubmesh.transform.parent = goParent.transform;
			goSubmesh.transform.localScale = new Vector3(1f,1f,1f);
			MeshFilter meshFilter = goSubmesh.GetComponent<MeshFilter>();
			MeshRenderer meshRendererChild = goSubmesh.GetComponent<MeshRenderer>();
			// Assign the submesh into the meshfilter component of the recently instantiated GameObject.
			meshFilter.mesh = submeshes[i];
			meshRendererChild.enabled = false;

			if (disperserProperties.uvMapping) 
			{
				meshRendererChild.materials = meshRenderer.sharedMaterials;
			}
		}		
	}
}
