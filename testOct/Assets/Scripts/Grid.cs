using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    private int rings =2;
    private float angle;
	public Cell cellPrefab;
    public bool oriented = true;
    public List<Cell> cells;
    public Cell center;
    public float cellLenght {get;}
    public Text cellLabelPrefab;
    private Metrics metric;
    [Range(0,100)] public int radius = 5;
    private float cellEdgeSize {get{return metric.EdgeSize;}}

    void Awake () {
        metric = new Metrics(cellPrefab.numVertices, oriented);
        angle = 2*Mathf.Atan(cellEdgeSize/radius);
        rings = (int)(90*Mathf.Deg2Rad/angle);
        Debug.Log($"rings: {rings} - angle: {angle} - edge: {cellEdgeSize}");
		StartCoroutine(Cell.GenerateNeighbors(this, cellPrefab,rings,angle));
	}
    private void RotateCells(){
        if(center && center.neighbors[1]){
            center.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            var reference = Vector3.right - center.transform.position;
            var neighPos = center.neighbors[1].transform.position - center.transform.position;
            float angle = Vector3.Angle(reference, neighPos);
            var currentRot = center.neighbors[1].transform.localRotation;
            center.neighbors[1].transform.localRotation = new Quaternion(currentRot.x+angle*Mathf.Rad2Deg,currentRot.y,currentRot.z,0f);
        }
    }

    private IEnumerator<WaitForSeconds> Clear(){
        foreach (var cell in cells)
        {
            Destroy(cell.gameObject);
            yield return new WaitForSeconds(0.1f);
        }
        cells.Clear();
    }


    private void Update() {
        if(Input.GetKeyUp(KeyCode.A)){
            RotateCells();
        }

        if(Input.GetKeyUp(KeyCode.Space)){
            Debug.Log(">>>>");
            StartCoroutine(Clear());
            if(!cells.Any()){
		        StartCoroutine(Cell.GenerateNeighbors(this, cellPrefab,rings,angle));
            }
        }
    }
	


    

}
