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
	public Cell[] neighbors;
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
		neighbors = new Cell[numVertices];
		CreateCanvasLabel();
		vertices = new List<Vector3>();
		triangles = new List<int>();
		colors = new List<Color>();
		mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
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
		for (int i = 0; i < numVertices; i++) {
			int dir = (i-2)%numVertices;
			dir = dir<0? numVertices + dir : dir;
			AddTriangleUpFace(i, neighbors[dir]!=null? color: Color.red);
			AddTriangleDownFace(i,neighbors[dir]!= null? color: Color.red);
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
				Center + metric.Corners[i+1],
				Center + metric.Corners[i],
				Center
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
				AnotherCenter,
				AnotherCenter + metric.Corners[i],
				AnotherCenter + metric.Corners[i+1]
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
	public Cell GetNeighbor (int direction) {
		return neighbors[direction];
		
	}
	public void SetNeighbor (int direction, Cell cell) {
		neighbors[(int)direction] = cell ;
		cell.neighbors[direction.HexOpposite()] = this;
	}
	public void RefreshNeighbor(int direction, Cell cell){
		var left = neighbors[direction.HexLeft()];
		var right = neighbors[direction.HexRight()];
		if(	cell.neighbors[direction.HexDoubleLeft()] == null && left != null){
			cell.SetNeighbor(direction.HexDoubleLeft(),left);
		}
		if(	cell.neighbors[direction.HexDoubleRight()] == null && right != null){
			cell.SetNeighbor(direction.HexDoubleRight(),right);
		}
		
	}

	public IEnumerator<WaitForSeconds> GenerateNeighbors(Cell prefab, Grid grid, int rings){
		MAX--;
		if(MAX<0)
			// return;
			yield return new WaitForSeconds(0);
		Cell current = this;
		Queue< (Cell,int)> toRefresh = new Queue<(Cell,int)>();
		Queue<Cell> toGenerate = new Queue<Cell>();
		toGenerate.Enqueue(current);
		grid.cells.Add(current);
		int qtdCells = numVertices*(2+rings-1)*rings/2;
		while(toGenerate.Any() && qtdCells>0){
			current = toGenerate.Dequeue();
			var pos = current.transform.localPosition+Vector3.up*(rings);
			for(int dir=0; dir<numVertices; dir++){
				if(current.neighbors[dir] == null){
					yield return new WaitForSeconds(0.3f);
					var cell = Instantiate<Cell>(prefab);
					current.SetNeighbor(dir,cell);
					cell.transform.SetParent(grid.transform);
					cell.transform.localPosition = current.metric.GenerateNeig(pos)[dir];
					cell.transform.SetParent(grid.transform);
					toRefresh.Enqueue((cell,dir));
					grid.cells.Add(cell);
					qtdCells--;
				}
			}
			while(toRefresh.Any()){
				var tuple = toRefresh.Dequeue();
				Cell neig = tuple.Item1;
				int dir = tuple.Item2;
				current.RefreshNeighbor(dir,neig);
				toGenerate.Enqueue(neig);
			}
		}
	}

	public static IEnumerator<WaitForSeconds> GenerateNeighbors(Grid grid,Cell prefab, int rings = 1){
		Cell current = Instantiate<Cell>(prefab);
		current.transform.localPosition = grid.transform.position;
		current.transform.SetParent(grid.transform);
		return current.GenerateNeighbors(prefab,grid,rings);
	}

}



// public class Neighbors{
// 	private Cell[] _cells;
// 	private int[] _reference;
	
// 	public Neighbors(int len, int[] reference){
// 		this._cells = new Cell[len];
// 		this._reference = reference;
// 	}

// 	public Cell this[int i] {
// 		get=> _cells[_reference[i]];
// 		set => _cells[_reference[i]] = value;
// 	}
// 	public Cell this[Direction d] {
// 		get=> _cells[_reference[(int)d]];
// 		set => _cells[_reference[(int)d]] = value;
// 	}
// }