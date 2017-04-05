using UnityEngine;

/// <summary>
/// Check if the object has a mesh renderer or a skinnermeshrenderer and calls one script or another in concequence.
/// </summary>

public class DisperserRendererManager : MonoBehaviour 
{
	public int trianglesPerSubMesh = 1;
	public GameObject prefabSubmesh;

	private MeshRenderer meshRenderer;
	private SkinnedMeshRenderer skinnedMeshRenderer;

	private DisperserMeshSlicer disperserMeshSlicer;
	private DisperserSkinnedSlicer disperserSkinnedSlicer;

	void Start()
	{
		if( ! DisperserError.error)
		{
			meshRenderer = GetComponent<MeshRenderer> ();
			skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer> ();
			disperserMeshSlicer = GetComponent<DisperserMeshSlicer> ();
			disperserSkinnedSlicer = GetComponent<DisperserSkinnedSlicer>();

			if (prefabSubmesh == null) 
			{
				DisperserError.error = true;
				Debug.LogWarning ("There is no prefabSubmesh Component attached on the DisperserRendererManager GameObject. Please attach one.");
				Destroy (this);
				return;
			}

			if (meshRenderer != null) 
			{
				disperserMeshSlicer.Slice(prefabSubmesh);
			} else 
			{
				if (skinnedMeshRenderer != null) 
				{
					disperserSkinnedSlicer.Slice (prefabSubmesh);
				} else 
				{
					DisperserError.error = true;
					Debug.LogWarning ("You must have one MeshRenderer or SkinnedMeshRenderer in this GameObject.");
					return;
				}
			}
		}
		else
		{
			return;
		}

	}
}
