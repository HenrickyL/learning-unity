using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Cell : MonoBehaviour
{
	public static int count=-1;	
	public static int MAX = 50;
    [Range(3, 20)]private int numVertices = 6;
	public bool oriented = false;
    Canvas canvas;
	Text label;
    Mesh mesh;
	Color color = Color.blue;
	List<Vector3> vertices;
	private Metrics metric;
	List<int> triangles;
	List<Color> colors;
	[SerializeField]
	public Neighbors neighbors;
	public Text cellLabelPrefab;
	private Vector3 Center{
		get{
			return transform.position;
		}
	}
	private Vector3 AnotherCenter{
		get{
			return Center + HeightOffSet;
		}
	}
	public bool showLabel = true;

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
	
	private void CreateCanvasLabel(){
		if(showLabel){
			canvas = GetComponentInChildren<Canvas>();
			canvas.transform.localPosition = transform.position;
			label = Instantiate<Text>(cellLabelPrefab);
			// label.rectTransform.anchoredPosition =
			// 		new Vector2(Center.x, Center.z);
			label.rectTransform.SetParent(canvas.transform, false);
			label.transform.position = canvas.transform.position;
			count++;
		}
	}
	private void DrawLabel(){
		if(showLabel){
			if(canvas == null)
				CreateCanvasLabel();
			var pos = transform.localPosition+Vector3.up*targetHeight;
			canvas.transform.localPosition = pos+Vector3.up*1.5f;
			label.text = this.name;
		}
	}
    void Awake() {
		metric = new Metrics(numVertices,oriented);
		neighbors = new Neighbors(numVertices,metric.NeighborsDirections.ToArray());
		CreateCanvasLabel();
		vertices = new List<Vector3>();
		triangles = new List<int>();
		colors = new List<Color>();
		mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        Triangulate();
    	UpdateMesh();
		LerpHight();
		DrawLabel();
		if(!showLabel){
			count++;
			this.name = count.ToString();
		}
    }
   
	void LateUpdate() {
		UpdateMesh();
		LerpHight();
	}

	private void LerpHight(){
		if(height != targetHeight){
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
			AddTriangleUpFace(i,color);
			AddTriangleDownFace(i,color);
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
				Center + metric.Corners[i+1],
				Center + metric.Corners[i],
				Center
			);
		}else{
			AddTriangle(
				Center,
				Center + metric.Corners[i],
				Center + metric.Corners[i+1]
			);
		}
		AddTriangleColor(c);
	}
	private void AddQuadFace(int i, Color c1, Color c2){
		if(isUpDirection){
			AddQuad(
				Center + metric.Corners[i+1],
				Center + metric.Corners[i],
				targetHeight
			);
		}else{
			AddQuad(
				Center + metric.Corners[i],
				Center + metric.Corners[i+1],
				targetHeight
			);
		}
		AddQuadColor(c1,c2);
	}
	public Cell GetNeighbor (Direction direction) {
		return neighbors[(int)direction];
		
	}
	public void SetNeighbor (Direction direction, Cell cell) {
		neighbors[(int)direction] = cell ;
		Debug.Log($"{this.name} {direction} -> {direction.Opposite(oriented)}");
		cell.neighbors[(int)direction.Opposite(oriented)] = this;
	}
	public void RefreshNeighbor(Direction direction, Cell cell){
		var left = neighbors[(int)direction.Left()];
		var right = neighbors[(int)direction.Right()];
		if(left != null){
			cell.SetNeighbor(direction.Right().Opposite(oriented),left);
		}
		if(right != null){
			cell.SetNeighbor(direction.Left().Opposite(oriented),right);
		}
	}

	public void GenerateNeighbors(Cell prefab, Grid grid, int count = 1){
		MAX--;
		if(MAX<0)
			return;
			// yield return new WaitForSeconds(0);
		var current = this;
		var pos = current.transform.localPosition+Vector3.up*(50-MAX + 2);
		for(int i=0; i<current.numVertices; i++){
			current.color = Color.green;
			// yield return new WaitForSeconds(2f);
			var dir = (Direction)current.metric.NeighborsDirections[i];
			if(current.neighbors[(int)dir]== null){
				var cell = Instantiate<Cell>(prefab);
				current.SetNeighbor(dir,cell);
				cell.transform.SetParent(grid.transform);
				cell.transform.localPosition = current.metric.GenerateNeig(pos)[i];
			}
		}
		if(count>0){
			for(var d=Direction.N; d<=Direction.NW; d++){
				Cell n = current.neighbors[(int)d];
				if(n!= null){
					n.color = Color.black;
					current.RefreshNeighbor(d,n);
				}
			}
			// for(var d=Direction.N; d<Direction.NW; d++){
			// 	Cell n = current.neighbors[(int)d];
			// 	if(n!= null){
			// 		n.GenerateNeighbors(prefab,grid,count-1);
			// 	}
			// }
		}
	}

	public static void GenerateNeighbors(Grid grid,Cell prefab, int rings = 1){
		Cell current = Instantiate<Cell>(prefab);
		current.transform.localPosition = grid.transform.position;
		current.color = Color.red;
		current.transform.SetParent(grid.transform);

		current.GenerateNeighbors(prefab,grid);
	}

}



public class Neighbors{
	private Cell[] _cells;
	private int[] _reference;
	
	public Neighbors(int len, int[] reference){
		this._cells = new Cell[len];
		this._reference = reference;
	}

	public Cell this[int i] {
		get=> _cells[_reference[i]];
		set => _cells[_reference[i]] = value;
	}
	public Cell this[Direction d] {
		get=> _cells[_reference[(int)d]];
		set => _cells[_reference[(int)d]] = value;
	}
}