using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Cell : MonoBehaviour
{
    //try use this
    //https://youtu.be/zTYFgZmS9tQ
    public int numVertices = 6;
	public bool oriented = false;
    Canvas gridCanvas;
    Mesh mesh;
	List<Vector3> vertices;
	List<int> triangles;
	List<Color> colors;
    void Awake() {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
    }
   
    void Start () {
		vertices = new List<Vector3>();
		triangles = new List<int>();
		colors = new List<Color>();
		mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        Triangulate();
    	UpdateMesh();
	}
	void Update() {
		UpdateMesh();
	}

    public static Cell CreateCell (Metrics metrics) {
		Cell cell = Instantiate<Cell>(new Cell());
        cell.UpdateMesh();
        return cell;
	}

    public void UpdateMesh() {
		mesh.Clear();
		vertices.Clear();
		triangles.Clear();
		colors.Clear();
        this.Triangulate();
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.colors = colors.ToArray();
		mesh.RecalculateNormals();
	}


    void Triangulate () {
		var metric = new Metrics(numVertices,oriented);
		Vector3 center = this.transform.localPosition;
		for (int i = 0; i < metric.numCorners; i++) {
			Debug.Log($"i :{i}");
			Debug.Log(metric.Corners[i]);
			Debug.Log(vertices.Count);

			AddTriangle(
				center,
				center + metric.Corners[i],
				center + metric.Corners[i+1]
			);
			AddTriangle(
				center + metric.Corners[i+1],
				center + metric.Corners[i],
				center
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
