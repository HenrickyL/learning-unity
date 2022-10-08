using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Cell : MonoBehaviour
{
    //try use this
    //https://youtu.be/zTYFgZmS9tQ
    Canvas gridCanvas;
    Mesh mesh;
	List<Vector3> vertices;
	List<int> triangles;
	List<Color> colors;
    public static Cell instance; 
    private Metrics  metrics;
    public Cell(){
        if(instance){
            instance = new Cell();
            instance.metrics = new Metrics(8);
        }
    }
    void Awake() {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
    }
   
    void Start () {
	}

    public static Cell CreateCell (Metrics metrics) {
		Cell cell = Instantiate<Cell>(new Cell());
        cell.metrics = metrics;
        cell.Clear();
        return cell;
	}

    public void Clear () {
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
		var metric = new Metrics(metrics.numCorners);
		Vector3 center = this.transform.localPosition;
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
