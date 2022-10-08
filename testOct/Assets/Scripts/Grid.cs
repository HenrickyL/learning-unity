using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public int width = 6;
	public int height = 6;
	public Cell cellPrefab;
    Cell[] cells;
    public Text cellLabelPrefab;
    


    void Awake () {
		
		cells = new Cell[height * width];
		var metric  = new Metrics(8);
		Cell.CreateCell(metric);
	}
	


    

}
