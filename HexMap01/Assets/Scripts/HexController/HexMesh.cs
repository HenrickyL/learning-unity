using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    Mesh hexMesh;
    List<Vector3> vertices;
    List<int> triangles;
    List<Color> colors;
    MeshCollider meshCollider;

    
    private void Awake() {
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        //We need to add a collider to the grid 
        meshCollider = gameObject.AddComponent<MeshCollider>();
        hexMesh.name = "Hex Mesh";
        vertices = new List<Vector3>();
        colors = new List<Color>();
        triangles = new List<int>();
    }

    public void Triangulate(HexCell[] cells){
        hexMesh.Clear();
        vertices.Clear();
        colors.Clear();
        triangles.Clear();
        for (int i = 0; i < cells.Length; i++){
            Triangulate(cells[i]);
        }
        hexMesh.vertices = vertices.ToArray();
        hexMesh.colors = colors.ToArray();
        hexMesh.triangles = triangles.ToArray();
        hexMesh.RecalculateNormals();
        //Assign our mesh to the collider after we finished triangulating.
        meshCollider.sharedMesh = hexMesh;
	}
    private void Triangulate(HexCell cell){
        for(HexDirection d = HexDirection.NE ; d<= HexDirection.NW; d++){
            Triangulate(d,cell);
        }
    }
    private void Triangulate(HexDirection direction, HexCell cell){
        Vector3 center = cell.transform.localPosition;
        Vector3 v1 = center + HexMetrics.GetFirstSolidCorner(direction);
        Vector3 v2 = center + HexMetrics.GetSecondSolidCorner(direction);

        AddTriangle(center, v1,v2);
        AddTriangleColor(cell.material.color);

        Vector3 bridge = HexMetrics.GetBridge(direction);
        Vector3 v3 = v1+bridge;
        Vector3 v4 = v2 + bridge;
        
        if(direction <= HexDirection.SE)
         TriangulateConnection(direction,cell,v1,v2);
    }

    private void TriangulateConnection(
        HexDirection direction, HexCell cell, Vector3 v1, Vector3 v2
    ){
        HexCell neighbor = cell.GetNeighbor(direction);
        if(neighbor == null){
            return;
        }
        Vector3 bridge = HexMetrics.GetBridge(direction);
		Vector3 v3 = v1 + bridge;
		Vector3 v4 = v2 + bridge;
		AddQuad(v1, v2, v3, v4);
		AddQuadColor(cell.material.color, neighbor.material.color);

        HexCell nextNeighbor = cell.GetNeighbor(direction.Next());
        if(direction <= HexDirection.E && nextNeighbor != null){
            AddTriangle(v2,v4,v2+HexMetrics.GetBridge(direction.Next()));
            AddTriangleColor(cell.material.color,neighbor.material.color, nextNeighbor.material.color);
        }
    }

    private void AddTriangleColor(Color c1,Color c2, Color c3)
    {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
    }
    private void AddTriangleColor(Color color)
    {
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
    }

    private void AddTriangle(Vector3 v1,Vector3 v2, Vector3 v3){
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex+1);
        triangles.Add(vertexIndex+2);
    }

    private void AddQuad(Vector3 v1,Vector3 v2,Vector3 v3, Vector3 v4){
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        vertices.Add(v4);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex+2);
        triangles.Add(vertexIndex+1);
        triangles.Add(vertexIndex+1);
        triangles.Add(vertexIndex+2);
        triangles.Add(vertexIndex+3);
    }
    private void AddQuadColor(Color c1,Color c2){
        colors.Add(c1);
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c2);
    }
}
