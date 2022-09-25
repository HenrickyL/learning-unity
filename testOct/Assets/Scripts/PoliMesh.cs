using System.Collections.Generic;
using System.Threading.Tasks;
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
		for (int i = 0; i < cells.Length; i++) {
			Triangulate(cells[i],i);
		}
		poliMesh.vertices = vertices.ToArray();
		poliMesh.triangles = triangles.ToArray();
		poliMesh.colors = colors.ToArray();
		poliMesh.RecalculateNormals();
	}
	
	void Triangulate (Cell cell, int j) {
		var metric = new Metrics(j+4);
		Vector3 center = cell.transform.localPosition;
		for (int i = 0; i < metric.numCorners; i++) {
			AddTriangle(
				center,
				center + metric.Corners[i],
				center + metric.Corners[i+1]
			);
			AddTriangleColor(new Color(0,0,i*30));
		}
	}
	void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3) {
		int vertexIndex = vertices.Count;
		vertices.Add(v1);
		vertices.Add(v2);
		vertices.Add(v3);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
	}
	void AddTriangleColor (Color color) {
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
	}
}
