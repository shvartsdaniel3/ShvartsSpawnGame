using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This script works for SkinnedMeshRenderer (Not MeshRenderers)
/// </summary>
/// 
/// 
/// 
/*
[RequireComponent(typeof(DisperserAnimator))]
[RequireComponent(typeof(DisperserMeshSlicer))]
[RequireComponent(typeof(DisperserRendererManager))]
*/
public class DisperserSkinnedSlicer : MonoBehaviour 
{
	public GameObject disperserManagerPrefab;
	public GameObject parentSubmeshPrefab;
	private SkinnedMeshRenderer skinnedMeshRenderer;
	private DisperserRendererManager disperserRendererManager;
	private DisperserProperties disperserProperties;

	void Awake()
	{
		if( ! DisperserError.error)
		{
			skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer> ();
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
					Debug.LogWarning ("There is no disperserManagerPrefab Component attached on the DisperserSkinnedSlicer GameObject. Please attach one.");
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
			if (skinnedMeshRenderer == null) 
			{
				skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer> ();

				if (skinnedMeshRenderer == null) 
				{
					DisperserError.error = true;
					Debug.LogWarning ("You must have a MeshRenderer.");
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

			if (disperserRendererManager == null) 
			{
				disperserRendererManager = GetComponent<DisperserRendererManager> ();
			}

			List<Mesh> submeshes = DisperserMeshesSlicer.SliceIntoMeshes (skinnedMeshRenderer.sharedMesh, disperserRendererManager.trianglesPerSubMesh,disperserProperties.uvMapping);

			if (submeshes != null) 
			{
				CreateSubGameObjects (submeshes, prefab);
				skinnedMeshRenderer.enabled = false;
			} else 
			{
				Debug.LogWarning ("Cannot create submeshes. Please review the log warnings.");
				return;
			}
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

		for (int i = 0; i < submeshes.Count; i++) 
		{
			goSubmesh = (GameObject)Instantiate(prefab,transform.position,transform.rotation);
			goSubmesh.name = "Submesh " + i.ToString();
			goSubmesh.transform.parent = goParent.transform;
			goSubmesh.transform.localScale = new Vector3(1f,1f,1f);
			MeshFilter meshFilter = goSubmesh.GetComponent<MeshFilter>();
			MeshRenderer meshRendererChild = goSubmesh.GetComponent<MeshRenderer>();
			meshFilter.mesh = submeshes[i];
			meshRendererChild.enabled = false;

			if (disperserProperties.uvMapping) 
			{
				meshRendererChild.materials = skinnedMeshRenderer.sharedMaterials;
			}
		}		
	}

}
