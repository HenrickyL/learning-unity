using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PoliMesh : MonoBehaviour
{
    Mesh poliMesh;
	List<Vector3> vertices;
	List<int> triangles;
	List<Color> colors;
   
   void Awake () {
		GetComponent<MeshFilter>().mesh = poliMesh = new Mesh();
		poliMesh.name = "Hex Mesh";
		vertices = new List<Vector3>();
		triangles = new List<int>();
		colors = new List<Color>();
	}

    public void Triangulate (Cell[] cells) {
		poliMesh.Clear();
		vertices.Clear();
		triangles.Clear();
		colors.Clear();
		
		poliMesh.vertices = vertices.ToArray();
		poliMesh.triangles = triangles.ToArray();
		poliMesh.colors = colors.ToArray();
		poliMesh.RecalculateNormals();
	}
	
	
}
