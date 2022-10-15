using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public int rings = 3;
	public Cell cellPrefab;
    public bool oriented = true;
    public List<Cell> cells;
    public Text cellLabelPrefab;


    void Awake () {
		StartCoroutine(Cell.GenerateNeighbors(this, cellPrefab,rings));

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
        if(Input.GetKeyUp(KeyCode.Space)){
            Debug.Log(">>>>");
            StartCoroutine(Clear());
            if(!cells.Any()){
		        StartCoroutine(Cell.GenerateNeighbors(this, cellPrefab,rings));
            }
        }
    }
	


    

}
