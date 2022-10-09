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
	private Metrics metric;
	List<int> triangles;
	List<Color> colors;
	private Vector3 center;
	private Vector3 AnotherCenter{
		get{
			return center + HeightOffSet;
		}
	}

	private Vector3 HeightOffSet{
		get{
			return isUpDirection? Vector3.up*targetHeight: - Vector3.up*targetHeight;
		}
	}
	public bool isUpDirection = true;

	 [Range(0, 20f)]public float height = 3f;
    private float targetHeight = 0f;
	private int totalSteps = 16; // 16 morph steps
	public float duration = 4f; // 4 seconds
	private float mps; // morphs per second
	private float LerpTime{
		get{
			mps = (float)duration*1000/(float)totalSteps;
			float t = Time.realtimeSinceStartup; // Should actually be since animation start
			int arrayIndex = Mathf.FloorToInt( t / mps );
			float p = (t-((float)arrayIndex)*mps) / mps;
			return p;
		}
	}
	

    void Awake() {
		center = this.transform.localPosition;
		vertices = new List<Vector3>();
		triangles = new List<int>();
		colors = new List<Color>();
		mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        Triangulate();
    	UpdateMesh();
		LerpHight();
    }
   
	void Update() {
		UpdateMesh();
		LerpHight();
	}

	private void LerpHight(){
		if(height != targetHeight){
			Debug.Log($"hight: {targetHeight}");
			targetHeight = Mathf.Lerp(targetHeight, height, LerpTime);
		}
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
		metric = new Metrics(numVertices,oriented);
		for (int i = 0; i < metric.numCorners; i++) {
			
			AddTriangleUpFace(i,Color.blue);
			AddTriangleDownFace(i,Color.blue);
			AddQuadFace(i, Color.red, Color.green);
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
	void AddQuad (Vector3 v1, Vector3 v2, float height) {
		int vertexIndex = vertices.Count;
		vertices.Add(v1);
		vertices.Add(v2);
		vertices.Add(v2 +HeightOffSet);
		vertices.Add(v1);
		vertices.Add(v2 +HeightOffSet);
		vertices.Add(v1+HeightOffSet);
		/*
			0,1,2,
            1,3,2
		*/
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 5);
		triangles.Add(vertexIndex + 4);
		triangles.Add(vertexIndex + 3);
	}
	void AddQuadColor(Color c1, Color c2){
		colors.Add(c1);
		colors.Add(c1);
		colors.Add(c1);

		colors.Add(c2);
		colors.Add(c2);
		colors.Add(c2);
	}
	void AddQuadColor(Color color){
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
	}

	void AddTriangleColor (Color color) {
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
	}

	private void AddTriangleUpFace(int i, Color c)
	{
		if(isUpDirection){
			AddTriangle(
				AnotherCenter,
				AnotherCenter + metric.Corners[i],
				AnotherCenter + metric.Corners[i+1]
			);
		}else{
			AddTriangle(
				AnotherCenter + metric.Corners[i+1],
				AnotherCenter + metric.Corners[i],
				AnotherCenter
			);
		}
		AddTriangleColor(c);
	}

	private void AddTriangleDownFace(int i, Color c)
	{
		if(isUpDirection){
			AddTriangle(
				center + metric.Corners[i+1],
				center + metric.Corners[i],
				center
			);
		}else{
			AddTriangle(
				center,
				center + metric.Corners[i],
				center + metric.Corners[i+1]
			);
		}
		AddTriangleColor(c);
	}


	private void AddQuadFace(int i, Color c1, Color c2){
		if(isUpDirection){
			AddQuad(
				center + metric.Corners[i+1],
				center + metric.Corners[i],
				targetHeight
			);
		}else{
			AddQuad(
				center + metric.Corners[i],
				center + metric.Corners[i+1],
				targetHeight
			);
		}
		AddQuadColor(c1,c2);
	}
}
