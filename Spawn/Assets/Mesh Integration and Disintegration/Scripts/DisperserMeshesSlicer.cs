using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// This is the most important class in all the package. It takes one mesh and subdivides it in thousands of tiny submeshes. 
/// </summary>

public class DisperserMeshesSlicer
{
	static Dictionary<int,int> newVectices;

	/// <summary>
	/// Receives one mesh and subdivides it in thousands of tiny submeshes.
	/// </summary>
	/// <returns>The thousand of tiny submeshes list.</returns>
	/// <param name="mesh">The mesh you want to subdivide.</param>
	/// <param name="trianglesPerSubmesh">How many triangles will have the submeshes. How large will be.</param>
	public static List<Mesh> SliceIntoMeshes(Mesh mesh,int trianglesPerSubmesh,bool uvMapping)
	{
		if (mesh == null) 
		{
			DisperserError.error = true;
			Debug.LogWarning ("There is no mesh to slice. Please add one.");			
			return null;
		}

		if (trianglesPerSubmesh <= 0) 
		{
			DisperserError.error = true;
			Debug.LogWarning ("The TrianglesPerSubmesh value cannot be zero or negative.");
			return null;
		}


		// Creates a list of submeshes
		List<Mesh> submeshes = new List<Mesh>();

		// Calculates how many submeshes we will create.
		int amountSubmeshes = (mesh.triangles.Length / 3) / trianglesPerSubmesh;
		// Calculates vertices per submesh
		int verticesPerSubmesh = trianglesPerSubmesh * 3;

		int trianglesIndex = 0;
		Mesh submesh;
		List<Vector3> vertices;
		List<Vector3> normals;
		List<int> triangles;
		//List<Color> colors;
		List<Vector2> uvs;

		// Iterates through all submeshes.
		for (int submeshIndex = 0; submeshIndex < amountSubmeshes; submeshIndex++) 
		{
			// Now we have a new submesh
			submesh = new Mesh();
			// With new vertices
			vertices = new List<Vector3>();
			// With new normals
			normals = new List<Vector3>();
			// With new triangles
			triangles = new List<int>();
			newVectices = new Dictionary<int,int>();
			//colors = new List<Color> ();
			uvs = new List<Vector2>();

			int trianglesIndexAnt = trianglesIndex;

			// Iterates through all the triangles of this specific submesh
			for (int triangle = trianglesIndexAnt; triangle < trianglesIndexAnt + verticesPerSubmesh; triangle += 3,trianglesIndex += 3) 
			{
				if(triangle >= mesh.triangles.Length)
				{
					break;
				}

				// Iterates through all the three vertices that this triangle has
				for(int vertex = 0; vertex < 3; vertex++)
				{
					// Get the old vertex index
					int t1 = mesh.triangles[triangle + vertex];

					// Add the old vertex to a new list of vertices and normals (For the new submesh)
					int v1 = GetNewVertex(t1,mesh.vertices[t1],mesh.normals[t1],vertices,normals,uvs,mesh.uv[t1],uvMapping);

					// Now the new triangles, has one more new generated vertex.
					triangles.Add(v1);
				}
			}

			// Generates the submesh by assign the vertices, normals, and triangles previously generated.
			submesh.Clear();
			submesh.vertices = vertices.ToArray();
			submesh.normals = normals.ToArray();
			submesh.triangles = triangles.ToArray();

			if (uvMapping) 
			{
				submesh.uv = uvs.ToArray ();
			}

			submesh.RecalculateBounds();
			submesh.RecalculateNormals();
			submesh.Optimize();

			// Add the submesh to a list of submeshes
			submeshes.Add(submesh);

			triangles = null;
			vertices = null;
			triangles = null;
			newVectices = null;
		}

		// We return the list of tiny submeshes
		return submeshes;
	}


	/// <summary>
	/// Add a new vertex and normal to a vertices and normals list. If the vertex already exist (shared vertices) then just return it.
	/// </summary>
	/// <returns>The index of the vertex in the vertices array, to be added later in the triangles array.</returns>
	/// <param name="key">Index of the vertex in the old vertices list</param>
	/// <param name="vertex">New vertex to be added</param>
	/// <param name="normal">New normal to be added</param>
	/// <param name="vertices">New Vertices list of the new submesh.</param>
	/// <param name="normals">New Normals list of the new submesh</param>
	private static int GetNewVertex(int key,Vector3 vertex,Vector3 normal,List<Vector3> vertices,List<Vector3> normals,List<Vector2> uvs,Vector2 uv,bool uvMapping)
	{
		// If the vertex index already exist, just return it.
		if (newVectices.ContainsKey (key)) 
		{
			return newVectices [key];
		}
		else
		{
			// generate vertex:
			int newIndex = vertices.Count;
			newVectices.Add(key,newIndex);
			
			// calculate new vertex
			vertices.Add(vertex);
			normals.Add(normal);
			//colors.Add (color);

			if (uvMapping) 
			{
				uvs.Add (uv);
			}
			// [... all other vertex data arrays]

			return newIndex;
		}
	}
}