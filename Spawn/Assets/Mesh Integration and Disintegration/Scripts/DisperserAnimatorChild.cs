using UnityEngine;

/// <summary>
/// This is a class to access a child mesh renderer. Remember, thousands and tiny submeshes are created to make this effect possible. So this is one of those thousands submeshes.
/// And also has all the propertys to make sure the animation can be done.
/// </summary>


public class DisperserAnimatorChild : MonoBehaviour 
{
	private Vector3 startPosition;
	private Vector3 endPosition;
	private Quaternion startRotation;
	private Quaternion endRotation;
	private MeshRenderer _meshRenderer;

	void Awake()
	{
		_meshRenderer = GetComponent<MeshRenderer> ();
	}

	public Vector3 StartPosition
	{
		set { startPosition = value; }
		get { return startPosition; }
	}

	public Vector3 EndPosition
	{
		set { endPosition = value; }
		get { return endPosition; }
	}

	public Quaternion StartRotation
	{
		set { startRotation = value; }
		get { return startRotation; }
	}

	public Quaternion EndRotation
	{
		set { endRotation = value; }
		get { return endRotation; }
	}

	public MeshRenderer meshRenderer
	{
		get
		{
			return _meshRenderer;
		}
	}


}
