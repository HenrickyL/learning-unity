using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public int rings = 3;
	public Cell cellPrefab;
    public bool oriented = true;
    Cell[] cells;
    public Text cellLabelPrefab;
    


    void Awake () {
		// StartCoroutine(Cell.GenerateNeighbors(this, cellPrefab,1));
		Cell.GenerateNeighbors(this, cellPrefab,1);

	}


    private void Update() {
        if(Input.GetMouseButton(0)){
        }
    }
	


    

}
